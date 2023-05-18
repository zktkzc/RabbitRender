using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using Rabbit_core.Rendering.Resources;
using GL = OpenTK.Graphics.OpenGL4.GL;

namespace Rabbit_core.Rendering.Buffers;

public class UniformBufferObject
{
    private int _id;

    public UniformBufferObject(int size = 1024, int bindingPoint = 0)
    {
        _id = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.UniformBuffer, _id);
        GL.BufferData(BufferTarget.UniformBuffer, size, 0, BufferUsageHint.DynamicDraw);
        GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        GL.BindBufferBase(BufferRangeTarget.UniformBuffer, bindingPoint, _id);
    }

    public void Bind()
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, _id);
    }

    public static void BindingPointToShader(Shader shader, string name, int bindingPoint = 0)
    {
        var index = GL.GetUniformBlockIndex(shader.Id, name);
        GL.UniformBlockBinding(shader.Id, index, bindingPoint);
    }

    public void Unbind()
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, 0);
    }

    public void SetData<T>(T data, int offset) where T : struct
    {
        Bind();
        GL.BufferSubData(BufferTarget.UniformBuffer, offset, Marshal.SizeOf<T>(), ref data);
        Unbind();
    }
}