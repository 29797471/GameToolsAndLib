//using CqCore;
//using System;
//using System.Collections.Generic;
//using UnityCore;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;

///// <summary>
///// 挂在ScrollRect同物体下,循环重复利用<para/>
///// 取content下第一个元素作为克隆体<para/>
///// 通过UpdateData更新单元格<para/>
///// 通过Insert/Add 添加单元格<para/>
///// 通过RemoveAt 移除单元格
///// </summary>
//[RequireComponent(typeof(ScrollRect))]
//public class RoundScroll : MonoBehaviourExtended
//{
//    ScrollRect sr;

//    CqTweenLerp_Vector2 sizeDelta_tweenHandle;

//    CqTweenLerp_Vector2 anchoredPosition_tweenHandle;

//    RectTransform cloneItem;
//    RectTransform rt;

//    /// <summary>
//    /// 一行或者一列的控件数量
//    /// </summary>
//    int rowOrColumn;

//    /// <summary>
//    /// 创建的行数或者列数
//    /// </summary>
//    int createLineCount;
//    List<RectTransform> clones;

//    /// <summary>
//    /// 由控件获取这个控件关联的缓动句柄
//    /// </summary>
//    Causality<RectTransform,CqTweenLerp_Vector2> GetHandle;
//    List<int> lineIndexs;

//    /// <summary>
//    /// 滑动时回调数据索引,更新控件
//    /// </summary>
//    public Action<GameObject, int> UpdateData
//    {
//        set
//        {
//            mUpdateData = value;
//            Init();
//        }
//    }
//    Action<GameObject, int> mUpdateData;

//    [TextBox("间距")]
//    public float spacing=10;


//    /*
//    [TextBox("多生成的行/列数")]
//    public int createMoreLineCount = 2;
//    */
//    /// <summary>
//    /// 多生成的行/列数
//    /// </summary>
//    public const int createMoreLineCount = 2;


//    /// <summary>
//    /// 包含间隔的单元格宽
//    /// </summary>
//    float cellWidth;
//    /// <summary>
//    /// 包含间隔的单元格高
//    /// </summary>
//    float cellHeight;

//    int mDataCount;

//    /// <summary>
//    /// 播放缓动
//    /// </summary>
//    bool playTween;

//    /// <summary>
//    /// 缓动时间
//    /// </summary>
//    [TextBox("滑动时间"),ToolTip("添加/删除/定位时的滑动时间")]
//    public float moveTime = 0.5f;

//    /// <summary>
//    /// 最后一个数据对应的克隆控件索引
//    /// </summary>
//    int lastDataIndex;

    

//    public int DataCount
//    {
//        get
//        {
//            return mDataCount;
//        }
//        set
//        {
//            Init();
//            sizeDelta_tweenHandle.Cancel();

//            ClearIndex();
//            //if (mDataCount != value)
//            {
//                mDataCount = value;
//                var sizeDelta = sr.content.sizeDelta;
//                sizeDelta_tweenHandle.start = sizeDelta;
//                if (sr.horizontal)
//                {
//                    sizeDelta.x = Mathf.CeilToInt(mDataCount * 1f / rowOrColumn) * cellWidth - spacing;
//                }
//                else if (sr.vertical)
//                {
//                    sizeDelta.y = Mathf.CeilToInt(mDataCount * 1f / rowOrColumn) * cellHeight - spacing;
//                }
//                if (playTween)
//                {
//                    sizeDelta_tweenHandle.end = sizeDelta;
//                    sizeDelta_tweenHandle.Play(moveTime,DestroyHandle);
//                }
//                else
//                {
//                    sr.content.sizeDelta = sizeDelta;
//                }
//                sr.onValueChanged.Invoke(Vector2.zero);
//            }
//        }
//    }

//    /// <summary>
//    /// 滑动列表位置
//    /// </summary>
//    public Vector2 Pos
//    {
//        set
//        {
//            sr.content.anchoredPosition = value;
//        }
//        get
//        {
//            return sr.content.anchoredPosition;
//        }
//    }

//    public enum TargetScrollPos
//    {
//        [EnumLabel("左/上")]
//        Start = 0,
//        [EnumLabel("中间")]
//        Center = 1,
//        [EnumLabel("右/下")]
//        End = 2,
//    }

