//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityCore;
//using UnityEngine;

///// <summary>
///// 材质属性获取,设置接口
///// </summary>
//public class MaterialProperty : MonoBehaviour
//{
//    [TextBox("属性名称")]
//    public string propertyName;
//    int PropertyID
//    {
//        get
//        {
//            if (mPropertyID == 0) mPropertyID = Shader.PropertyToID(propertyName);
//            return mPropertyID;
//        }
//    }
//    int mPropertyID;
//    MaterialPropertyBlock mpb;
//    public MaterialPropertyBlock Mpb
//    {
//        get
//        {
//            if (mpb == null)
//            {
//                mpb = new MaterialPropertyBlock();
//                if (mRender != null) mRender.GetPropertyBlock(mpb);
//            }
//            return mpb;
//        }
//    }

//    public Renderer mRender;
//    public Renderer Render
//    {
//        get
//        {
//            if (mRender == null)
//            {
//                mRender = GetComponent<Renderer>();
//            }
//            return mRender;
//        }
//        set
//        {
//            mRender = value;
//            mRender.GetPropertyBlock(mpb);
//        }
//    }

//    void Awake()
//    {
//        var a = Mpb;
//    }

//    public Vector4 Vector4Value
//    {
//        get
//        {
//            return Mpb.GetVector(PropertyID);
//        }
//        set
//        {

//            Mpb.SetVector(PropertyID, value);
//            mRender.SetPropertyBlock(Mpb);
//        }
//    }

//    public float FloatValue
//    {
//        get
//        {
//            return Mpb.GetFloat(PropertyID);
//        }
//        set
//        {
//            Mpb.SetFloat(PropertyID, value);
//            if (mRender != null)
//            {
//                mRender.SetPropertyBlock(Mpb);
//            }
//        }
//    }

//    public Color ColorValue
//    {
//        get
//        {
//            return Mpb.GetColor(PropertyID);
//        }
//        set
//        {
//            Mpb.SetColor(PropertyID, value);
//            mRender.SetPropertyBlock(Mpb);
//        }
//    }

//    public Matrix4x4 Matrix4x4Value
//    {
//        get
//        {
//            return Mpb.GetMatrix(PropertyID);
//        }
//        set
//        {
//            Mpb.SetMatrix(PropertyID, value);
//            mRender.SetPropertyBlock(Mpb);
//        }
//    }
//}
