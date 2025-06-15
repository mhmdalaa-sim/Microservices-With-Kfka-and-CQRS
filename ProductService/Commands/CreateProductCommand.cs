using MediatR;
namespace ProductService.Commands
{
    public record CreateProductCommand(string Name) : IRequest<Guid>;


}
