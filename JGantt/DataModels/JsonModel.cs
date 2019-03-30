using System.Collections.Generic;

namespace JGantt.DataModels
{
    public class JsonModel
    {
        public List<JsonProject> Projects { get; set; }
        public List<JsonPersonProject> Plan { get; set; }
        public List<JsonMilestone> Milestones { get; set; }
    }
}
