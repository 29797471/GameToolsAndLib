using DevelopTool;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Proto
{
    [Editor]
    public class ProtoFile : BaseTreeNotifyObject
    {
        public string file;
        new public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(file);
            }
        }

        /// <summary>
        /// 当前定义类型列表
        /// </summary>
        public List<string> TypeList
        {
            get
            {
                var typeList = new List<string>();
                ForEach((obj =>
                {
                    if (obj is ProtoMessage) typeList.Add((obj as ProtoMessage).Name);
                    else if (obj is ProtoEnum) typeList.Add((obj as ProtoEnum).Name);
                }));
                return typeList;
            }
        }

        /// <summary>
        /// 获取导入的类型列表
        /// </summary>
        public List<string> ImportTypeList
        {
            get
            {
                var list = new List<string>();
                ForEach((obj =>
                {
                    var data = obj as ProtoImport;
                    if (data != null)
                    {
                        var b = ProtoModel.instance.ProtoList.ToList().Find(x => Path.GetFileName(x.file) == Path.GetFileName(data.Import));

                        list = list.Concat(b.TypeList).ToList();
                    }
                }));

                return list;
            }
        }
    }

}


