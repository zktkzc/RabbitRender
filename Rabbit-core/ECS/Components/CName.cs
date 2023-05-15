namespace Rabbit_core.ECS.Components;

public class CName : IComponent
{
    public Guid Id { get; }
    public string Name { get; set; }

    public CName(Guid id)
    {
        Id = id;
        Name = "Empty";
    }
    
    public CName(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    
}