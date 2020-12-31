/****************************************************
	文件：PETool.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2018/10/30 11:21   	
	功能：工具类
*****************************************************/

using System;

namespace PENet
{
    public class PETool 
    {
        public static Func<object, byte[]> SerializeBinary = Torsion.SerializeBinary;
        public static Func<byte[], object> Deserialize = Torsion.Deserialize;
        
    }

}