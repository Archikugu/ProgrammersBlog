namespace ProgrammersBlog.Core.Utilities.Extensions;

public static class DateTimeExtensions
{
    public static string FullDateAndTimeStringWithUnderscore(this DateTime dateTime)
    {
        return $"{dateTime.ToShortDateString().Replace('.', '-')}_{dateTime.ToLongTimeString().Replace(':', '-')}";
    }
}
