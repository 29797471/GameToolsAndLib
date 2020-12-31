using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 对象序列化和反序列化后的回调接口
/// </summary>
public interface IJsonSerialize
{
    void OnDeserialize();
    void OnSerialize();
}
