using System.Collections.Generic;
using System.Linq;

namespace JGantt.ViewModels
{
    public class ItemPlan
    {
        public ItemPlan(IPlannableItem planableItem)
        {
            PlannableItem = planableItem;
            this.Channels = new List<Channel>();
        }

        public IPlannableItem PlannableItem { get; set; }
        public List<Channel> Channels { get; set; }

        public int ViewChannelCount()
        {
            switch (this.PlannableItem)
            {
                case Project proj:
                    return this.Channels.Count + proj.Milestones.GroupBy(m=>m.Date).MaxOrDefault(g=>g.Count(), 0);

                case Person p:
                default:
                    return this.Channels.Count;
            }
        }
    }
}
