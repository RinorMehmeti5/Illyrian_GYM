using System.Globalization;

namespace Illyrian.RestApi.Utils.General;

public static class FormattingHelpers
{
    public static string FormatDuration(int days)
    {
        if (days % 365 == 0)
        {
            int years = days / 365;
            return $"{years} {(years == 1 ? "year" : "years")}";
        }
        else if (days % 30 == 0)
        {
            int months = days / 30;
            return $"{months} {(months == 1 ? "month" : "months")}";
        }
        else
        {
            return $"{days} days";
        }
    }

    public static string FormatPrice(decimal price)
    {
        return price.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
    }

    public static string FormatDate(DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    public static string FormatTime(DateTime time)
    {
        return time.ToString("hh:mm tt");
    }

    public static bool TryParseTimeString(string timeString, out DateTime result)
    {
        result = DateTime.Today;

        if (string.IsNullOrEmpty(timeString))
            return false;

        var parts = timeString.Split(':');
        if (parts.Length != 2)
            return false;

        if (!int.TryParse(parts[0], out int hours) || !int.TryParse(parts[1], out int minutes))
            return false;

        if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
            return false;

        result = DateTime.Today.AddHours(hours).AddMinutes(minutes);
        return true;
    }
}
