using ProductService.Models;

namespace ProductService.Services
{
    public class ProductStore
    {
        private readonly List<Product> _products = new();

        public void Add(Product product)=>_products.Add(product);

        public IEnumerable<Product> GetAll() => _products;
    }
}
