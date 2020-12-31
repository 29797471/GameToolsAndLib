using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityCore;
using UnityEngine;

/// <summary>
/// 生成一个多边形
/// </summary>
public class PolygonBuilder : MonoBehaviour
{
    /// <summary>
    /// 多边形的顶点
    /// </summary>
    [PolygonEdit]
    [ListBox("多边形")]
    public List<Vector2> list=new Vector2[3] {Vector2.zero,Vector2.one*100,Vector2.right*100 }.ToList();

    /// <summary>
    /// 世界坐标系下的坐标
    /// </summary>
    public List<Vector3> WorldList
    {
        get
        {
            var worldMat = transform.localToWorldMatrix;
            var temp = new List<Vector3>();
            for (int i = 0; i < list.Count; i++)
            {
                var p = worldMat.MultiplyPoint(list[i]);
                temp.Add(p);
            }
            return temp;
        }
    }
    [Button("求面积"),Click("CalcArea")]
    public string _1;
    public void CalcArea()
    {
        Debug.Log("面积:" + Mathf.Abs(Vector2Util.CalcArea(list))*transform.lossyScale.x*transform.lossyScale.y);
    }
    /// <summary>
    /// 判定世界坐标系下的点和多边形位置关系<para/>
    /// 返回点是否在多边形内<para/>
    /// 参数返回和最近的边的距离,以及到最近的点
    /// </summary>
    public bool InRange(Vector3 worldPos, out float distance,out Vector3 intersection)
    {
        var p = (Vector2)transform.worldToLocalMatrix.MultiplyPoint(worldPos);
        int start, end;
        Vector2 _intersection;
        distance = p.MinDistance(list, out start, out end, out _intersection);
        intersection = transform.localToWorldMatrix.MultiplyPoint(_intersection);
        return p.InRange(list);
    }

    /// <summary>
    /// 判定世界坐标系下的点和多边形位置关系<para/>
    /// 返回0内1边上2外<para/>
    /// 参数返回和最近的边的距离,以及到最近的点
    /// </summary>
	public PolyPointRelations InRangeX(Vector3 worldPos, out float distance, out Vector3 intersection,int round=2)
    {
        var p = (Vector2)transform.worldToLocalMatrix.MultiplyPoint(worldPos);
        if(round!=0)
        {
            p = new Vector2((float)System.Math.Round(p.x, round), (float)System.Math.Round(p.y, round));
        }
        int start, end;
        Vector2 _intersection;
        distance = p.MinDistance(list, out start, out end, out _intersection);
        intersection = transform.localToWorldMatrix.MultiplyPoint(_intersection);
        return p.InRangeX(list);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var worldMat = transform.localToWorldMatrix;
        var lastP = worldMat.MultiplyPoint(list[0]);
        for (int i = 0; i < list.Count; i++)
        {
            var p = worldMat.MultiplyPoint(list.GetItemByRound(i+1));
            Gizmos.DrawLine(lastP, p);
            lastP = p;
        }
    }
}
