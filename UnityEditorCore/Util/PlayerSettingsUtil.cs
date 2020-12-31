using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
    public static class PlayerSettingsUtil
    {
        public static void SetDefaultIcon(Texture2D icon)
        {
            var allPrivateMethods = typeof(PlayerSettings).GetMethods(BindingFlags.NonPublic | BindingFlags.Static).ToList();
            MethodInfo GetAllIconsForPlatform = allPrivateMethods.Find(x => x.Name == "GetAllIconsForPlatform");
            MethodInfo GetIconWidthsOfAllKindsForPlatform = allPrivateMethods.Find(x => x.Name == "GetIconWidthsOfAllKindsForPlatform");
            MethodInfo SetIconsForPlatform = allPrivateMethods.Find(x => x.Name == "SetIconsForPlatform");

            var array = (Texture2D[])GetAllIconsForPlatform.Invoke(null, new object[] { string.Empty });
            var iconWidthsOfAllKindsForPlatform = (int[])GetIconWidthsOfAllKindsForPlatform.Invoke(null, new object[] { string.Empty });
            if (array.Length != iconWidthsOfAllKindsForPlatform.Length)
            {
                array = new Texture2D[iconWidthsOfAllKindsForPlatform.Length];
            }
            array[0] = icon;
            SetIconsForPlatform.Invoke(null, new object[] { string.Empty, array });
        }
        /*
         public static void SetIcons(Texture2D tex2D)
        {
            int[] iconSizes = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
            Texture2D[] texArray = new Texture2D[iconSizes.Length];
            for (int i = 0; i < iconSizes.Length; ++i)
            {
                int iconSize = iconSizes[i];
                texArray[i] = tex2D;
            }

            //此处为新API
            var platform = EditorUserBuildSettings.selectedBuildTargetGroup;
            //kind有3种，分别对应PlayerSettings的Legacy，Round，Adaptive
            var kind = UnityEditor.Android.AndroidPlatformIconKind.Round;
            var icons = PlayerSettings.GetPlatformIcons(platform, kind);

            for (int i = 0, length = icons.Length; i < length; ++i)
            {
                //将转换后获得的Texture2D数组，逐个赋值给icons
                icons[i].SetTexture(texArray[i]);
            }
            PlayerSettings.SetPlatformIcons(platform, kind, icons);
        }
         */

    }
}
