namespace JGantt.DataModels
{
    public class JsonProject
    {
        public JsonProject(string name, string colour)
        {
            this.Name = name;
            this.Colour = colour;
        }

        public string Name { get; set; }
        public string Colour { get; set; }
    }
}
