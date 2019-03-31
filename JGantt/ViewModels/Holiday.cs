using Newtonsoft.Json;
using System;
using System.Globalization;

namespace JGantt.ViewModels
{
    public class Holiday
    {
        public Holiday(DateTime date, string colour)
        {
            Date = date;
            Colour = colour;
        }

        public DateTime Date { get; set; }
        public string Colour { get; set; }
    }
}
