using System;
using System.Globalization;
using Newtonsoft.Json;

namespace JGantt.ViewModels
{
    public class Milestone
    {
        public Milestone(string title, DateTime date)
        {
            this.Title = title;
            this.Date = date;
        }

        public Project Project { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }
    }
}