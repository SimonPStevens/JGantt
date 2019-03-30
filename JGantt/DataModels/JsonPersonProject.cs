using System;
using System.Globalization;
using Newtonsoft.Json;

namespace JGantt.DataModels
{
    public class JsonPersonProject
    {
        private const string stringFormat = "yyyyMMdd";

        public string Person { get; set; }
        public string Project { get; set; }
        public string Start { get; set; }

        [JsonIgnore]
        public DateTime StartDate
        {
            get
            {
                return DateTime.ParseExact(this.Start, stringFormat, CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
            //set
            //{
            //    this.Start = value.ToString(stringFormat);
            //}
        }

        public string End { get; set; }

        [JsonIgnore]
        public DateTime EndDate
        {
            get
            {
                if (DateTime.TryParseExact(this.End, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                else if (this.DaysDuration.HasValue)
                {
                    return this.StartDate.AddworkingDays(this.DaysDuration.Value);
                } else
                {
                    return this.StartDate;
                }
            }
            //set
            //{
            //    this.End = value.ToString(stringFormat);
            //}
        }

        public int? DaysDuration { get; set; }
    }
}
