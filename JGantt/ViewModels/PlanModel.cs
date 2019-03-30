using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JGantt.ViewModels
{
    public class PlanModel
    {
        public PlanModel(List<PersonProject> plan)
        {
            this.Plan = plan ?? new List<PersonProject>();
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

        public List<PersonProject> Plan { get; set; }
        public List<ItemPlan> PersonPlans { get; set; }
        public List<ItemPlan> ProjectPlans { get; set; }

        public string Json { get; set; }
        public string JsonError { get; set; }
    }
}
