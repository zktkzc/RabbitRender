using OpenTK.Mathematics;

namespace Rabbit_core.Maths
{
    /// <summary>
    /// 球体
    /// </summary>
    public readonly struct Sphere
    {
        public readonly Vector3 Position;
        public readonly float Radius;

        public Sphere(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
    }
}
