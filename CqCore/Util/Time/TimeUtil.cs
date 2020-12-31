using System;

public static partial class TimeUtil
{
    /// <summary>
    /// 时间转为版本格式,形如: 2019.05.23.1201
    /// </summary>
    public static string NowVersion
    {
        get
        {
            var now = DateTime.Now;
            return now.ToString("yyyy.MM.dd.HHmm");
        }
    }

    /// <summary>
    /// 时间转为版本格式,形如: 2019.0523.1201
    /// </summary>
    public static string NowVersion3
    {
        get
        {
            var now = DateTime.Now;
            return now.ToString("yyyy.MMdd.HHmmss");
        }
    }

    public static bool IsDate(string value)
    {
        DateTime d;
        return DateTime.TryParse(value, out d);
    }

    /// <summary>
    /// 1970-1-1
    /// </summary>
    public static DateTime dt_1970_1_1 = new DateTime(1970, 1, 1);

    /// <summary>
    /// 当前时区的1970_1_1
    /// </summary>
    public static DateTime CurrentTimeZone_dt_1970_1_1 = TimeZone.CurrentTimeZone.ToLocalTime(dt_1970_1_1);
    /// <summary>
    /// 1秒
    /// </summary>
    public static TimeSpan second= TimeSpan.FromSeconds(1);

    /// <summary>
    /// 当前时区
    /// </summary>
    static TimeZone CurrentTimeZone = TimeZone.CurrentTimeZone;

    /// <summary>
    /// 时间戳转本地时间<para/>
    /// 由1970至今的毫秒数返回本地带时区的DateTime
    /// </summary>
    public static DateTime ToLocalTime(long Unix_timestamp)
    {
        return CurrentTimeZone_dt_1970_1_1.AddMilliseconds(Unix_timestamp);
        //return TimeZone.CurrentTimeZone.ToLocalTime(TimeUtil.dt_1970_1_1 + TimeSpan.FromMilliseconds(totalMilliseconds));
    }


    /// <summary>
    /// 时间戳(毫秒)<para/>
    /// 由本地时区时间DateTime返回格林时间1970,1,1至今的毫秒数
    /// </summary>
    public static long ToUnix_timestamp_long(this DateTime dateTime)
    {
        return (CurrentTimeZone.ToUniversalTime(dateTime) - dt_1970_1_1).Ticks/10000;
    }

    /// <summary>
    /// 时间戳(秒)
    /// 由本地时区时间DateTime返回格林时间1970,1,1至今的秒数
    /// </summary>
    public static int ToUnix_timestamp(this DateTime dateTime)
    {
        return (int)((CurrentTimeZone.ToUniversalTime(dateTime) - dt_1970_1_1).Ticks/10000000);
    }

    /// <summary>
    /// 时差,对中国来说是28800,000毫秒
    /// </summary>
    public static double ZoneTimeOffect
    {
        get
        {
            return CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMilliseconds;
        }
    }

    /// <summary>
    /// 当前时间戳(秒)
    /// </summary>
    public static int Unix_timestamp
    {
        get
        {
            return DateTime.Now.ToUnix_timestamp();
        }
    }

    /// <summary>
    /// 当前时间戳(毫秒)
    /// </summary>
    public static long Unix_timestamp_long
    {
        get
        {
            return DateTime.Now.ToUnix_timestamp_long();
        }
    }

    /// <summary>
    /// 传入两个时刻相减的秒数 返回时间长度(string) 形如00:02:51
    /// </summary>
    public static string TimeSpanFormat(int seconds)
    {
        TimeSpan t = new TimeSpan(0, 0, seconds);
        
        return t.ToString();
    }

    /// <summary>
    /// 传入两个时刻相减的秒数 返回时间长度(string) 形如00:02:51 
    /// format="HH:mm:ss"
    /// style 可选D2 null
    /// </summary>
    public static string TimeSpanFormat(int seconds,string format,string style="D2")
    {
        TimeSpan t = new TimeSpan(0, 0, seconds);
        
        format = format.Replace("HH", t.Hours.ToString(style));
        format = format.Replace("mm", t.Minutes.ToString(style));
        format = format.Replace("ss", t.Seconds.ToString(style));
        format = format.Replace("dd", t.Days.ToString());
        return format;
    }

    /// <summary>
    /// 本地时刻
    /// yyyy-MM-dd HH:mm:ss
    /// </summary>
    public static string GetTimeStringByDate(DateTime date, string format = "yyyy-MM-dd HH:mm:ss")
    {
        return date.ToString(format);
    }

    /// <summary>
    /// 时刻
    /// yyyy-MM-dd HH:mm:ss
    /// </summary>
    public static string GetTimeStringBySecond(int dayOfSeconds, string format = "yyyy-MM-dd HH:mm:ss")
    {
        return GetTimeStringByDate(ToLocalTime((long)dayOfSeconds * 1000), format);
    }

    /// <summary>
    /// 时刻
    /// </summary>
    public static string TimeFormat(int dayOfSeconds,string format="yyyy-MM-dd HH:mm:ss")
    {
        return ToLocalTime(dayOfSeconds * 1000L).ToString(format);
    }


}