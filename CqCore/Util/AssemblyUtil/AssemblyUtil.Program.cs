using System;
using System.Globalization;
using System.IO;
using System.Reflection;
public static partial class AssemblyUtil
{
    /// <summary>
    /// 嵌入 dll解决方式
    /// </summary>
    public static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
    {

        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        AssemblyName executingAssemblyName = executingAssembly.GetName();
        var resName = executingAssemblyName.Name + ".resources";

        AssemblyName assemblyName = new AssemblyName(args.Name);
        string path = "";
        if (resName == assemblyName.Name)
        {
            path = executingAssemblyName.Name + ".g.resources";
        }
        else
        {
            path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = String.Format(@"{0}\{1}", assemblyName.CultureInfo, path);
            }
        }
        using (Stream stream = executingAssembly.GetManifestResourceStream(path))
        {

            if (stream == null)return null;
            byte[] assemblyRawBytes = new byte[stream.Length];
            stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
            return Assembly.Load(assemblyRawBytes);
        }
    }
}