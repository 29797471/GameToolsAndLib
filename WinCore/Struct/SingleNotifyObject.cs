using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SingleNotifyObject<T> : NotifyObject
{
    protected static T ms_instance = default(T);

    protected SingleNotifyObject()
    {
    }

    public static T instance
    {
        get
        {
            if (ms_instance == null)
            {
                ms_instance = Activator.CreateInstance<T>();
            }
                
            return ms_instance;
        }
    }

    public static void ClearSaveData()
    {
        ms_instance = default(T);
    }
}