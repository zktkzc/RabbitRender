using OpenTK.Mathematics;

namespace Rabbit_core.Maths
{
    /// <summary>
    /// 平面
    /// </summary>
    public readonly struct Plane
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;

        public Plane(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        /// <summary>
        /// 点到平面的距离
        /// </summary>
        /// <param name="point"></param>
        public readonly float DistanceToPlane(in Vector3 point) => Vector3.Dot(point, Vector3.Normalize(Normal)) - Vector3.Distance(Position, Vector3.Zero);
    }
}
