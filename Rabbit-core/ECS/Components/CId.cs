namespace Rabbit_core.ECS.Components;

public class CId : IComponent
{
    public Guid Id { get; }

    public CId(Guid id)
    {
        Id = Guid.NewGuid();
    }
}