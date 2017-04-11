namespace BikeDistributor
{
    public interface IBikeDiscount
    {
        double Apply(Line line, double oldPrice);
    }
}
