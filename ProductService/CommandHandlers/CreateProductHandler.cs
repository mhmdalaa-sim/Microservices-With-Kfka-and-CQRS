using Confluent.Kafka;
using MediatR;
using ProductService.Commands;
using ProductService.Events;
using ProductService.Models;
using ProductService.Services;
using System.Text.Json;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly ProductStore _store;
    private readonly IProducer<Null, string> _producer;

    public CreateProductHandler(ProductStore store, IProducer<Null, string> producer)
    {
        _store = store;
        _producer = producer;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product { Id = Guid.NewGuid(), Name = request.Name };
        _store.Add(product);

        var @event = new ProductCreatedEvent(product.Id, product.Name);
        var message = new Message<Null, string> { Value = JsonSerializer.Serialize(@event) };

        await _producer.ProduceAsync("product-created", message, cancellationToken);
        return product.Id;
    }
}
