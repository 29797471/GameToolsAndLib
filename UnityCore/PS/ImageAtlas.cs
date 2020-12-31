using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// psd导出的图集转图片方式
/// </summary>
[RequireComponent(typeof(Image))]
public class ImageAtlas : PsdAtlas
{
    Image mTarget;
    Image Target
    {
        get
        {
            if (mTarget == null) mTarget = GetComponent<Image>();
            return mTarget;
        }
    }
    
    public void OnChangeSprite()
    {
        if (sprite != null)
        {
            var width = texture.width;
            var height = texture.height;
            Target.sprite = Sprite.Create((Texture2D)texture,
                new Rect(sprite.x , height - sprite.y  - sprite.height , sprite.width , sprite.height ), Vector2.zero,
            100, 0, SpriteMeshType.Tight,
            sprite.border);
            if(sprite.border==Vector4.zero)
            {
                Target.type = Image.Type.Simple;
            }
            else
            {
                Target.type = Image.Type.Sliced;
            }
            Target.SetNativeSize();
        }
    }
}