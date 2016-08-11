using System;

public static class Clock
{

    public static Func<DateTime> LocalTimeGetter { get; set; }
    public static Func<int> HoursDifferenceFromGlobal { get; set; }


    public static DateTime CurrentLocalTime
    {
        get
        {
            return LocalTimeGetter == null ? DateTime.Now : LocalTimeGetter.Invoke();
        }
    }

    public static DateTime CurrentGlobalTime
    {
        get
        {
            if (HoursDifferenceFromGlobal != null)
                return CurrentLocalTime - TimeSpan.FromHours(HoursDifferenceFromGlobal.Invoke());
            return CurrentLocalTime.ToUniversalTime();
        }
    }

    public static DateTime ToLocalTime(DateTime globalDateTime)
    {
        if (HoursDifferenceFromGlobal != null)
            return globalDateTime + TimeSpan.FromHours(HoursDifferenceFromGlobal.Invoke());
        return globalDateTime.ToLocalTime();
    }
}
