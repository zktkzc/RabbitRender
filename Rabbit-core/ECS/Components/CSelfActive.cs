namespace Rabbit_core.ECS.Components;

public class CSelfActive : IComponent
{
    public Guid Id { get; }
    public bool IsSelfActive { get; set; }

    public CSelfActive(Guid id)
    {
        Id = id;
        IsSelfActive = true;
    }
    
    public CSelfActive(Guid id, bool isSelfActive)
    {
        Id = id;
        IsSelfActive = isSelfActive;
    }
}