//    /// <summary>
//    /// 计算在totalWidth中生成多少个itemWidth可以填满<para/>
//    /// </summary>
//    int CalcCount(float totalWidth, float itemWidth)
//    {
//        if (totalWidth < itemWidth) totalWidth = itemWidth;
//        return Mathf.FloorToInt((totalWidth - itemWidth) / (itemWidth + spacing)) + 1;
//    }

//    void Awake()
//    {
//        Init();
//    }
//    bool hasInit;
//    /// <summary>
//    /// 处理:
//    /// 1.拿到克隆体
//    /// 2.获得单元格宽高
//    /// 3.克隆填充面板
//    /// 4.监听面板滑动
//    /// </summary>
//    private void Init()
//    {
//        if (hasInit) return;
//        hasInit = true;
//        Func<float, float> tweenCurve = UnityEngine.EaseFun.GetEase(EaseFunEnum.Quadratic, EaseStyleEnum.EaseOut);
//        sr = GetComponent<ScrollRect>();
//        sizeDelta_tweenHandle = new CqTweenLerp_Vector2()
//        {
//            memberProxy=MemberProxy.GetMemberProxy(sr.content, "sizeDelta"),
//            Evaluate = tweenCurve,
//        };
//        anchoredPosition_tweenHandle = new CqTweenLerp_Vector2()
//        {
//            memberProxy = MemberProxy.GetMemberProxy(sr.content, "anchoredPosition"),
//            Evaluate = tweenCurve,
//        };
//        rt = sr.GetComponent<RectTransform>();
//        cloneItem = sr.content.GetChild(0).GetComponent<RectTransform>();
//        cloneItem.gameObject.SetActive(false);
//        if (sr.horizontal)
//        {
//            rowOrColumn = CalcCount(sr.content.rect.height, cloneItem.rect.height);
//            sr.onValueChanged.AddListenerX(v =>
//            {
//                OnScroll(-sr.content.anchoredPosition.x / cellWidth);
//            },DestroyHandle);
//        }
//        else if (sr.vertical)
//        {
//            rowOrColumn = CalcCount(sr.content.rect.width, cloneItem.rect.width);

//            sr.onValueChanged.AddListenerX(v =>
//            {
//                OnScroll(sr.content.anchoredPosition.y / (spacing + cloneItem.rect.height));
//            }, DestroyHandle);
//        }
//        cellWidth = spacing + cloneItem.rect.width;
//        cellHeight = spacing + cloneItem.rect.height;

//        CloneItem();

//        GetHandle = new Causality<RectTransform, CqTweenLerp_Vector2>(tran =>
//        {
//            var handle = new CqTweenLerp_Vector2()
//            {
//                memberProxy = MemberProxy.GetMemberProxy(tran, "anchoredPosition"),
//                Evaluate = tweenCurve,
//            };
//            return handle;
//        });
//    }

//    /// <summary>
//    /// 在删除数据之后调用该方法作界面表现
//    /// 1.错位更新单元格
//    /// 2.滑动定位新位置
//    /// </summary>
//    public bool RemoveAt(int dataIndex)
//    {
//        if (dataIndex >= DataCount || dataIndex < 0) return false;
//        var removeCloneIndex = MathUtil.MoveToRange(dataIndex, 0, clones.Count);
//        var cloneIndexBylastData = MathUtil.MoveToRange(lastDataIndex, 0, clones.Count);

//        var removeItem = GetCloneTran(removeCloneIndex);
//        var lastItem = GetCloneTran(cloneIndexBylastData);
//        removeItem.anchoredPosition = lastItem.anchoredPosition;
//        removeItem.SetSiblingIndex(lastItem.GetSiblingIndex());
//        clones.RemoveAt(removeCloneIndex);
//        clones.Insert(cloneIndexBylastData, removeItem);
//        if (removeCloneIndex > cloneIndexBylastData)
//        {
//            var item = clones[0];
//            clones.RemoveAt(0);
//            clones.Add(item);
//        }

