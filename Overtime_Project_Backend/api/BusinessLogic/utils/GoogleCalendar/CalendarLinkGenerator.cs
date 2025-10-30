using System;
using System.Web;

namespace api.Utils
{
    public static class CalendarLinkGenerator
    {
        public static string GenerateGoogleCalendarLink(string title, DateTime date, TimeSpan start, TimeSpan end, string justification)
        {
            var startUtc = date.Add(start).ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
            var endUtc = date.Add(end).ToUniversalTime().ToString("yyyyMMddTHHmmssZ");

            var details = HttpUtility.UrlEncode(justification);
            var encodedTitle = HttpUtility.UrlEncode(title);

            return $"https://calendar.google.com/calendar/render?action=TEMPLATE&text={encodedTitle}&dates={startUtc}/{endUtc}&details={details}";
        }
    }
}
