using System;

namespace BikeDistributor
{
    public class PercentageDiscount : IBikeDiscount
    {
        public PercentageDiscount(double rate, Func<Line, bool> condition)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition)); ;
            _rate = rate >= 0 && rate <= 1 ? rate : throw new ArgumentOutOfRangeException(nameof(rate));
        }

        private Func<Line, bool> _condition;
        private double _rate;

        public double Apply(Line line, double oldPrice)
        {
            if (_condition(line))
            {
                return oldPrice * _rate;
            }
            return oldPrice;
        }
    }
}
