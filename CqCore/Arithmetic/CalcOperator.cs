using CqCore;

/// <summary>
/// 计算运算符
/// </summary>
public enum CalcOperator
{
    [CalcEnum("+",0, "op_Addition")]
    Add,

    [CalcEnum("-",0, "op_Subtraction", false)]
    Sub,

    [CalcEnum("*",1, "op_Multiply")]
    Mul,

    [CalcEnum("/", 1, "op_Division", false)]
    Div,

    /*
    [CalcEnum('^', 2, "op_ExclusiveOr", false)]
    Pow,
    */
}

/// <summary>
/// 条件运算符
/// </summary>
public enum ConditionOperator
{
    [CalcEnum("==", 0, "op_Equality")]
    Equality,

    [CalcEnum("!=", 0, "op_Inequality")]
    Inequality,

    [CalcEnum("<", 0, "op_LessThan")]
    LessThan,

    [CalcEnum(">", 0, "op_GreaterThan")]
    GreaterThan,

    [CalcEnum("<=", 0, "op_LessThanOrEqual")]
    LessThanOrEqual,

    [CalcEnum(">=", 0, "op_GreaterThanOrEqual")]
    GreaterThanOrEqual,
}
