using OpenTK.Mathematics;
using Rabbit_core.Maths;

namespace Rabbit_core.Rendering.Geometry
{
    /// <summary>
    /// 视锥体
    /// </summary>
    public struct Frustum
    {
        public Plane NearPlane;
        public Plane FarPlane;
        public Plane RightPlane;
        public Plane LeftPlane;
        public Plane TopPlane;
        public Plane BottomPlane;

        public void CalculateFrustum(Camera camera, float aspect, Vector3 position, Quaternion rotation)
        {
            Vector3 front = rotation * Vector3.UnitZ;
            Vector3 right = rotation * Vector3.UnitX;
            Vector3 up = rotation * Vector3.UnitY;
            float halfVSide = camera.Far * (float)MathHelper.Tan(MathHelper.DegreesToRadians(camera.Fov / 2));
            float halfHSide = halfVSide * aspect;
            Vector3 frontMulFar = camera.Far * front;

            NearPlane = new Plane
            {
                Position = position + camera.Near * front,
                Normal = front
            };

            FarPlane = new Plane
            {
                Position = position + camera.Far * front,
                Normal = -front
            };

            RightPlane = new Plane
            {
                Position = position,
                Normal = Vector3.Cross(frontMulFar - right * halfHSide, up)
            };

            LeftPlane = new Plane
            {
                Position = position,
                Normal = Vector3.Cross(up, frontMulFar + right * halfHSide * halfHSide)
            };

            TopPlane = new Plane
            {
                Position = position,
                Normal = Vector3.Cross(right, up * halfVSide + frontMulFar)
            };

            BottomPlane = new Plane
            {
                Position = position,
                Normal = Vector3.Cross(frontMulFar - up * halfVSide, right)
            };
        }

        /// <summary>
        /// 判断包围球是否在视锥体中
        /// </summary>
        /// <param name="sphere"></param>
        /// <returns></returns>
        public bool IsSphereInFrustum(Sphere sphere) => NearPlane.DistanceToPlane(sphere.Position) >= -sphere.Radius &&
                FarPlane.DistanceToPlane(sphere.Position) >= -sphere.Radius &&
                BottomPlane.DistanceToPlane(sphere.Position) >= -sphere.Radius &&
                TopPlane.DistanceToPlane(sphere.Position) >= -sphere.Radius &&
                LeftPlane.DistanceToPlane(sphere.Position) >= -sphere.Radius &&
                RightPlane.DistanceToPlane(sphere.Position) >= -sphere.Radius;


        public bool IsBoundingSphereInFrustum(Sphere sphere)
        {
            return false;
        }
    }
}
