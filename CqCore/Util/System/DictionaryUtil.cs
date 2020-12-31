namespace System.Collections.Generic
{
    public static class DictionaryUtil
    {
        public static bool TryGetKey<T1, T2>(this Dictionary<T1, T2> dic, T2 value,out T1 key)
        {
            foreach(var it in dic)
            {
                if(it.Value.Equals(value))
                {
                    key = it.Key;
                    return true;
                }
            }
            key = default(T1);
            return false;
        }
    }
}