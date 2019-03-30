using System;
using System.Globalization;
using Newtonsoft.Json;

namespace JGantt.DataModels
{
    public class JsonMilestone
    {
        private const string stringFormat = "yyyyMMdd";

        public string Project { get; set; }

        [JsonProperty("Date")]
        public string DateString { get; set; }
        public string Title { get; set; }


        [JsonIgnore]
        public DateTime Date
        {
            get
            {
                return DateTime.ParseExact(this.DateString, stringFormat, CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
        }
    }
}
