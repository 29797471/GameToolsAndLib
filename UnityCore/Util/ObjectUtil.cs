using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
public class ObjectUtil
{
    public static void CloneTronsfrom(Transform clone , Transform orgin)
    {
        clone.position = orgin.position;
        clone.localScale = orgin.localScale;
        clone.rotation = orgin.rotation;
        clone.forward = orgin.forward;
    }

    public static void SetLayer(GameObject obj, string newLayer)
    {
        //obj.layer = (int)(Mathf.Log((int)newLayer) / Mathf.Log(2));
        obj.layer = LayerMask.NameToLayer(newLayer);
    }
    public static void SetLayerRecursively(GameObject obj, string newLayer)
    {
        SetLayer(obj,newLayer);

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    public static Quaternion LookRotation(Vector3 current, Vector3 target)
    {
        Vector3 facingDirection = target - current;
        facingDirection.y = 0;
        return Quaternion.LookRotation(facingDirection);
    }

    
    public static Transform FindChild(Transform parent,string childName)
    {
        foreach(Transform child in parent)
        {
            Transform result = child.Find(childName);
            if (result) return result;
            if(child.childCount>0)
            {
                result = FindChild(child, childName);
                if (result) return result;
            }
        }
        return null;
    }
    public static bool DrawLine(Texture2D tex, Vector2 start, Vector2 end, Color col)
    {
        int x0 = (int)start.x, y0 = (int)start.y, x1 = (int)end.x, y1 = (int)end.y;

		float dy, dx, x, y, m;
		dx = x1 - x0;
		dy = y1 - y0;
		m = dy / dx;
		if (x0 < x1)
		{
			if (m <= 1 && m >= -1)
			{
				y = y0;
				for (x = x0; x <= x1; x++)
				{
					tex.SetPixel((int)x, (int)(y + 0.5f), col);
					y += m;
				}
			}
		}
		if (x0 > x1)
		{
			if (m <= 1 && m >= -1)
			{
				y = y0;
				for (x = x0; x >= x1; x--)
				{
					tex.SetPixel((int)x, (int)(y + 0.5f), col);
					y -= m;
				}
			}
		}
		if (y0 < y1)
		{
			if (m >= 1 || m <= -1)
			{
				m = 1 / m;
				x = x0;
				for (y = y0; y <= y1; y++)
				{
                    tex.SetPixel((int)(x + 0.5f), (int)y, col);
					x += m;
				}
			}
		}
		if (y0 > y1)
		{
			if (m <= -1 || m >= 1)
			{
				m = 1 / m;
				x = x0;
				for (y = y0; y >= y1; y--)
				{
					tex.SetPixel((int)(x + 0.5f),(int) y, col);
					x -= m;
				}
			}
		}
        
        tex.Apply();
        return true;
    }
    //row 行数
    //col 列数
    public static bool DrawGrid(Texture2D tex,int row,int col,Color color)
    {
        float deltaY=tex.height;
        deltaY/=row;
        float deltaX=tex.width;
        deltaX/=col;
        for (float x = deltaX; x < tex.width; x += deltaX)
        {
            DrawLine(tex, new Vector2(x, 0), new Vector2(x, tex.height), color);
        }
        for (float y = deltaY; y < tex.height; y += deltaY)
        {
            DrawLine(tex, new Vector2(0, y), new Vector2(tex.width,y ), color);
        }
        return true;
    }

    //row 行数
    //col 列数
    public static Texture2D DrawGridTexture(Texture2D tex, int row, int col, Color color)
    {
        Texture2D tempTex = CopyTexture(tex);
        float deltaY = tempTex.height; 
        deltaY /= row;
        float deltaX = tempTex.width;
        deltaX /= col;
        for (float x = deltaX; x < tempTex.width; x += deltaX)
        {
            DrawLine(tempTex, new Vector2(x, 0), new Vector2(x, tempTex.height), color);
        }
        for (float y = deltaY; y < tempTex.height; y += deltaY)
        {
            DrawLine(tempTex, new Vector2(0, y), new Vector2(tempTex.width, y), color);
        }
        return tempTex;
    }

    public static Texture2D CopyTexture(Texture2D tex)
    {
        Texture2D tempTex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);

        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                Color cPixel = tex.GetPixel(i, j);
                tempTex.SetPixel(i, j, cPixel);
            }
        }
        tempTex.Apply();
        return tempTex;
    }

}
