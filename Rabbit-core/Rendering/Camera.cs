using OpenTK.Mathematics;
using Rabbit_core.Rendering.Enums;

namespace Rabbit_core.Rendering
{
    /// <summary>
    /// 相机
    /// </summary>
    public struct Camera
    {
        public float Fov;
        public float Near;
        public float Far;
        public Color4 ClearColor;
        public bool IsFrustumGeometryCulling;
        public bool IsFrustumLightCulling;
        public EProjectionMode ProjectionMode;

        public Matrix4 ViewMatrix;
        public Matrix4 PerspectiveMatrix;

        public Camera()
        {
            Fov = 45;
            Near = 0.1f;
            Far = 1000f;
            ClearColor = Color4.DarkCyan;
            IsFrustumGeometryCulling = true;
            IsFrustumLightCulling = false;
            ProjectionMode = EProjectionMode.Perspective;
        }

        public void UpdateMatrices(Vector3 position, Quaternion rotation, float width, float height)
        {
            Vector3 unitZ = rotation * Vector3.UnitZ;
            ViewMatrix = Matrix4.LookAt(position, position + unitZ, Vector3.UnitY);
            PerspectiveMatrix = ProjectionMode == EProjectionMode.Perspective ? Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), width / height, Near, Far) : Matrix4.CreateOrthographic(width, height, Near, Far);
        }
    }
}
