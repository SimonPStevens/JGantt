using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JGantt.ViewModels
{
    public class PlanModel
    {
        public PlanModel(List<PersonProject> plan, List<Holiday> holidays)
        {
            this.Plan = plan ?? new List<PersonProject>();

            this.Projects = plan.Select(p => p.Project).Distinct().ToList();
            this.Categories = plan.Where(p=> p.Person.Category != null).Select(p => p.Person.Category).Distinct().ToList();
            this.Holidays = holidays;

            this.PersonPlans = BuildItemPlan(item=>item.Person);
            this.ProjectPlans = BuildItemPlan(item => item.Project);
        }

        private List<ItemPlan> BuildItemPlan(Func<PersonProject, IPlannableItem> getPlannableItem)
        {
            var plan = new List<ItemPlan>();

            foreach (var personProject in this.Plan)
            {
                var itemPlan = plan.FirstOrDefault(p => p.PlannableItem == getPlannableItem(personProject));
                if (itemPlan == null)
                {
                    itemPlan = new ItemPlan(getPlannableItem(personProject));
                    plan.Add(itemPlan);
                }

                var channel = itemPlan.Channels.FirstOrDefault(c => !c.PersonProjects.Any(p => p.Start < personProject.End && p.End > personProject.Start));
                if (channel == null)
                {
                    int newChannelNumber = itemPlan.Channels.Any() ? itemPlan.Channels.Max(c => c.ChannelNumber) + 1 : 0;
                    channel = new Channel(newChannelNumber);
                    itemPlan.Channels.Add(channel);
                }
                channel.PersonProjects.Add(personProject);
            }

            return plan;
        }

        public bool IsHoliday(DateTime date)
        {
            return this.Holidays?.Any(h => h.Date == date.Date) ?? false;
        }

        public string GetDayColour(DateTime date)
        {
            var holiday = this.Holidays?.FirstOrDefault(h => h.Date == date.Date);
            if (holiday != null)
            {
                return !string.IsNullOrWhiteSpace(holiday.Colour) ? holiday.Colour : "darkgrey";
            }
            else if (date.IsWeekend())
            {
                return "darkgrey";
            }
            else
            {
                return "lightgrey";
            }
        }

        public List<PersonProject> Plan { get; set; }
        public List<ItemPlan> PersonPlans { get; set; }
        public List<ItemPlan> ProjectPlans { get; set; }

        public List<Category> Categories { get; set; }
        public List<Project> Projects { get; set; }
        public List<Holiday> Holidays { get; set; }

        public string Json { get; set; }
        public string JsonError { get; set; }
    }
}
