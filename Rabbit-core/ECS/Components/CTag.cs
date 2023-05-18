using System.Runtime.Serialization;

namespace Rabbit_core.ECS.Components;

[DataContract]
public class CTag : IComponent
{
    [DataMember] private Guid _id;
    public Guid Id => _id;
    [DataMember] public string Tag { get; set; }

    public CTag(Guid id)
    {
        _id = id;
        Tag = "";
    }

    public CTag(Guid id, string tag)
    {
        _id = id;
        Tag = tag;
    }
}