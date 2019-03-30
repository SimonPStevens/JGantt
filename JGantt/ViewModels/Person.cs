namespace JGantt.ViewModels
{
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
}
