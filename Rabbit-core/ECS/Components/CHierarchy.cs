namespace Rabbit_core.ECS.Components;

public class CHierarchy : IComponent
{
    public Guid Id { get; }
    public Guid? ParentId { get; internal set; }
    public List<Guid> Children { get;}

    public CHierarchy(Guid id)
    {
        Id = id;
        ParentId = null;
        Children = new List<Guid>();
    }
}