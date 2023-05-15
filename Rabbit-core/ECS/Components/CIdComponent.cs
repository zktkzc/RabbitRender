namespace Rabbit_core.ECS.Components;

public class CIdComponent : IComponent
{
    public Guid Id { get; }

    public CIdComponent(Guid id)
    {
        Id = Guid.NewGuid();
    }
}