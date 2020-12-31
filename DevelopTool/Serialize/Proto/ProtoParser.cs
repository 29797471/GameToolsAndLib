using ParserCore;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Proto;

public class ProtoParser : TokenParser
{
    ProtoFile p;
    public ProtoParser(List<Token> list) : base(list)
    {

    }
    string lastComment;
    public bool TryParseProto( out ProtoFile p)
    {
        p = new ProtoFile();
        this.p = p;
        while (!IsEnd())
        {
            object data;
            if (Value.type == TokenType.SEMICOLON )
            {
                Next(); 
            }
            else if (Value.type == TokenType.COMMENT)
            {
                lastComment = Value.value.ToString();
                NextSkipComment();
            }
            else if (TryParseLogic(TryProtoSyntax, out data))
            {
                p.AddChildren(data as BaseTreeNotifyObject);
            }
            else if(TryParseLogic(TryParsePackage, out data))
            {
                p.AddChildren(data as BaseTreeNotifyObject);
            }
            else if (TryParseLogic(TryParseEOption, out data))
            {
                p.AddChildren(data as BaseTreeNotifyObject);
            }
            else if (TryParseLogic( TryParseImport,out data))
            {
                p.AddChildren(data as BaseTreeNotifyObject);
            }
            else if(TryParseLogic(TryParseMeaage, out data))
            {
                p.AddChildren(data as BaseTreeNotifyObject);
            }
            else if (TryParseLogic(TryParseProtoEnumNode,out data)) 
            {
                p.AddChildren(data as BaseTreeNotifyObject);
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// package XX;
    /// </summary>
    public bool TryParsePackage(out object package)
    {
        package = null;
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "package") return false;
        NextSkipComment();
        if (Value.type != TokenType.VARIABLE) return false;
        package = new ProtoPackage() { Package = Value.value.ToString() };
        NextSkipComment();
        if (Value.type != TokenType.SEMICOLON) return false;
        //NextSkipComment();

        return true;
    }
    /// <summary>
    /// syntax = "proto2";
    /// </summary>
    public bool TryProtoSyntax(out object data)
    {
        data = null;
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "syntax") return false;
        NextSkipComment();
        if (Value.type != TokenType.EQUAL) return false;
        NextSkipComment();
        switch (Value.type)
        {
            case TokenType.STRING:
                data = new ProtoSyntax() {  mSyntax = "\"" + Value.value.ToString() + "\"" };
                NextSkipComment();
                if (Value.type != TokenType.SEMICOLON) return false;
                //NextSkipComment();
                return true;
            default:
                return false;
        }
    }
    
    /// <summary>
    /// option optimize_for = LITE_RUNTIME;
    /// </summary>
    public bool TryParseEOption( out object option)
    {
        option = null;
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "option") return false;
        NextSkipComment();
        if (Value.type != TokenType.VARIABLE ) return false;
        var key = Value.value.ToString();
        NextSkipComment();
        if (Value.type != TokenType.EQUAL) return false;
        NextSkipComment();
        switch(Value.type)
        {
            case TokenType.VARIABLE:
                option = new ProtoOption() { OKey = key, OValue = Value.value.ToString() };
                NextSkipComment();
                if (Value.type != TokenType.SEMICOLON) return false;
                //NextSkipComment();
                return true;
            case TokenType.STRING:
                option = new ProtoOption() { OKey = key, OValue = "\""+Value.value.ToString()+"\"" };
                NextSkipComment();
                if (Value.type != TokenType.SEMICOLON) return false;
                //NextSkipComment();
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// option optimize_for = LITE_RUNTIME;
    /// </summary>
    public bool TryParseImport(out object import)
    {
        import = null;
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "import") return false;
        NextSkipComment();
        if (Value.type != TokenType.STRING) return false;
        import = new ProtoImport() { Import = Value.value.ToString() };
        NextSkipComment();
        if (Value.type != TokenType.SEMICOLON) return false;
        //NextSkipComment();
        return true;
    }

    /// <summary>
    /// message XXXX
    /// </summary>
    public bool TryParseMeaage(out object message)
    {
        message = null;
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "message") return false;
        NextSkipComment();
        if (Value.type != TokenType.VARIABLE) return false;
        ProtoMessage mn = new ProtoMessage { Name = Value.value.ToString() };
        Next();
        if (Value.type == TokenType.COMMENT)
        {
            mn.Comment = Value.value.ToString();
            NextSkipComment();
        }
        else if(lastComment!=null)
        {
            mn.Comment = lastComment;
            lastComment = null;
        }
        if (Value.type != (TokenType.LEFT_BRACE)) return false;
        NextSkipComment();
        while (Value.type != (TokenType.RIGHT_BRACE))
        {
            object data;
            if (TryParseLogic(TryParseProtoEnumNode, out data))
            {
                mn.AddChildren(data as ProtoEnum);
                if (Value.type == TokenType.SEMICOLON) NextSkipComment();
            }
            else if (TryParseLogic(TryParseMeaage, out data))
            {
                mn.AddChildren(data as ProtoMessage);
                if (Value.type == TokenType.SEMICOLON) NextSkipComment();
            }
            else if(TryParseLogic( TryParseMessageExpression,out data))
            {
                mn.Expres.Add(data as ProtoExpression);
            }
            else if(TryParseLogic(TryParseExtensions, out data))
            {
                mn.Extensions = (data as ProtoExtensions).Extensions;
            }
            else 
            {
                return false;
            }
        }
        Next();
        message = mn;
        return true;
    }
    public bool TryParseExtensions(out object v)
    {
        v = null;
        ProtoExtensions pe = new ProtoExtensions();
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "extensions") return false;
        NextSkipComment();
        if (Value.type != TokenType.NUMBER) return false;
        pe.Extensions = int.Parse(Value.value.ToString());
        NextSkipComment();
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "to") return false;
        NextSkipComment();
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "max") return false;
        NextSkipComment();
        if (Value.type != TokenType.SEMICOLON) return false;
        NextSkipComment();
        return true;

    }
    public bool TryParseMessageExpression(out object v)
    {
        v = null;
        ProtoExpression me = new ProtoExpression();
        if (Value.type != TokenType.VARIABLE) return false;
        try
        {
            me.distinction = StringUtil.EnumNameToValue<EDistinction>(Value.value.ToString());
        }
        catch (Exception)
        {
            return false;
        }
        
        NextSkipComment();

        if (Value.type != TokenType.VARIABLE) return false;
        me.FieldType = Value.value.ToString();
        NextSkipComment();
        if (Value.type != TokenType.VARIABLE) return false;
        me.name = Value.value.ToString();
        NextSkipComment();

        if (Value.type != TokenType.EQUAL) return false;
        NextSkipComment();
        if (Value.type != TokenType.NUMBER) return false;
        NextSkipComment();
        if (Value.type == (TokenType.LEFT_BRACKET))//解析默认值
        {
            NextSkipComment();
            if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "default") return false;
            NextSkipComment();
            if (Value.type != TokenType.EQUAL) return false;
            NextSkipComment();
            if (Value.type != TokenType.VARIABLE && Value.type != TokenType.NUMBER) return false;
            me.defaultValue = Value.value;
            NextSkipComment();
            if (Value.type != (TokenType.RIGHT_BRACKET)) return false;
            NextSkipComment();
        }

        if (Value.type != TokenType.SEMICOLON) return false;
        Next();
        if (Value.type == TokenType.COMMENT)
        {
            me.comment = Value.value.ToString();
            NextSkipComment();
        }
        v = me;
        return true;
    }
    public bool TryParseProtoEnumNode(out object enumNode)
    {
        enumNode = null;
        if (Value.type != TokenType.VARIABLE || Value.value.ToString() != "enum") return false;
        NextSkipComment();
        if (Value.type != TokenType.VARIABLE) return false;
        ProtoEnum me = new ProtoEnum { Name = Value.value.ToString() };
        Next();
        if (Value.type == TokenType.COMMENT)
        {
            me.Comment = Value.value.ToString();
            NextSkipComment();
        }
        if (Value.type != (TokenType.LEFT_BRACE)) return false;
        NextSkipComment();
        while (Value.type != (TokenType.RIGHT_BRACE))
        {
            EnumChild child;
            if (Value.type != TokenType.VARIABLE) return false;
            child = new EnumChild {name= Value.value.ToString() };
            NextSkipComment();
            if (Value.type != TokenType.EQUAL) return false;
            NextSkipComment();
            if (Value.type != TokenType.NUMBER) return false;
            child.number = (int)Value.value;
            NextSkipComment();
            if (Value.type != TokenType.SEMICOLON) return false;
            Next();
            if (Value.type == TokenType.COMMENT)
            {
                child.comment = Value.value.ToString();
                NextSkipComment();
            }

            if (me.Childs == null) me.Childs = new ObservableCollection<EnumChild>();
            me.Childs.Add(child);
        }
        NextSkipComment();
        if (IsEnd() == false && Value.type == TokenType.SEMICOLON) NextSkipComment();
        enumNode = me;
        return true;
    }
}
