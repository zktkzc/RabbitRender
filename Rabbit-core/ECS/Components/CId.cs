using System.Runtime.Serialization;

namespace Rabbit_core.ECS.Components;

[DataContract]
public class CId : IComponent
{
    public Guid Id => _id;
    [DataMember] private Guid _id;

    public CId(Guid id)
    {
        _id = Guid.NewGuid();
    }
}