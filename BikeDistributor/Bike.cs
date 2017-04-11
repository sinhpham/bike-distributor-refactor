using System;

namespace BikeDistributor
{
    public class Bike
    {
        public Bike(string brand, string model, int price)
        {
            Brand = !string.IsNullOrEmpty(brand) ? brand : throw new ArgumentOutOfRangeException(nameof(brand));
            Model = !string.IsNullOrEmpty(model) ? model : throw new ArgumentOutOfRangeException(nameof(model));
            // Should save price as priceInCents, and careful with using int/doulbe for currency amount.
            Price = price > 0 ? price : throw new ArgumentOutOfRangeException(nameof(price));
        }

        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int Price { get; private set; }
    }
}
