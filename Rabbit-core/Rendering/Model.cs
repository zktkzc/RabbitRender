namespace Rabbit_core.Rendering;

public class Model : IDisposable
{
    public List<Mesh> Meshes { get; set; }
    public Dictionary<int, string> MaterialDic { get; }

    public Model(List<Mesh> meshes, Dictionary<int, string> materialDic)
    {
        Meshes = meshes;
        MaterialDic = materialDic;
    }

    public void Dispose()
    {
        foreach (var mesh in Meshes)
        {
            mesh.Dispose();
        }
    }
}