//        playTween = true;
//        DataCount = DataCount - 1;
//        playTween = false;
//        return true;
//    }
//    public bool Add()
//    {
//        return Insert(DataCount);
//    }
//    /// <summary>
//    /// 在添加数据之后调用该方法作界面表现
//    /// 1.错位更新单元格
//    /// 2.滑动定位新位置
//    /// </summary>
//    public bool Insert(int dataIndex)
//    {
//        if (dataIndex > DataCount || dataIndex < 0) return false;
//        var addCloneIndex = MathUtil.MoveToRange(dataIndex, 0, clones.Count);
//        var cloneIndexBylastData = MathUtil.MoveToRange(lastDataIndex, 0, clones.Count);

//        var addItem = GetCloneTran(addCloneIndex);
//        var lastItem = GetCloneTran(cloneIndexBylastData);

//        lastItem.anchoredPosition = addItem.anchoredPosition;
//        lastItem.SetSiblingIndex(addItem.GetSiblingIndex());
//        clones.RemoveAt(cloneIndexBylastData);
//        clones.Insert(addCloneIndex, lastItem);
//        if (addCloneIndex > cloneIndexBylastData)
//        {
//            var item = clones[clones.Count - 1];
//            clones.RemoveAt(clones.Count - 1);
//            clones.Insert(0, item);
//        }

//        playTween = true;
//        DataCount = DataCount + 1;
//        playTween = false;
//        return true;
//    }
//    /// <summary>
//    /// 滑动到数据索引关联的对象可见.
//    /// </summary>
//    /// <param name="dataIndex">数据索引</param>
//    /// <param name="offsetCount">偏移多少个单位数据项</param>
//    public void MoveToVisible(int dataIndex, float offsetCount = 0.5f)
//    {
//        if (sr.horizontal)
//        {
//            var minPos = GetScrollTarget(dataIndex, TargetScrollPos.Start, -offsetCount);
//            var maxPos = GetScrollTarget(dataIndex, TargetScrollPos.End, offsetCount);
//            var x = Pos.x;
//            if (x < minPos.x)
//            {
//                MoveContentTo(minPos);
//            }
//            else if (x > maxPos.x)
//            {
//                MoveContentTo(maxPos);
//            }
//        }
//        else if (sr.vertical)
//        {
//            var maxPos = GetScrollTarget(dataIndex, TargetScrollPos.Start, -offsetCount);
//            var minPos = GetScrollTarget(dataIndex, TargetScrollPos.End, offsetCount);
//            var y = Pos.y;
//            if (y < minPos.y)
//            {
//                MoveContentTo(minPos);
//            }
//            else if (y > maxPos.y)
//            {
//                MoveContentTo(maxPos);
//            }
//        }
//    }

//    /// <summary>
//    /// 滑动定位到数据索引
//    /// </summary>
//    public void MoveToIndex(int dataIndex, TargetScrollPos tsp = TargetScrollPos.Center)
//    {
//        MoveContentTo(GetScrollTarget(dataIndex, tsp, 0));
//    }
//    /// <summary>
//    /// 滑动定位到数据索引
//    /// </summary>
//    /// <param name="dataIndex">定位的数据索引</param>
//    /// <param name="tsp">定位样式</param>
//    /// <param name="deltaCount">定位偏移多少个单位数据宽度</param>
//    /// <returns></returns>
//    Vector2 GetScrollTarget(int dataIndex, TargetScrollPos tsp, float deltaCount)
//    {
//        var lineIndex = Mathf.FloorToInt(dataIndex * 1f / rowOrColumn);
//        Vector2 pos = Pos;

//        if (sr.horizontal)
//        {
//            pos.x = (lineIndex + deltaCount) * cellWidth - (rt.rect.width - cloneItem.rect.width) * ((int)tsp) * 0.5f;
//            MathUtil.BetweenRange(ref pos.x, 0, sr.content.rect.width - rt.rect.width);
//            pos.x = -pos.x;
//        }
//        else if (sr.vertical)
//        {
//            pos.y = (lineIndex + deltaCount) * cellHeight - (rt.rect.height - cloneItem.rect.height) * ((int)tsp) * 0.5f;
//            MathUtil.BetweenRange(ref pos.y, 0, sr.content.rect.height - rt.rect.height);
//        }
//        return pos;
//    }

//    void MoveContentTo(Vector2 pos)
//    {
//        anchoredPosition_tweenHandle.Cancel();
//        anchoredPosition_tweenHandle.start = anchoredPosition_tweenHandle.current;
//        anchoredPosition_tweenHandle.end = pos;
//        anchoredPosition_tweenHandle.Play(moveTime,DestroyHandle);
//    }
//    void ClearIndex()
//    {
//        for (int i = 0; i < lineIndexs.Count; i++)
//        {
//            lineIndexs[i] = int.MaxValue;
//        }
//    }

