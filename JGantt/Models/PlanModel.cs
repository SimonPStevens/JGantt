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
            this.PersonPlans = new List<PersonPlan>();

            foreach (var item in this.Plan)
            {
                var personPlan = this.PersonPlans.FirstOrDefault(p => p.Person == item.Person);
                if(personPlan == null)
                {
                    personPlan = new PersonPlan(item.Person);
                    this.PersonPlans.Add(personPlan);
                }

                var channel = personPlan.Channels.FirstOrDefault(c=>!c.Projects.Any(p=>p.Start < item.End && p.End > item.Start));
                if(channel == null)
                {
                    int newChannelNumber = personPlan.Channels.Any() ? personPlan.Channels.Max(c => c.ChannelNumber) + 1 : 0;
                    channel = new PersonChannel(newChannelNumber);
                    personPlan.Channels.Add(channel);
                }
                channel.Projects.Add(item);
            }
        }

        public List<PersonProject> Plan { get; set; }
        public List<PersonPlan> PersonPlans { get; set; }

        public string Json { get; set; }
        public string JsonError { get; set; }
    }

    public class PersonPlan
    {
        public PersonPlan(Person person)
        {
            Person = person;
            this.Channels = new List<PersonChannel>();
        }

        public Person Person { get; set; }
        public List<PersonChannel> Channels { get; set; }
    }

    public class PersonChannel
    {
        public PersonChannel(int channelNumber)
        {
            this.Projects = new List<PersonProject>();
            this.ChannelNumber = channelNumber;
        }

        public List<PersonProject> Projects { get; set; }
        public int ChannelNumber { get; set; }
    }

    public class Person
    {
        public Person(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
        public string Type { get; set; }
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

        public string CalcWidth()
        {
            return (int)((this.End - this.Start).TotalDays * 50)+ "px";
        }

        public string CalcLeftOffset()
        {
            return (int)((this.Start - DateTime.Now.Date).TotalDays * 50)+"px";
        }
    }

    public class Project {
        public Project(string name, string colour)
        {
            this.Name = name;
            this.Colour = colour;
        }

        public string Name { get; set; }
        public string Colour { get; set; }
    }

}
