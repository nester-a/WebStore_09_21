using System.Collections.Generic;
using System.Linq;

namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<(ProductViewModel Product, int Quantity)> Items { get; set; }

        public int ItemsCount => Items?.Sum(i => i.Quantity) ?? 0;

        public decimal TotalPrice => Items?.Sum(i => i.Product.Price * i.Quantity) ?? 0m;
    }
}
