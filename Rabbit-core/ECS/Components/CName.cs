using System.Runtime.Serialization;

namespace Rabbit_core.ECS.Components;

[DataContract]
public class CName : IComponent
{
    public Guid Id => _id;
    [DataMember] public string Name { get; set; }
    [DataMember] private Guid _id;

    public CName(Guid id)
    {
        _id = id;
        Name = "Empty";
    }

    public CName(Guid id, string name)
    {
        _id = id;
        Name = name;
    }


}