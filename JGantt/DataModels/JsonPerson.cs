namespace JGantt.DataModels
{
    public class JsonPerson
    {
        public JsonPerson(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}
