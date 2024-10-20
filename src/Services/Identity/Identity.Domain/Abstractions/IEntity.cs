namespace Identity.Domain.Abstractions
{
    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }

    public interface IEntity;
}