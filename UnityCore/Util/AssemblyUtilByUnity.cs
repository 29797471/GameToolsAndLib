using System.Reflection;
using UnityEngine;

public static class AssemblyUtilByUnity
{
    static Assembly mUnityEditorAssembly;
    public static Assembly UnityEditorAssembly
    {
        get
        {
            if (mUnityEditorAssembly == null)
            {
                mUnityEditorAssembly = Assembly.LoadFile(Application.dataPath + @"\..\Library\ScriptAssemblies\Assembly-CSharp-Editor.dll");
            }
            return mUnityEditorAssembly;
        }
    }

    /// <summary>
    /// 函数表达式
    /// </summary>
    public static object InvokeMethod(this Assembly assembly, string expression, params object[] args)
    {
        var ary = expression.Split('.');
        var type=assembly.GetType(ary[0]);
        var obj= AssemblyUtil.GetStaticMemberValue(type, ary[1]);
        return AssemblyUtil.InvokeMethod(obj, ary[2], args);
    }
}