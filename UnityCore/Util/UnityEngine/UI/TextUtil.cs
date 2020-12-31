namespace UnityEngine.UI
{
    public static class TextUtil
    {
        static TextGenerator tg = new TextGenerator();
        /// <summary>
        /// 获得内容真实宽度
        /// </summary>
        public static float GetRealWidth(this Text text)
        {
            var settings = text.GetGenerationSettings(Vector2.zero);
            settings.generateOutOfBounds = true;
            return tg.GetPreferredWidth(text.text, settings);
        }
    }
}
