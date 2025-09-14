using System.Globalization;

namespace FitnessAppAPI.Utilities
{
    /// <summary>
    /// Helper class for date and time operations
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Gets the start of the week for a given date
        /// </summary>
        public static DateTime GetStartOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets the end of the week for a given date
        /// </summary>
        public static DateTime GetEndOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return GetStartOfWeek(date, startOfWeek).AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        /// <summary>
        /// Gets the start of the month for a given date
        /// </summary>
        public static DateTime GetStartOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the end of the month for a given date
        /// </summary>
        public static DateTime GetEndOfMonth(DateTime date)
        {
            return GetStartOfMonth(date).AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        /// <summary>
        /// Gets the start of the year for a given date
        /// </summary>
        public static DateTime GetStartOfYear(DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// Gets the end of the year for a given date
        /// </summary>
        public static DateTime GetEndOfYear(DateTime date)
        {
            return new DateTime(date.Year, 12, 31, 23, 59, 59);
        }

        /// <summary>
        /// Calculates age from birth date
        /// </summary>
        public static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            
            if (birthDate.Date > today.AddYears(-age))
                age--;
                
            return age;
        }

        /// <summary>
        /// Gets the week number of the year
        /// </summary>
        public static int GetWeekOfYear(DateTime date)
        {
            var calendar = CultureInfo.CurrentCulture.Calendar;
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// Gets the quarter of the year (1-4)
        /// </summary>
        public static int GetQuarter(DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// Checks if a date is a weekend
        /// </summary>
        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks if a date is a weekday
        /// </summary>
        public static bool IsWeekday(DateTime date)
        {
            return !IsWeekend(date);
        }

        /// <summary>
        /// Gets the next occurrence of a specific day of week
        /// </summary>
        public static DateTime GetNextDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            int daysUntilTarget = ((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7;
            if (daysUntilTarget == 0)
                daysUntilTarget = 7; // If it's the same day, get next week's occurrence
                
            return date.AddDays(daysUntilTarget);
        }

        /// <summary>
        /// Gets the previous occurrence of a specific day of week
        /// </summary>
        public static DateTime GetPreviousDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
            int daysSinceTarget = ((int)date.DayOfWeek - (int)dayOfWeek + 7) % 7;
            if (daysSinceTarget == 0)
                daysSinceTarget = 7; // If it's the same day, get previous week's occurrence
                
            return date.AddDays(-daysSinceTarget);
        }

        /// <summary>
        /// Formats a date for display in various formats
        /// </summary>
        public static string FormatForDisplay(DateTime date, DateDisplayFormat format = DateDisplayFormat.Short)
        {
            return format switch
            {
                DateDisplayFormat.Short => date.ToString("MM/dd/yyyy"),
                DateDisplayFormat.Medium => date.ToString("MMM dd, yyyy"),
                DateDisplayFormat.Long => date.ToString("MMMM dd, yyyy"),
                DateDisplayFormat.Full => date.ToString("dddd, MMMM dd, yyyy"),
                DateDisplayFormat.Relative => GetRelativeTimeString(date),
                DateDisplayFormat.ISO => date.ToString("yyyy-MM-dd"),
                _ => date.ToString()
            };
        }

        /// <summary>
        /// Gets a relative time string (e.g., "2 days ago", "in 3 hours")
        /// </summary>
        public static string GetRelativeTimeString(DateTime date)
        {
            var now = DateTime.Now;
            var timeSpan = now - date;

            if (timeSpan.TotalDays > 365)
            {
                int years = (int)(timeSpan.TotalDays / 365);
                return $"{years} year{(years == 1 ? "" : "s")} ago";
            }
            
            if (timeSpan.TotalDays > 30)
            {
                int months = (int)(timeSpan.TotalDays / 30);
                return $"{months} month{(months == 1 ? "" : "s")} ago";
            }
            
            if (timeSpan.TotalDays >= 1)
            {
                int days = (int)timeSpan.TotalDays;
                return $"{days} day{(days == 1 ? "" : "s")} ago";
            }
            
            if (timeSpan.TotalHours >= 1)
            {
                int hours = (int)timeSpan.TotalHours;
                return $"{hours} hour{(hours == 1 ? "" : "s")} ago";
            }
            
            if (timeSpan.TotalMinutes >= 1)
            {
                int minutes = (int)timeSpan.TotalMinutes;
                return $"{minutes} minute{(minutes == 1 ? "" : "s")} ago";
            }
            
            return "Just now";
        }

        /// <summary>
        /// Checks if two dates are on the same day
        /// </summary>
        public static bool IsSameDay(DateTime date1, DateTime date2)
        {
            return date1.Date == date2.Date;
        }

        /// <summary>
        /// Gets business days between two dates
        /// </summary>
        public static int GetBusinessDaysBetween(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                return 0;

            int businessDays = 0;
            var currentDate = startDate.Date;

            while (currentDate <= endDate.Date)
            {
                if (IsWeekday(currentDate))
                    businessDays++;
                    
                currentDate = currentDate.AddDays(1);
            }

            return businessDays;
        }

        /// <summary>
        /// Converts UTC time to local time zone
        /// </summary>
        public static DateTime ConvertUtcToLocal(DateTime utcDateTime, string timeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        }

        /// <summary>
        /// Converts local time to UTC
        /// </summary>
        public static DateTime ConvertLocalToUtc(DateTime localDateTime, string timeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
        }
    }

    /// <summary>
    /// Date display format options
    /// </summary>
    public enum DateDisplayFormat
    {
        Short,      // 01/15/2024
        Medium,     // Jan 15, 2024
        Long,       // January 15, 2024
        Full,       // Monday, January 15, 2024
        Relative,   // 2 days ago
        ISO         // 2024-01-15
    }

    /// <summary>
    /// Time period helper for common date ranges
    /// </summary>
    public static class TimePeriods
    {
        public static (DateTime Start, DateTime End) Today => 
            (DateTime.Today, DateTime.Today.AddDays(1).AddTicks(-1));

        public static (DateTime Start, DateTime End) Yesterday => 
            (DateTime.Today.AddDays(-1), DateTime.Today.AddTicks(-1));

        public static (DateTime Start, DateTime End) ThisWeek => 
            (DateTimeHelper.GetStartOfWeek(DateTime.Today), DateTimeHelper.GetEndOfWeek(DateTime.Today));

        public static (DateTime Start, DateTime End) LastWeek => 
            (DateTimeHelper.GetStartOfWeek(DateTime.Today.AddDays(-7)), DateTimeHelper.GetEndOfWeek(DateTime.Today.AddDays(-7)));

        public static (DateTime Start, DateTime End) ThisMonth => 
            (DateTimeHelper.GetStartOfMonth(DateTime.Today), DateTimeHelper.GetEndOfMonth(DateTime.Today));

        public static (DateTime Start, DateTime End) LastMonth => 
            (DateTimeHelper.GetStartOfMonth(DateTime.Today.AddMonths(-1)), DateTimeHelper.GetEndOfMonth(DateTime.Today.AddMonths(-1)));

        public static (DateTime Start, DateTime End) ThisYear => 
            (DateTimeHelper.GetStartOfYear(DateTime.Today), DateTimeHelper.GetEndOfYear(DateTime.Today));

        public static (DateTime Start, DateTime End) LastYear => 
            (DateTimeHelper.GetStartOfYear(DateTime.Today.AddYears(-1)), DateTimeHelper.GetEndOfYear(DateTime.Today.AddYears(-1)));

        public static (DateTime Start, DateTime End) Last30Days => 
            (DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1).AddTicks(-1));

        public static (DateTime Start, DateTime End) Last90Days => 
            (DateTime.Today.AddDays(-90), DateTime.Today.AddDays(1).AddTicks(-1));
    }
}
