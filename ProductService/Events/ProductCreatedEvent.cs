namespace ProductService.Events
{
    public record ProductCreatedEvent(Guid Id, string Name);
}
