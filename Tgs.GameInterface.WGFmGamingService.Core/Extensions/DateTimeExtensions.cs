namespace Tgs.GameInterface.WGFmGamingService.Core.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo _TimeZone;

    static DateTimeExtensions()
    {
        _TimeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");
    }

    public static DateTimeOffset StringConvertDateTimeOffset(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return DateTime.UnixEpoch;
        }
        var date = DateTime.Parse(str);

        return new DateTimeOffset(date, _TimeZone.GetUtcOffset(date));
    }
}
