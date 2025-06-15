using MediatR;
using ProductService.Models;

namespace ProductService.Queries
{
    public record GetAllProductsQuery() : IRequest<List<Product>>;
}
