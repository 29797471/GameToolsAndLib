using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEngine
{
    public class ImgData
    {
        public string base64str;
        public int width;
        public int height;
    }
    public static class SpriteUtil
    {
        public static Sprite ToSprite(this ImgData spr)
        {
            Texture2D pic = new Texture2D(spr.width, spr.height);
            byte[] data = Convert.FromBase64String(spr.base64str);
            pic.LoadImage(data);
            return pic.ToSprite();
        }
        public static ImgData ToImgData(this Sprite spr)
        {
            var data = new ImgData();
            data.base64str = ToBase64String(spr.texture);
            data.width = spr.texture.width;
            data.height = spr.texture.height;
            return data;
        }
        public static Sprite ToSprite(this Texture2D tex)
        {
            if (tex == null) return null;
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        static string ToBase64String(Texture2D tex)
        {
            byte[] data = tex.EncodeToPNG();
            return Convert.ToBase64String(data);
        }

        static Texture2D ToTexture2D(string base64str, int width, int height)
        {
            Texture2D pic = new Texture2D(width, height);
            byte[] data = Convert.FromBase64String(base64str);
            pic.LoadImage(data);
            return pic;
        }
    }
}
