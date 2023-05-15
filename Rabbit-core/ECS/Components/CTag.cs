namespace Rabbit_core.ECS.Components;

public class CTag : IComponent
{
    public Guid Id { get; }
    public string Tag { get; set; }

    public CTag(Guid id)
    {
        Id = id;
        Tag = "";
    }

    public CTag(Guid id, string tag)
    {
        Id = id;
        Tag = tag;
    }
}