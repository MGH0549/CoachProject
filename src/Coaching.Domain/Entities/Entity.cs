namespace Coaching.Domain;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public byte[] RowVersion { get; set; } = [];
}
