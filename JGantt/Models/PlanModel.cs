using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JGantt.Models
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

    public class ItemPlan
    {
        public ItemPlan(IPlannableItem planableItem)
        {
            PlannableItem = planableItem;
            this.Channels = new List<Channel>();
        }

        public IPlannableItem PlannableItem { get; set; }
        public List<Channel> Channels { get; set; }
    }

    public class Channel
    {
        public Channel(int channelNumber)
        {
            this.PersonProjects = new List<PersonProject>();
            this.ChannelNumber = channelNumber;
        }

        public List<PersonProject> PersonProjects { get; set; }
        public int ChannelNumber { get; set; }
    }

    public interface IPlannableItem
    {
        string Name { get; set; }
        string Colour { get; set; }
    }

    public class Person : IPlannableItem
    {
        public Person(string name, string colour)
        {
            this.Name = name;
            this.Colour = colour;
        }

        public string Name { get; set; }
        public string Colour { get; set; }
    }

    public class Project : IPlannableItem
    {
        public Project(string name, string colour)
        {
            this.Name = name;
            this.Colour = colour;
        }

        public string Name { get; set; }
        public string Colour { get; set; }
    }

    public class PersonProject
    {
        public PersonProject(Person person, Project project, DateTime start, DateTime end)
        {
            this.Person = person;
            this.Project = project;
            this.Start = start;
            this.End = end;
        }

        public Project Project { get; set; }
        public Person Person { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public IPlannableItem GetOther(IPlannableItem item)
        {
            return this.Project == item ? (IPlannableItem)this.Person : this.Project;
        }
        public string CalcWidth()
        {
            return (int)((this.End - this.Start).TotalDays * 50)+ "px";
        }

        public string CalcLeftOffset()
        {
            return (int)((this.Start - DateTime.Now.Date).TotalDays * 50)+"px";
        }
    }
}
