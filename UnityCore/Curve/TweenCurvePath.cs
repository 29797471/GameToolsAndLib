using CqCore;
using System;
using UnityCore;
using UnityEngine;

/// <summary>
/// 在路径曲线中缓动,外部调用K:0~1,物体在路径上移动
/// </summary>
[Obsolete]
public class TweenCurvePath:MonoBehaviour
{
    public Curve2D curve2D;
    bool matHasSet;
    Matrix4x4 worldMat;
    Matrix4x4 localMat;

    /// <summary>
    /// 
    /// </summary>
    [CheckBox("匀速运动"),ToolTip("true:每段贝塞尔曲线长度越长运动时间越长;\nfalse:每段贝塞尔曲线运动时间一致")]
    public bool uniformMotion;

    /// <summary>
    /// 每段贝塞尔曲线长度
    /// </summary>
    float[] lensK;

    [Button("更新缓动曲线矩阵"),Click("UpdateMat")]
    public string _1;
    public void UpdateMat()
    {
        index = -1;
        if (curve2D!=null)
        {
            worldMat = curve2D.transform.localToWorldMatrix;
            localMat = curve2D.transform.worldToLocalMatrix;
            matHasSet = true;
            if(uniformMotion)
            {
                if (curve2D.isClose)
                {
                    //每段贝塞尔曲线长度
                    lensK = new float[curve2D.list.Count+1];
                    float len = 0f;
                    for (int i=0;i< curve2D.list.Count; i++)
                    {
                        var list = curve2D.list;
                        var prev = list.GetItemByRound(i - 1);
                        var p1 = list[i];
                        var p2 = list.GetItemByRound(i + 1);
                        var next = list.GetItemByRound(i + 2);
                        var startTangent = p1 + (p2 - prev) * curve2D.a;
                        var endTangent = p2 - (next - p1) * curve2D.b;
                        len+= BezierUtil.Length(p1, startTangent, endTangent, p2);
                        lensK[i+1] = len;
                    }
                    for (int i = 0; i < lensK.Length; i++)
                    {
                        lensK[i] = lensK[i]/len;
                    }
                    //Debug.Log(Torsion.Serialize(lensK));
                }
                else
                {
                    //每段贝塞尔曲线长度
                    lensK = new float[curve2D.list.Count];
                    float len = 0f;
                    for (int i = 0; i < curve2D.list.Count-1; i++)
                    {
                        var list = curve2D.list;
                        var prev = list.GetItemByRange(i - 1);
                        var p1 = list[i];
                        var p2 = list.GetItemByRange(i + 1);
                        var next = list.GetItemByRange(i + 2);
                        var startTangent = p1 + (p2 - prev) * curve2D.a;
                        var endTangent = p2 - (next - p1) * curve2D.b;
                        len += BezierUtil.Length(p1, startTangent, endTangent, p2);
                        lensK[i + 1] = len;
                    }
                    for (int i = 0; i < lensK.Length; i++)
                    {
                        lensK[i] = lensK[i] / len;
                    }
                    //Debug.Log(Torsion.Serialize(lensK));
                }
            }
        }
    }

    float mK;
    /// <summary>
    /// 控制系数:0~1,0时在起点,1时返回起点
    /// </summary>
    public float K
    {
        set
        {
            mK = value;
            if (!matHasSet) UpdateMat();
            UpdateIndex();
        }
        get
        {
            return mK;
        }
    }
    int index=-1;
    void UpdateIndex()
    {
        if(curve2D.isClose)
        {
            if (mK == 1) mK = 0;

            int _index;
            float partK=0f;
            if (uniformMotion)
            {
                _index = MathUtil.GetIndexOfExtent(mK,out partK, lensK)-1;
                //Debug.Log(string.Format("mk:{2}  i:{0} k:{1}", _index, partK,mK));
            }
            else
            {
                var m = mK * curve2D.list.Count;
                _index = Mathf.FloorToInt(m);
                partK= m - _index;
            }

            if (index!=_index)
            {
                index = _index;
                
                var list = curve2D.list;
                var i = index;
                var prev = list.GetItemByRound(i - 1);
                var p1 = list[i];
                var p2 = list.GetItemByRound(i + 1);
                var next = list.GetItemByRound(i + 2);
                var startTangent = p1 + (p2 - prev) * curve2D.a;
                var endTangent = p2 - (next - p1) * curve2D.b;

                UpdatePos = t => Pos = BezierUtil.LerpUnclamped(p1, startTangent, endTangent, p2, t);
            }
            T = partK;
        }
        else
        {
            int _index;
            float partK = 0f;
            if (uniformMotion)
            {
                _index = MathUtil.GetIndexOfExtent(mK, out partK, lensK) - 1;
            }
            else
            {
                var m = mK * (curve2D.list.Count - 1);
                _index = Mathf.FloorToInt(m);
                partK = m - _index;
            }
            
            if (index != _index)
            {
                index = _index;

                var list = curve2D.list;
                var i = index;
                var prev = list.GetItemByRange(i - 1);
                var p1 = list[i];
                var p2 = list.GetItemByRange(i + 1);
                var next = list.GetItemByRange(i + 2);
                var startTangent = p1 + (p2 - prev) * curve2D.a;
                var endTangent = p2 - (next - p1) * curve2D.b;

                UpdatePos = t => Pos = BezierUtil.LerpUnclamped(p1, startTangent, endTangent, p2, t);
            }
            T = partK;
        }
    }
    Action<float> UpdatePos;
    float T
    {
        set
        {
            UpdatePos(value);
        }
    }

    Vector3 lastPos;
    Vector2 Pos
    {
        set
        {
            var p = worldMat.MultiplyPoint(value);
            if(lastPos!=p) transform.rotation=Quaternion.LookRotation(lastPos - p);
            transform.position = p;
            lastPos = p;
        }
        get
        {
            return localMat.MultiplyPoint(transform.position);
        }
    }
}
