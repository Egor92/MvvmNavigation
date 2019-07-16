namespace RestaurantApp.Models
{
    public class Food
    {
        public Food(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }

        public double Price { get; private set; }
    }
}