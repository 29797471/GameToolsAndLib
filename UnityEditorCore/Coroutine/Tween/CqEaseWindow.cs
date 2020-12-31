using CqCore;
using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 常用曲线编辑窗口
/// </summary>
public class CqEaseWindow : EditorWindow
{
    /// <summary>
    /// 获取常用曲线
    /// </summary>
    /// <param name="SetCurve"></param>
    public static void GetCurve(Action<AnimationCurve> SetCurve)
    {
        var win = GetWindow<CqEaseWindow>("获取常用曲线");
        win.OnResult = (result) =>
        {
            SetCurve(result ? win.mCurve : null);
        };
    }
    //[MenuItem("Assets/获取常用曲线")]
    //[MenuItem("Window/获取常用曲线")]
    static void Open()
    {
        GetWindow<CqEaseWindow>("获取常用曲线");
    }

    public CqEaseWindow()
    {
        categories = EnumUtil.GetEnumNames<EaseFunEnum>();
        styles = EnumUtil.GetEnumNames<EaseStyleEnum>();

        mSampling = 2;
        mCurve = new AnimationCurve();
        UpdateCurve();
    }

    private string[] categories;
    private string[] styles;

    int mEaseFunIndex;
    int EaseFunIndex
    {
        set
        {
            if(mEaseFunIndex!=value)
            {
                mEaseFunIndex = value;
                Sampling = (mEaseFunIndex == 0) ? 2 : 50;
            }
        }
    }

    int mStyleIndex;

    int mSampling;
    int Sampling
    {
        get
        {
            return mSampling;
        }
        set
        {
            if (value < 2) return;
            if(mSampling!=value)
            {
                mSampling = value;
            }
        }
    }

    void UpdateCurve()
    {
        var fun = UnityEngine.EaseFun.GetEase((EaseFunEnum)mEaseFunIndex, (EaseStyleEnum)mStyleIndex);
        mCurve.SetEvaluate(fun, mSampling);
    }
    
    private AnimationCurve mCurve;

    Action<bool> OnResult;

    void OnEnable()
    {
    }

    void OnDisable()
    {
        OnResult?.Invoke(false);
    }
    void OnHierarchyChange()
    {
    }
    
    void OnGUI()
    {

        
        //if (Event.current.type == EventType.ContextClick)
        //{
        //    GenericMenu genericMenu = new GenericMenu();
        //    genericMenu.AddItem(new GUIContent("None"), false, null);

        //    genericMenu.ShowAsContext();
        //    //EditorUtility.DisplayPopupMenu(new Rect(Event.current.mousePosition, Vector2.zero), "CONTEXT/Vector3/", null);
        //}

        GUILayoutUtil.Area(()=>
        {
            GUILayoutUtil.Horizontal(() =>
            {
                EaseFunIndex = EditorGUILayout.Popup(mEaseFunIndex, categories);
                mStyleIndex = EditorGUILayout.Popup(mStyleIndex, styles);
                Sampling = EditorGUILayout.IntField("采样点数量", mSampling);
            });
            GUILayout.Space(5f);
            GUILayoutUtil.Horizontal(() =>
            {
                EditorGUILayout.CurveField( mCurve,GUILayout.Height(200));
            });
            GUILayout.Space(5f);

            GUILayoutUtil.Horizontal(() =>
            {
                if (GUILayout.Button("确定"))
                {
                    this.Close();
                    OnResult(true);
                }
                if (GUILayout.Button("取消"))
                {
                    this.Close(); OnResult(false);
                }
            });
        },new Rect(5f, 5f, position.width - 10f, position.height - 10f));
        if(GUI.changed)
        {
            UpdateCurve();
        }
    }
    
}

