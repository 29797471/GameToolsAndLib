using UnityEngine;

/// <summary>
/// Lod策略控制物体的各个精度的模型显示
/// 通过计算物体在屏幕成像的占比,推算这个物体这时理应不超过的最大面数,来控制切换到对应的lod层级
/// </summary>
public class CqLodGroup : MonoBehaviour
{
	public GameObject[] objs;

	int[] tris;

	/// <summary>
	/// 同屏面数上限
	/// </summary>
	public static int screenTrisMax = 5000;
	string format1 = "屏占比:{0}\n屏占比最大面数:{1}\n面数:{2}";

	float r;

	public int current;
	public int Current
	{
		set
		{
			if (current != value)
			{
				if (objs[current] != null) objs[current].SetActive(false);
				current = value;
				if (objs[current] != null) objs[current].SetActive(true);
			}
		}
		get
		{
			return current;
		}
	}

	void Start()
	{
		//用第一个模型的外围包围盒计算r
		var size = objs[0].GetComponent<MeshRenderer>().bounds.size;
		r = (size.x + size.y + size.z) / 3;
		tris = new int[objs.Length];
		for (int i = 0; i < objs.Length; i++)
		{
			if (objs[i] == null)
			{
				tris[i] = 0;
			}
			else tris[i] = objs[i].GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
		}
	}
	void Update()
	{
		var k = r;
		k /= Mathf.Tan(Camera.main.fieldOfView / 2 * Mathf.Deg2Rad);
		k /= Vector3.Distance(transform.position, Camera.main.transform.position);

		var screenPercent = k * k;

		var maxTris = Mathf.RoundToInt(screenPercent * screenTrisMax);

		for (int i = 0; i < tris.Length; i++)
		{
			if (tris[i] < maxTris)
			{
				Current = i;
				break;
			}
		}
		//Debug.Log(string.Format(format1, screenPercent, maxTris, tris[Current]));
		
	}

}
