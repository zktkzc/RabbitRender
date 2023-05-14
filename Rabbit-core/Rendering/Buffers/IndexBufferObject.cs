using OpenTK.Graphics.OpenGL4;

namespace Rabbit_core.Rendering.Buffers
{
    public class IndexBufferObject : IDisposable
    {
        private readonly int _id;
        public int Length { get; }

        public IndexBufferObject(uint[] indices)
        {
            Length = indices.Length;
            _id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _id);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void ReleaseUnmanagedResources()
        {
            GL.DeleteBuffer(_id);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~IndexBufferObject()
        {
            ReleaseUnmanagedResources();
        }
    }
}
