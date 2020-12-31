using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// psd导出的图集转图片方式
/// </summary>
[RequireComponent(typeof(RawImage))]
public class RawImageAtlas : PsdAtlas
{
    RawImage mTarget;
    RawImage Target
    {
        get
        {
            if (mTarget == null) mTarget = GetComponent<RawImage>();
            return mTarget;
        }
    }
    
    public void OnChangeSprite()
    {
        Target.texture = texture;
        if (sprite != null)
        {
            var width = texture.width;
            var height = texture.height;
            Target.uvRect = new Rect
                (sprite.x / width,
                1f - sprite.y / height - sprite.height / height,
                sprite.width / width,
                sprite.height / height);
            Target.SetNativeSize();
        }
    }
}