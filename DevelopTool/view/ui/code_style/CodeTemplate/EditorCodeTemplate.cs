using DevelopTool;
using System.Linq;

[Editor("代码编辑器"), Width(700+50),Height(400+280)]
public class EditorCodeTemplate : NotifyObject, ICqSerialize
{
    public static EditorCodeTemplate ToEditor(IExpression e,EventExp ee=null)
    {
        var ect = new EditorCodeTemplate() { e = e,eExp=ee};
        ect.Init();
        return ect;
    }

    [Priority(0)]
    [Label,Margin(150,0,0,0)]
    public string Title
    {
        get
        {
            var it = CodeStyleNewModel.instance.setting.CodeSettingList.ToList().Find(x=>x.Name== Language);
            return string.Format("{0}编辑器({1})",
                it.LinkTypeList.ToList().Find(x => x.Key == StyleType).Value,
                StyleType);
        }
    }
    
    [Priority(1)]
    [RadioButtonGroup("Index",60),Margin(30,0)]
    public string[] Group
    {
        get
        {
            return new string[] { "表达式", "值", "事件成员" ,"变量","自定义"};
        }
    }
    public int Index
    {
        get { return mIndex; }
        set { mIndex = value; Update("Index"); }
    }
    public int mIndex;

    /// <summary>
    /// 返回类型
    /// </summary>
    public string StyleType
    {
        get{return mStyleType;}
    }
    public string mStyleType;

    /// <summary>
    /// 编程语言
    /// </summary>
    public string Language
    {
        get { return e.Language; }
    }
    
    [Visibility("Index", AttributeTarget.Self , "0=x")]
    [GroupBox(), Height(400), Width(700)]
    [Priority(2)]
    public ExprCodeTemplate ExpCode
    {
        get
        {
            if(expCode==null)
            {
                expCode=CodeStyleNewModel.instance.DefaultExpr(mStyleType, Language);//赋予一个默认表达式
                expCode.Ee = eExp;
            }
            return expCode;
        }
        set{expCode = value;Update("ExpCode"); }
    }
    ExprCodeTemplate expCode;
    
    [Visibility("Index", AttributeTarget.Self, "1=x")]
    [GroupBox(),Height(400),Width(700)]
    [Priority(2)]
    public ValueCodeTemplate ValueCode
    {
        get
        {
            if(valueCode==null)
            {
                valueCode = CodeStyleNewModel.instance.DefaultValue(mStyleType, Language);
            }
            return valueCode;
        }
        set{valueCode = value;Update("ValueCode");  }
    }
    ValueCodeTemplate valueCode;

    [Visibility("Index", AttributeTarget.Self, "2=x")]
    [GroupBox(), Height(400), Width(700)]
    [Priority(2)]
    public EventArgCodeTemplate EventArg
    {
        get
        {
            return eventArg;
        }
        set
        {
            eventArg = value;
            Update("EventArg");
        }
    }
    EventArgCodeTemplate eventArg;

    [Visibility("Index", AttributeTarget.Self, "3=x")]
    [GroupBox(), Height(350), Width(700)]
    [Priority(2)]
    public VarCodeTemplate Var
    {
        get
        {
            if(mVar==null)
            {
                mVar=  new VarCodeTemplate() { StyleType = mStyleType, Language = Language };//变量
            }
            return mVar;
        }
        set
        {
            mVar = value;
            Update("Var");
        }
    }
    VarCodeTemplate mVar;

    [Visibility("Index", AttributeTarget.Self, "4=x")]
    [GroupBox(), Height(350), Width(700)]
    [Priority(2)]
    public CustomCodeTemplate CustomCT
    {
        get
        {
            if(mCustomCT==null)
            {
                mCustomCT = new CustomCodeTemplate() { StyleType = mStyleType, Language = Language };
            }
            return mCustomCT;
        }
        set
        {
            mCustomCT = value;
            Update("CustomCT");
        }
    }
    CustomCodeTemplate mCustomCT;

    public EventExp eExp;

    public IExpression e;



    public void Init()
    {
        mStyleType = e.StyleType;
        if (e is ExprCodeTemplate)
        {
            var c=(e as ExprCodeTemplate);
            mIndex = 0;

            ExpCode = c;
            if (ExpCode != null) ExpCode.Ee = eExp;

            if (eExp != null)
            {
                EventArg = new EventArgCodeTemplate();
                EventArg.Ee = eExp;
                EventArg.StyleType = mStyleType;
            }
        }
        else if (e is ValueCodeTemplate)
        {
            var c=(e as ValueCodeTemplate);
            mIndex = 1;

            ValueCode = c;

            if (eExp != null)
            {
                EventArg = new EventArgCodeTemplate();
                EventArg.Ee = eExp;
                EventArg.StyleType = mStyleType;

            }
        }
        else if (e is EventArgCodeTemplate)
        {
            var c=(e as EventArgCodeTemplate);
            mIndex = 2;

            if (eExp != null)
            {
                EventArg = new EventArgCodeTemplate();
                EventArg.Ee = eExp;
                EventArg.StyleType = mStyleType;
            }
        }
        else if (e is VarCodeTemplate)
        {
            var c=(e as VarCodeTemplate);
            mIndex = 3;
            Var = c;
        }
        else if(e is CustomCodeTemplate)
        {
            var c=(e as CustomCodeTemplate);
            mIndex = 4;

            CustomCT = c;
        }
    }

    public IExpression GetValue()
    {
        switch (Index)
        {
            case 0:
                return ExpCode;
            case 1:
                return ValueCode;
            case 2:
                return EventArg;
            case 3:
                return Var;
            case 4:
                return CustomCT;
        }
        return null;
    }

    public void OnDeserialize()
    {
        Init();
    }

    public void OnSerialize()
    {
    }
}