using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
/// <summary>
/// 程序说明：C#动态编译计算表达式的值，是通过System.CodeDom.Compiler命名空间下的相关类来实现的,如：CSharpCodeProvider类。步骤如下：
/// 1.将表达式封装成为可编译的C#代码；
/// 2.动态编译C#代码，生成Assembly程序集。
/// 3.使用.NET反射调用方法计算表达式的值。
/// </summary>
public static class CodeDomExpression
{
    /// <summary>
    /// 算术表达式求结果
    /// </summary>
    public static decimal Calculate(string expression)
    {
        try
        {
            return decimal.Parse(DoCalculate(expression).ToString());
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// 计算表达式的值
    /// </summary>
    /// <param name="expression">表达式</param>
    /// <returns></returns>
    private static object DoCalculate(string expression)
    {
        string code = DoWrapCode(expression);

        CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
        CompilerParameters compilerParameters = new CompilerParameters();
        compilerParameters.CompilerOptions = "/t:library";
        compilerParameters.GenerateInMemory = true;

        //编译C#源码,稍有停顿！
        CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, code);
        if (compilerResults.Errors.Count > 0)
            throw new Exception("编译表达式出错！");

        Assembly assembly = compilerResults.CompiledAssembly;
        Type type = assembly.GetType("ExpressionCalculate");
        MethodInfo method = type.GetMethod("Calculate");
        return method.Invoke(null, null);
    }

    /// <summary>
    /// 封装C#代码
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    private static string DoWrapCode(string expression)
    {
        string code = @"
      using System;
      public class ExpressionCalculate
      {
         public static object Calculate()
         {
            return {0};
         }
         }";

        return code.Replace("{0}", expression);
    }

}