using Rabbit_core.Rendering.Geometry;

namespace Rabbit_core.Rendering;

public class Mesh : IDisposable
{
    public int VertexCount { get; }
    public int IndexCount { get; }
    public int MaterialIndex { get; }

    private IndexBufferObject? _indexBufferObject;
    private VertexBufferObject _vertexBufferObject;
    private VertexArrayObject _vertexArrayObject;

    public Mesh(List<Vertex> vertices, List<uint> indices, int materialIndex)
    {
        VertexCount = vertices.Count;
        IndexCount = indices.Count;
        MaterialIndex = materialIndex;
        
        CreateBuffer(vertices, indices);
    }

    private void CreateBuffer(List<Vertex> vertices, List<uint> indices)
    {
        List<float> vertexData = new();
        foreach (var vertex in vertices)
        {
            vertexData.Add(vertex.Position.X);
            vertexData.Add(vertex.Position.Y);
            vertexData.Add(vertex.Position.Z);

            vertexData.Add(vertex.Normal.X);
            vertexData.Add(vertex.Normal.Y);
            vertexData.Add(vertex.Normal.Z);

            vertexData.Add(vertex.TexCoords.X);
            vertexData.Add(vertex.TexCoords.Y);

            vertexData.Add(vertex.Tangent.X);
            vertexData.Add(vertex.Tangent.Y);
            vertexData.Add(vertex.Tangent.Z);

            vertexData.Add(vertex.BitTangent.X);
            vertexData.Add(vertex.BitTangent.Y);
            vertexData.Add(vertex.BitTangent.Z);
        }

        _vertexBufferObject = new VertexBufferObject(vertexData.ToArray());
        VertexBufferLayout layout = new VertexBufferLayout();
        layout.AddElement(
            new VertexBufferLayoutElement(0, 3),
            new VertexBufferLayoutElement(1, 3),
            new VertexBufferLayoutElement(2, 2),
            new VertexBufferLayoutElement(3, 3),
            new VertexBufferLayoutElement(4, 3)
        );
        _vertexBufferObject.AddLayout(layout);

        if (indices.Count > 3)
        {
            _indexBufferObject = new IndexBufferObject(indices.ToArray());
        }

        _vertexArrayObject = new VertexArrayObject(_indexBufferObject, _vertexBufferObject);
    }

    public void Bind()
    {
        _vertexArrayObject.Bind();
    }

    public void Unbind()
    {
        _vertexArrayObject.Unbind();
    }

    public void Dispose()
    {
        _indexBufferObject?.Dispose();
        _vertexBufferObject.Dispose();
        _vertexArrayObject.Dispose();
    }
}