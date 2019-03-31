using Newtonsoft.Json;
using System;
using System.Globalization;

namespace JGantt.DataModels
{
    public class JsonHoliday
    {
        [JsonProperty("Date")]
        public string DateString { get; set; }

        [JsonIgnore]
        public DateTime Date
        {
            get
            {
                return DateTime.ParseExact(this.DateString, "yyyyMMdd", CultureInfo.CurrentCulture);
            }
        }
        public string Colour { get; set; }
    }
}
