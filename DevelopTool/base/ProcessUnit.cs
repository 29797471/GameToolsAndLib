using CqCore;
using System.Collections.Generic;
using System;
using System.Windows;
/*
机器学习:
通过结果 调整估价函数
通过估价函数完成ai策略
对棋牌游戏的博弈而言,把对己方的增益和对敌方的减益 放在一个函数里面估价，这个数学模型有依据吗

人为定义价值函数，分活二，活三，眠二，眠三，死二，死三是不全面，不准确的。
应该对一个点在所在的其中一条直线上，能否形成五连中的一点，形成还需要几个棋子,会有几种组合,进行打分(这样的数学模型含概了任何情况，包括五子棋界没有定义的眠一,跳二)
紧靠的同色棋子在同一直线上应该是价值共享的。

Ai和图形识别应该以数字找规律为基础
1,4,9,下一个数字是什么
计算机
将离散的数据存储转化为函数是AI的基础
*/
/// <summary>
/// 处理单元
/// </summary>
public class ProcessUnit
{
    /*
    Dictionary<float, float> dic;
    List<float> list;
    ICurve curve;

    object funcData;
    */
    public ProcessUnit()
    {
    }

    ///找规律
    public void FindFunc()
    {

    }
}

/// <summary>
/// 
///var c = new QuadraticCurve(new Vector() { x = 1, y = 2 }, new Vector() { x = 2, y = 5 }, new Vector() { x = 3, y = 10 });
///var y = c.Evaluate(4);
///var y2 = c.Evaluate(5);
/// 二次函数曲线
/// </summary>
public class QuadraticCurve : ICurve
{
    /// <summary>
    /// 函数系数
    /// </summary>
    Matrix a;
    /// <summary>
    /// 函数值
    /// </summary>
    public double Evaluate(double x)
    {
        return (XX(x) * a)[0, 0];
    }
    /// <summary>
    /// 由3个点推导二次函数
    /// </summary>
    public QuadraticCurve(Vector p1, Vector p2, Vector p3)
    {
        SquareMatrix A = (XX(p1.x).Transpose() | XX(p2.x).Transpose() | XX(p3.x).Transpose()).Transpose().ToSquareMatrix();
        Matrix B = new Matrix(new double[1, 3] { { p1.y, p2.y, p3.y } }).Transpose();
        a = A.Inverse() * B ;
    }
    public Matrix XX(double x)
    {
        return new Matrix(new double[1, 3] { { x * x, x, 1 } });
    }

}
public interface ICurve
{
    double Evaluate(double x);
}
public struct Vector
{
    public double x;
    public double y;

    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector() { x = a.x + b.x, y = a.y + b.y };
    }
    public static Vector operator -(Vector a, Vector b)
    {
        return new Vector() { x = a.x - b.x, y = a.y - b.y };
    }
    public static Vector operator *(Vector a, double d)
    {
        return new Vector() { x = a.x * d, y = a.y * d };
    }
    public static Vector operator /(Vector a, double d)
    {
        return new Vector() { x = a.x / d, y = a.y / d };
    }

}
