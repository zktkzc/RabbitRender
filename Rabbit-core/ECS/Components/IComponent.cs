namespace Rabbit_core.ECS.Components;

public interface IComponent
{
    /// <summary>
    /// component所属的Actor(Entity)的Id
    /// </summary>
    Guid Id { get; }
}