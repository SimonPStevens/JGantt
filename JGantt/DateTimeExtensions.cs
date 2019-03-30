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
    }
}
