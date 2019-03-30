using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JGantt
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static DateTime AddworkingDays(this DateTime startDate, int days)
        {
            DateTime rollingDate = startDate;
            while (days > 0)
            {
                if (!rollingDate.IsWeekend())
                {
                    days--;
                }

                rollingDate = rollingDate.AddDays(1);
            }
            return rollingDate;

        }
    }
}
