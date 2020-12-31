using System;

namespace UnityEngine
{
    /// <summary>
    /// GUILayout扩展
    /// </summary>
    public static class GUILayoutUtil
    {
        public static void Horizontal(Action act, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            if (act != null) act();
            GUILayout.EndHorizontal();
        }
        public static void Horizontal(Action act,string text,GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(text, style, options);
            if (act != null) act();
            GUILayout.EndHorizontal();
        }
        public static void Vertical(Action act, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            if (act != null) act();
            GUILayout.EndVertical();
        }
        public static void Vertical(Action act, string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(text, style, options);
            if (act != null) act();
            GUILayout.EndVertical();
        }
        public static void Area(Action act, Rect screenRect)
        {
            GUILayout.BeginArea(screenRect);
            if (act != null) act();
            GUILayout.EndArea();
        }
    }
}
