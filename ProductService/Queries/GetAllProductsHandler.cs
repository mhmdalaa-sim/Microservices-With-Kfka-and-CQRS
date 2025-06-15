using MediatR;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Queries
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly ProductStore _store;

        public GetAllProductsHandler(ProductStore store)
        {
            _store = store;
        }
        public Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_store.GetAll().ToList());
        }
    }
}
