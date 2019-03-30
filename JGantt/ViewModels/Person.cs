namespace JGantt.ViewModels
{
    public class Person : IPlannableItem
    {
        private string _colour;

        public Person(string name, string colour, Category cateory)
        {
            this.Name = name;
            this.Category = cateory;

            _colour = colour;
        }

        public string Name { get; set; }
        public string Colour
        {
            get
            {
                return this.Category?.Colour ?? _colour;
            }
        }
        public Category Category { get; set; }
    }
}