//    void OnScroll(float startIndex)
//    {
//        int startDataIndex = Mathf.FloorToInt(startIndex - createMoreLineCount / 2f);

//        for (int lineDataIndex = startDataIndex; lineDataIndex < startDataIndex + createLineCount; lineDataIndex++)
//        {
//            var lineIndex = MathUtil.MoveToRange(lineDataIndex, 0, createLineCount);
//            if (lineIndexs[lineIndex] != lineDataIndex)
//            {
//                lineIndexs[lineIndex] = lineDataIndex;
//                for (int i = 0; i < rowOrColumn; i++)
//                {
//                    UpdateItem(lineDataIndex * rowOrColumn + i);
//                }
//            }
//        }

//        lastDataIndex = (startDataIndex + createLineCount) * rowOrColumn - 1;
//    }

//    /// <summary>
//    /// 当数据量小时,不会重复克隆填满.需要时才做克隆
//    /// </summary>
//    RectTransform GetCloneTran(int index)
//    {
//        if(clones[index]==null)
//        {
//            var obj = cloneItem.gameObject.Clone();
//            clones[index] = obj.GetComponent<RectTransform>();
//        }
//        return clones[index];
//    }
//    void UpdateItem(int dataIndex)
//    {
//        int cloneIndex = MathUtil.MoveToRange(dataIndex, 0, clones.Count);
        
//        var item = clones[cloneIndex];
//        //基于数据动态克隆
//        if(item==null)
//        {
//            if(dataIndex >= 0 && dataIndex < mDataCount)
//            {
//                item = GetCloneTran(cloneIndex);
//            }
//            else
//            {
//                return;
//            }
//        }
//        var tweenHandle = GetHandle.Call(item);
//        tweenHandle.Cancel();
//        Vector2 targetPos = Vector2.zero;

//        if (sr.horizontal)
//        {
//            targetPos = new Vector2((dataIndex / rowOrColumn) * cellWidth, (dataIndex % rowOrColumn) * -cellHeight) + cloneItem.anchoredPosition;
//        }
//        else if (sr.vertical)
//        {
//            targetPos = new Vector2((dataIndex % rowOrColumn) * cellWidth, (dataIndex / rowOrColumn) * -cellHeight) + cloneItem.anchoredPosition;
//        }
//        if (playTween)
//        {
//            tweenHandle.start = tweenHandle.current;
//            tweenHandle.end = targetPos;
//            tweenHandle.Play(moveTime,DestroyHandle);
//        }
//        else
//        {
//            item.anchoredPosition = targetPos;
//        }
//        item.name = string.Format("{0}_{1}", cloneItem.name, dataIndex);
//        //item.name = string.Format("{0}_{1}_{2}", cloneItem.name, cloneIndex, dataIndex);
//        if (dataIndex >= 0 && dataIndex < mDataCount)
//        {
//            item.gameObject.SetActive(true);
//            if(mUpdateData != null) mUpdateData(item.gameObject, dataIndex);
//        }
//        else
//        {
//            item.gameObject.SetActive(false);
//        }
//    }

//    /// <summary>
//    /// 计算填满显示范围需要克隆的组件数量,并克隆
//    /// </summary>
//    void CloneItem()
//    {
//        clones = new List<RectTransform>();
//        lineIndexs = new List<int>();
        
//        createLineCount = 0;
//        if (sr.horizontal)
//        {
//            createLineCount = CalcCount(rt.rect.width, cloneItem.rect.width);
//        }
//        else if (sr.vertical)
//        {
//            createLineCount = CalcCount(rt.rect.height, cloneItem.rect.height);
//        }

//        createLineCount += 1 + createMoreLineCount;
//        for (int i = 0; i < createLineCount; i++)
//        {
//            lineIndexs.Add(int.MaxValue);
//            for (int j = 0; j < rowOrColumn; j++)
//            {
//                //var obj = cloneItem.gameObject.Clone();
//                //clones.Add(obj.GetComponent<RectTransform>());
//                clones.Add(null);
//            }
//        }
//    }
//}
