using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

public static partial class AssemblyUtil
{
    /// <summary>
    /// 编译执行代码,返回结果
    /// </summary>
    public static object CompileAndExec(string code)
    {
        if (IsEquation(code))
        {
            return EquationParser(code);
        }
        else if(IsMethod(code))
        {
            return MethodParser(code);
        }
        else 
        {
            return MemberParser(code);
        }
    }
    /// <summary>
    /// 编译
    /// </summary>
    public static Assembly NewAssembly(string code)
    {
        //创建编译器实例。  
        var provider = new CSharpCodeProvider();
        //设置编译参数。  
        var cp = new CompilerParameters();
        cp.GenerateExecutable = false;
        cp.GenerateInMemory = true;

        // Generate an executable instead of 
        // a class library.
        //cp.GenerateExecutable = true;

        // Set the assembly file name to generate.
        cp.OutputAssembly = "f:\\1.dll";

        // Generate debug information.
        cp.IncludeDebugInformation = true;


        // Save the assembly as a physical file.
        cp.GenerateInMemory = false;

        // Set the level at which the compiler 
        // should start displaying warnings.
        cp.WarningLevel = 3;

        // Set whether to treat all warnings as errors.
        cp.TreatWarningsAsErrors = false;

        // Set compiler argument to optimize output.
        cp.CompilerOptions = "/optimize";

        cp.ReferencedAssemblies.Add("System.dll");
        //cp.ReferencedAssemblies.Add("System.Core.dll");
        cp.ReferencedAssemblies.Add("System.Data.dll");
        //cp.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
        cp.ReferencedAssemblies.Add("System.Deployment.dll");
        cp.ReferencedAssemblies.Add("System.Design.dll");
        cp.ReferencedAssemblies.Add("System.Drawing.dll");
        cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");

        //编译代码。  
        CompilerResults result = provider.CompileAssemblyFromSource(cp, code);
        if (result.Errors.Count > 0)
        {
            for (int i = 0; i < result.Errors.Count; i++)
                Console.WriteLine(result.Errors[i]);
            Console.WriteLine("error");
            return null;
        }

        //获取编译后的程序集。  
        Assembly assembly = result.CompiledAssembly;

        return assembly;
    }
}