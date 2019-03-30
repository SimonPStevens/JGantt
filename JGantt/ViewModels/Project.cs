using JGantt.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace JGantt.ViewModels
{
    public class Project : IPlannableItem, IKeyItem
    {
        public Project(string name, string colour, IEnumerable<Milestone> milestones)
        {
            this.Name = name;
            this.Colour = colour;
            this.Milestones = milestones?.ToList() ?? new List<Milestone>();
            foreach (var milestone in this.Milestones)
            {
                milestone.Project = this;
            }
        }

        public string Name { get; set; }
        public string Colour { get; set; }

        public List<Milestone> Milestones { get; set; }
    }
}
