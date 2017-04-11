using System;
using System.Collections.Generic;
using System.Linq;
using RazorLight;

namespace BikeDistributor
{
    public class Order
    {
        public Order(string company, double taxRate = 0.0725, IEnumerable<IBikeDiscount> discounts = null)
        {
            Company = !string.IsNullOrEmpty(company) ? company : throw new ArgumentOutOfRangeException(nameof(company));
            TaxRate = taxRate >= 0 ? taxRate : throw new ArgumentOutOfRangeException(nameof(taxRate));

            _discounts = discounts != null ? discounts.ToList() : DefaultDiscounts();
        }

        public string Company { get; private set; }
        public double TaxRate { get; private set; }

        private readonly List<Line> _lines = new List<Line>();
        private readonly List<IBikeDiscount> _discounts;
        private readonly IRazorLightEngine _templateEngine = EngineFactory.CreateEmbedded(typeof(OrderViewModel));

        public void AddLine(Line line)
        {
            _lines.Add(line);
        }

        public OrderViewModel PrepareViewModel()
        {
            return new OrderViewModel(this, _lines, _discounts);
        }

        public string Receipt()
        {
            var vm = PrepareViewModel();
            var ret = _templateEngine.Parse("Views.StringReceipt", vm);
            return ret;
        }

        public string HtmlReceipt()
        {
            var vm = PrepareViewModel();
            var ret = _templateEngine.Parse("Views.HtmlReceipt", vm);
            return ret;
        }

        private List<IBikeDiscount> DefaultDiscounts()
        {
            var d1 = new PercentageDiscount(0.9, line => line.Bike.Price == 1000 && line.Quantity >= 20);
            var d2 = new PercentageDiscount(0.8, line => line.Bike.Price == 2000 && line.Quantity >= 10);
            var d3 = new PercentageDiscount(0.8, line => line.Bike.Price == 5000 && line.Quantity >= 5);

            return new List<IBikeDiscount> { d1, d2, d3 };
        }
    }

    public class OrderViewModel
    {
        public string Company { get; private set; }
        public List<LineViewModel> Lines { get; private set; }
        public double PreTaxAmount { get; private set; }
        public double Tax { get; private set; }
        public double TotalAmount { get; private set; }

        public OrderViewModel(Order model, IEnumerable<Line> lines, IEnumerable<IBikeDiscount> discounts)
        {
            Company = model.Company;
            Lines = lines.Select(line => new LineViewModel(line, discounts)).ToList();

            PreTaxAmount = Lines.Aggregate(0.0, (currAmount, currLine) => currAmount += currLine.Amount);
            Tax = model.TaxRate * PreTaxAmount;
            TotalAmount = PreTaxAmount + Tax;
        }
    }

    public class LineViewModel
    {
        public int Quantity { get; private set; }
        public string Brand { get; private set; }
        public string BikeModel { get; private set; }
        public double Amount { get; private set; }

        public LineViewModel(Line model, IEnumerable<IBikeDiscount> discounts)
        {
            Quantity = model.Quantity;
            Brand = model.Bike.Brand;
            BikeModel = model.Bike.Model;
            Amount = model.CalcPrice(discounts);
        }
    }
}
