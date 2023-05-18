using System.Runtime.Serialization;

namespace Rabbit_core.ECS.Components;

[DataContract]
public class CSelfActive : IComponent
{
    public Guid Id => _id;
    [DataMember] public bool IsSelfActive { get; set; }
    [DataMember] private Guid _id;

    public CSelfActive(Guid id)
    {
        _id = id;
        IsSelfActive = true;
    }

    public CSelfActive(Guid id, bool isSelfActive)
    {
        _id = id;
        IsSelfActive = isSelfActive;
    }
}