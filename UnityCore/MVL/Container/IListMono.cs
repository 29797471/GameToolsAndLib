using System;
using UnityEngine;

namespace UnityCore
{
    /// <summary>
    /// 列表控件接口<para/>
    /// 实现这个接口的组件,可以通过linklist绑定,数据关联
    /// </summary>
    public interface IListMono
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        Action<GameObject, int> UpdateData { set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        int DataCount { get; set; }

        /// <summary>
        /// 插入数据
        /// </summary>
        bool Insert(int dataIndex);

        /// <summary>
        /// 删除数据
        /// </summary>
        bool RemoveAt(int dataIndex);
    }
}
