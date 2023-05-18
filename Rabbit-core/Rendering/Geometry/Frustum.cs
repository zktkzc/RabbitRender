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
            var front = rotation * Vector3.UnitZ;
            var right = rotation * Vector3.UnitX;
            var up = rotation * Vector3.UnitY;
            var halfVSide = camera.Far * (float)MathHelper.Tan(MathHelper.DegreesToRadians(camera.Fov / 2));
            var halfHSide = halfVSide * aspect;
            var frontMulFar = camera.Far * front;

            NearPlane = new Plane(position + camera.Near * front, front);
            FarPlane = new Plane(position + camera.Far * front, -front);
            RightPlane = new Plane(position, Vector3.Cross(frontMulFar - right * halfHSide, up));
            LeftPlane = new Plane(position, Vector3.Cross(up, frontMulFar + right * halfHSide * halfHSide));
            TopPlane = new Plane(position, Vector3.Cross(right, up * halfVSide + frontMulFar));
            BottomPlane = new Plane(position, Vector3.Cross(frontMulFar - up * halfVSide, right));
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


        public bool IsBoundingSphereInFrustum(Quaternion rotation, Vector3 scale, Sphere sphere)
        {
            float radius = MathHelper.Max(MathHelper.Max(scale.X, scale.Y), scale.Z) * sphere.Radius;
            Vector3 centerPos = rotation * sphere.Position;
            return IsSphereInFrustum(new Sphere(centerPos, radius));
        }
    }
}