using System.Collections.Generic;

namespace JGantt.ViewModels
{
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
}
