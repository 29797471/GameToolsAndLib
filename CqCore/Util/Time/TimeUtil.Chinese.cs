using System;
using System.Collections.Generic;

public static partial class TimeUtil
{

    /// <summary>
    /// 公历转为农历的函数
    /// </summary>
    /// <remarks>作者：DeltaCat</remarks>
    /// <example>网址：http://www.zu14.cn</example>
    /// <param name="solarDateTime">公历日期</param>
    /// <returns>农历的日期</returns>
    public static string SolarToChineseLunisolarDate(this DateTime solarDateTime)
    {
        System.Globalization.ChineseLunisolarCalendar cal = new System.Globalization.ChineseLunisolarCalendar();

        int year = cal.GetYear(solarDateTime);
        int month = cal.GetMonth(solarDateTime);
        int day = cal.GetDayOfMonth(solarDateTime);
        int leapMonth = cal.GetLeapMonth(year);
        return string.Format("农历{0}{1}（{2}）年{3}{4}月{5}{6}"
                            , "甲乙丙丁戊己庚辛壬癸"[(year - 4) % 10]
                            , "子丑寅卯辰巳午未申酉戌亥"[(year - 4) % 12]
                            , "鼠牛虎兔龙蛇马羊猴鸡狗猪"[(year - 4) % 12]
                            , month == leapMonth ? "闰" : ""
                            , "无正二三四五六七八九十冬腊"[leapMonth > 0 && leapMonth <= month ? month - 1 : month]
                            , "初十廿三"[day / 10]
                            , "日一二三四五六七八九"[day % 10]
                            );
    }

    /// <summary>
    /// 传入出生日期,获取百岁以内生日新旧历与出生时相同的年龄列表
    /// </summary>
    public static List<int> GetBirthdays(DateTime lifeStartTime,int MaxAge=100)
    {
        var years = new List<int>();
        var oldDate = lifeStartTime.SolarToChineseLunisolarDate();
        var temp = oldDate.Substring(oldDate.Length - 4);
        for (int i = 0; i < MaxAge; i++)
        {
            DateTime a = lifeStartTime.AddYears(i);
            var str = a.SolarToChineseLunisolarDate();
            if (str.Substring(str.Length-4)== temp)
            {
                years.Add(i);
            }
        }
        return years;
    }

}