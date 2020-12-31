using System.Collections.Generic;
using UnityCore;
using UnityEngine;

/// <summary>
/// psd导出的图集转图片方式
/// </summary>
public class PsdAtlas : MonoBehaviour
{
    public Texture texture;

    [ComBox("图片", ComBoxStyle.RadioBox), Items("Alts"), OnValueChanged("OnChangeSprite")]
    public IImg sprite;

    public TextAsset asset;

    public class IImg
    {
        public string name;
        public float x;
        public float y;
        public float width;
        public float height;
        public Vector4 border;
    }
    List<IImg> mAlts;
    protected List<IImg> Alts
    {
        get
        {
            if (mAlts == null)
            {
                if (asset != null) mAlts = Torsion.Deserialize<List<IImg>>(asset.text);
            }
            return mAlts;
        }
    }
}