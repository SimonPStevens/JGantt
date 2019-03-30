using System;

namespace JGantt.ViewModels
{
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
