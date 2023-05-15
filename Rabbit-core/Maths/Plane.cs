using OpenTK.Mathematics;

namespace Rabbit_core.Maths
{
    /// <summary>
    /// 平面
    /// </summary>
    public struct Plane
    {
        public Vector3 Position;
        public Vector3 Normal;

        /// <summary>
        /// 点到平面的距离
        /// </summary>
        /// <param name="point"></param>
        public float DistanceToPlane(Vector3 point) => Vector3.Dot(point, Normal.Normalized()) - Vector3.Distance(Position, Vector3.Zero);
    }
}
