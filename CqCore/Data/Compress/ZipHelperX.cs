using ICCEmbedded.SharpZipLib.Checksum;
using ICCEmbedded.SharpZipLib.Zip;
using System;
using System.IO;


namespace CqCore
{
    public class ZipHelperX
    {
        // 0 - store only to 9 - means best compression
        public enum CompressLevel
        {
            Store = 0,
            Level1,
            Level2,
            Level3,
            Level4,
            Level5,
            Level6,
            Level7,
            Level8,
            Best
        }

        private const int BUFFER_SIZE = 1024 * 4;

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        public static int ZipFileDirectory(string destFolder, string srcFolder,
            string[] skipsDir = null, string[] skipsExt = null,
            string password = null, CompressLevel level = CompressLevel.Level1)
        {
            int count = 0; ZipOutputStream zipStream = null;

            try
            {
                if (File.Exists(destFolder))
                {
                    File.Delete(destFolder);
                }

                FileStream ZipFile = File.Create(destFolder);

                zipStream = new ZipOutputStream(ZipFile);

                zipStream.Password = password;
                zipStream.SetLevel(Convert.ToInt32(level));

                count = ZipSetp(srcFolder, null, skipsDir, skipsExt, zipStream, new Crc32());
            }
            catch (Exception e)
            {
                CqDebug.Log(e.ToString(),LogType.Error);
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
            }

            return count;
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        private static int ZipSetp(string strDirectory, string logicBaseDir,
            string[] skipsDir, string[] skipsExt, ZipOutputStream zipStream, Crc32 crc32)
        {
            int count = 0;

            string logicDir = null;
            if (logicBaseDir == null)
            {
                logicBaseDir = strDirectory;
                logicDir = @"";
            }
            else
            {
                logicDir = strDirectory.Substring(logicBaseDir.Length) + "/";
            }

            if (logicDir.Length > 0)
            {
                ZipEntry dirEntry = new ZipEntry(logicDir);
                dirEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(dirEntry);
            }

            int skipDirLength = skipsDir != null ? skipsDir.Length : 0;
            //Get Directories Name
            string[] dirs = Directory.GetDirectories(strDirectory);
            for (int i = 0; i < dirs.Length; ++i)
            {
                bool isSkiped = false;

                for (int j = 0; j < skipDirLength; ++j)
                {
                    if (dirs[i].Contains(skipsDir[j]))
                    {
                        CqDebug.Log(string.Format("skip dir {0}", dirs[i]));
                        isSkiped = true; break;
                    }
                }

                if (isSkiped) continue;

                count += ZipSetp(dirs[i], logicBaseDir, skipsDir, skipsExt, zipStream, crc32);
            }

            #region 遍历所有的文件

            int skipExtLength = skipsExt != null ? skipsExt.Length : 0;
            // 遍历所有的文件和目录
            string[] files = Directory.GetFiles(strDirectory);
            for (int i = 0; i < files.Length; ++i)
            {
                string file = files[i];

                //if (Path.GetExtension(file).Length < 1) continue;

                // 没有扩展名的文件不包含进来
                bool isSkiped = false;

                //文件后缀过滤
                for (int j = 0; j < skipExtLength; ++j)
                {
                    if (files[i].EndsWith(skipsExt[j]))
                    {
                        isSkiped = true; break;
                    }
                }

                if (isSkiped) continue;

                //Debug.Log("to zip file" + file);

                //打开压缩文件
                using (FileStream fs = File.OpenRead(file))
                {
                    //Read the file to stream
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    string fileName = Path.Combine(logicDir, Path.GetFileName(file));

                    //Specify ZipEntry
                    crc32.Reset();
                    crc32.Update(buffer);
                    ZipEntry zipEntry = new ZipEntry(fileName);
                    zipEntry.DateTime = DateTime.Now;
                    zipEntry.Size = buffer.Length;
                    zipEntry.Crc = crc32.Value;

                    fs.Close();

                    //Put file info into zip stream
                    zipStream.PutNextEntry(zipEntry);

                    //Put file data into zip stream
                    zipStream.Write(buffer, 0, buffer.Length);

                    count++;
                }
            }

            #endregion

            return count;
        }

        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">指定解压目标目录</param>
        /// <param name="UnZipProgress">解压进度</param>
        /// <param name="memStream">解压流</param>
        public static bool UnZip(string FileToUpZip, string ZipedFolder, Action<float> UnZipProgress, Stream memStream = null)
        {
            bool res = true;

            if (memStream == null)
            {
                if (!File.Exists(FileToUpZip))
                {
                    CqDebug.Log(FileToUpZip + " is not exist",LogType.Error); return false;
                }
            }

            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }

            ZipEntry theEntry = null;
            ZipInputStream zipInputS = null;

            string fileName;
            FileStream streamWriter = null;

            try
            {
                if (memStream == null)
                {
                    memStream = File.OpenRead(FileToUpZip);
                }

                long zipIndex = 0;
                long zipLeng = memStream.Length;
                long modval = zipLeng / 100;

                zipInputS = new ZipInputStream(memStream);

                //一个一个进行解压
                while ((theEntry = zipInputS.GetNextEntry()) != null)
                {
                    if (string.IsNullOrEmpty(theEntry.Name)) continue;

                    fileName = Path.Combine(ZipedFolder, theEntry.Name);

                    //判断文件路径是否是文件夹
                    if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                    {
                        Directory.CreateDirectory(fileName); continue;
                    }

                    //创建没有的目录
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    directoryName = Path.Combine(ZipedFolder, directoryName);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    //继续解压下一个文件
                    streamWriter = File.Create(fileName);
                    int size = BUFFER_SIZE;
                    byte[] data = new byte[BUFFER_SIZE];

                    while (true)
                    {
                        size = zipInputS.Read(data, 0, data.Length);

                        if (size <= 0) break;

                        streamWriter.Write(data, 0, size);

                        if (zipInputS.Position >= zipIndex)
                        {
                            zipIndex += modval;

                            if (UnZipProgress != null)
                            {
                                UnZipProgress(zipIndex * 1f / zipLeng);
                            }
                        }
                    }

                    streamWriter.Close(); streamWriter = null;
                }

                //解压完成
                if (UnZipProgress != null) UnZipProgress(1);
            }
            catch (Exception ex)
            {
                CqDebug.Log("zip error:" + ex.Message,LogType.Error); res = false;
            }
            finally
            {

                if (memStream != null)
                {
                    memStream.Close();
                    memStream = null;
                }

                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (zipInputS != null)
                {
                    zipInputS.Close();
                    zipInputS = null;
                }

                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }
    }
}

