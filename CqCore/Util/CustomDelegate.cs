using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*-------------Func 带out 和ref-----------------*/
#region FuncOut
/// <summary>
/// 最后一个参数是out的泛型委托
/// </summary>
public delegate TResult FuncOut<T1, TResult>(out T1 arg1);

/// <summary>
/// 最后一个参数是out的泛型委托
/// </summary>
public delegate TResult FuncOut<T1, T2, TResult>(T1 arg1, out T2 arg2);

/// <summary>
/// 最后一个参数是out的泛型委托
/// </summary>
public delegate TResult FuncOut<T1, T2, T3, TResult>(T1 arg1, T2 arg2, out T3 arg3);
#endregion

#region FuncOutAll

/// <summary>
/// 所有参数是out的泛型委托
/// </summary>
public delegate TResult FuncOutAll<T1, T2, TResult>(out T1 arg1, out T2 arg2);

/// <summary>
/// 所有参数是out的泛型委托
/// </summary>
public delegate TResult FuncOutAll<T1, T2, T3, TResult>(out T1 arg1, out T2 arg2, out T3 arg3);
#endregion

#region FuncRef
/// <summary>
/// 最后一个参数是ref的泛型委托
/// </summary>
public delegate TResult FuncRef<T1, TResult>(ref T1 arg1);

/// <summary>
/// 最后一个参数是ref的泛型委托
/// </summary>
public delegate TResult FuncRef<T1, T2, TResult>(T1 arg1, ref T2 arg2);

/// <summary>
/// 最后一个参数是ref的泛型委托
/// </summary>
public delegate TResult FuncRef<T1, T2, T3, TResult>(T1 arg1, T2 arg2, ref T3 arg3);
#endregion

#region FuncRefAll

/// <summary>
/// 所有参数是ref的泛型委托
/// </summary>
public delegate TResult FuncRefAll<T1, T2, TResult>(ref T1 arg1, ref T2 arg2);

/// <summary>
/// 所有参数是ref的泛型委托
/// </summary>
public delegate TResult FuncRefAll<T1, T2, T3, TResult>(ref T1 arg1, ref T2 arg2, ref T3 arg3);
#endregion

/*-------------Action-----------------*/

/// <summary>
/// 最后一个参数是out的泛型委托
/// </summary>
public delegate void ActionOut<T_out>(out T_out arg_out);

/// <summary>
/// 最后一个参数是out的泛型委托
/// </summary>
public delegate void ActionOut<T1, T_out>(T1 arg1, out T_out arg_out);

/// <summary>
/// 最后一个参数是out的泛型委托
/// </summary>
public delegate void ActionOut<T1, T2, T_out>(T1 arg1, T2 arg2, out T_out arg_out);

/// <summary>
/// 最后一个参数是ref的泛型委托
/// </summary>
public delegate void ActionRef<T_ref>(ref T_ref arg_ref);

/// <summary>
/// 最后一个参数是ref的泛型委托
/// </summary>
public delegate void ActionRef<T1, T_ref>(T1 arg1, ref T_ref arg_ref);

/// <summary>
/// 最后一个参数是ref的泛型委托
/// </summary>
public delegate void ActionRef<T1, T2, T_ref>(T1 arg1, T2 arg2, ref T_ref arg_ref);