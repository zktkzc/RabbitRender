using System.Runtime.Serialization;

namespace Rabbit_core.ECS.Components;

[DataContract]
public class CHierarchy : IComponent
{
    public Guid Id => _id;
    public Guid? ParentId => _parentId;
    public List<Guid> Children => _children;

    [DataMember] private Guid _id;
    [DataMember] private Guid? _parentId;
    [DataMember] private List<Guid> _children;

    public CHierarchy(Guid id)
    {
        _id = id;
        _parentId = null;
        _children = new List<Guid>();
    }
}