﻿using OpenTK.Graphics.OpenGL4;

namespace Rabbit_core.Rendering.Buffers
{
    public class VertexArrayObject : IDisposable
    {
        private readonly int _id;
        public IndexBufferObject? IndexBufferObject { get; }

        public VertexArrayObject(IndexBufferObject? indexBufferObject, params VertexBufferObject[] vertexBufferObjects)
        {
            IndexBufferObject = indexBufferObject;
            _id = GL.GenVertexArray();
            GL.BindVertexArray(_id);

            foreach (var vertexBufferObject in vertexBufferObjects)
            {
                vertexBufferObject.Bind();
                int offset = 0;
                foreach (var element in vertexBufferObject.Layout.Elements)
                {
                    GL.VertexAttribPointer(element.Location, element.Count, VertexAttribPointerType.Float, element.IsNormalized, vertexBufferObject.Stride, offset);
                    GL.EnableVertexAttribArray(element.Location);
                    offset += element.Count * sizeof(float);
                }
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(_id);
            IndexBufferObject?.Bind();
        }

        public void Unbind()
        {
            GL.DeleteVertexArray(0);
        }

        private void ReleaseUnmanagedResources()
        {
            GL.DeleteVertexArray(_id);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~VertexArrayObject()
        {
            ReleaseUnmanagedResources();
        }
    }
}
