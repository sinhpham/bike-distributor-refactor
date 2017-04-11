using System;
using System.Collections.Generic;
using System.Linq;

namespace BikeDistributor
{
    public class Line
    {
        public Line(Bike bike, int quantity)
        {
            Bike = bike ?? throw new ArgumentNullException(nameof(bike));
            Quantity = quantity > 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        public double CalcPrice(IEnumerable<IBikeDiscount> discounts)
        {
            if (discounts == null)
            {
                return Quantity * Bike.Price;
            }
            // Assuming discounts can accumulate
            // Maybe apply all fixed discounts first before percentage discounts?
            var discountedPrice = discounts.Aggregate((double)Bike.Price, (currPrice, currDiscount) => currPrice = currDiscount.Apply(this, currPrice));

            return Quantity * discountedPrice;
        }

        public Bike Bike { get; private set; }
        public int Quantity { get; private set; }
    }
}
