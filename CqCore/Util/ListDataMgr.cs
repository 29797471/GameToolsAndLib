using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 主要用于优化对静态数据的查询,先生成查询数据,使用时0检索
/// 大数据列表检索器,用于不作变化的数据的快速查询
/// 对数据量大的表 形如List(Struct),建立Struct属性的查找索引表
/// 查询时0查询次数获取数据
/// 用法例:ListDataMgr.MakeQueryIndex(ConfigMgr_XX.instance);
/// var v = ListDataMgr.Find&lt;Config_XX.RECORD&gt;("id", 5);
/// var list = ListDataMgr.FindList&lt;Config_XX.RECORD&gt;("price", 10);
/// </summary>
public class ListFind<T>
{
	List<T> list;
	//通过数据项名称和数据 映射 满足的数据列表
	Dictionary<string, List<int>> dic=new Dictionary<string, List<int>>();
	public ListFind(List<T> list)
	{
		this.list=list;
        //dic = new Dictionary<string, List<T>>();
		var fields = typeof(T).GetFields();
		foreach (var fieldInfo in fields)
        {
            foreach(var it in list)
            {
                var searchStr = MakeSeachBy(fieldInfo.Name, fieldInfo.GetValue(it));
				if(!dic.ContainsKey(searchStr))
				{
					//dic[searchStr]=new List<T>();
				}
				//dic[searchStr].Add(it);
            }
        }
	}
	
	public List<int> FindDataArray(string key, object data)
	{
		var searchStr= MakeSeachBy(key,data);
		if(dic.ContainsKey(searchStr))
		{
			return dic[searchStr];
		}
		return null;
	}
	string MakeSeachBy(string key,object data)
	{
		return key+Torsion.Serialize(data);
	}
}
