﻿using Assimp;
using OpenTK.Mathematics;
using Rabbit_core.Log;
using Rabbit_core.Maths;
using Rabbit_core.Rendering.Geometry;

namespace Rabbit_core.Rendering.Resources
{
    public class Model : IDisposable
    {
        public string Path { get; }
        public List<Mesh> Meshes { get; set; }
        public List<string> MaterialNames { get; }
        public bool IsDestroy { get; private set; } = false;
        public Sphere BoundingSphere { get; private set; }

        private Model(string path)
        {
            Path = path;
            (Meshes, MaterialNames) = LoadModel(path);
            CreateBoundingShpere(Meshes);
        }

        public static Model? Create(string path)
        {
            Model? model = null;
            try
            {
                model = new Model(path);
            }
            catch (Exception e)
            {
                RaLog.ErrorLogCore(e.Message);
            }
            return model;
        }

        // 加载模型
        public static (List<Mesh> meshes, List<string> materialNames) LoadModel(string path,
            PostProcessSteps postProcessSteps = PostProcessSteps.None)
        {
            AssimpContext assimp = new AssimpContext();
            Scene scene = assimp.ImportFile(path, postProcessSteps);
            if (scene is null || (scene.SceneFlags & SceneFlags.Incomplete) == SceneFlags.Incomplete ||
                scene?.RootNode is null)
            {
                Console.WriteLine($"ERROR:ASSIMP");
                return (null, null);
            }

            List<string> matrialNames = ProcessMaterials(scene);
            List<Mesh> meshes = new List<Mesh>();
            ProcessNode(scene, meshes, scene.RootNode, Matrix4x4.Identity);
            return (meshes, matrialNames);
        }

        private static void ProcessNode(Scene scene, List<Mesh> meshes, Node rootNode, Matrix4x4 transform)
        {
            var nodeTransform = rootNode.Transform * transform;
            foreach (var index in rootNode.MeshIndices)
            {
                Assimp.Mesh mesh = scene.Meshes[index];
                meshes.Add(ProcessMesh(mesh, nodeTransform));
            }

            foreach (var node in rootNode.Children)
            {
                ProcessNode(scene, meshes, node, nodeTransform);
            }
        }

        private static Mesh ProcessMesh(Assimp.Mesh mesh, Matrix4x4 transform)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();
            int materialIndex = mesh.MaterialIndex;
            string meshName = mesh.Name;

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                var position = transform * (mesh.HasVertices ? mesh.Vertices[i] : new Vector3D(0, 0, 0));
                var normal = transform * (mesh.HasNormals ? mesh.Normals[i] : new Vector3D(0, 0, 0));
                var texCoords = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i] : new Vector3D(0, 0, 0);
                var tangent = transform * (mesh.HasTangentBasis ? mesh.Tangents[i] : new Vector3D(0, 0, 0));
                var bitTangent = transform * (mesh.HasTangentBasis ? mesh.Tangents[i] : new Vector3D(0, 0, 0));

                Vertex vertex = new Vertex();
                vertex.Position.X = position.X;
                vertex.Position.Y = position.Y;
                vertex.Position.Z = position.Z;

                vertex.Normal.X = normal.X;
                vertex.Normal.Y = normal.Y;
                vertex.Normal.Z = normal.Z;

                vertex.TexCoords.X = texCoords.X;
                vertex.TexCoords.Y = texCoords.Y;

                vertex.Tangent.X = tangent.X;
                vertex.Tangent.Y = tangent.Y;
                vertex.Tangent.Z = tangent.Z;

                vertex.BitTangent.X = bitTangent.X;
                vertex.BitTangent.Y = bitTangent.Y;
                vertex.BitTangent.Z = bitTangent.Z;

                vertices.Add(vertex);
            }

            if (mesh.HasFaces)
            {
                foreach (var face in mesh.Faces)
                {
                    foreach (var faceIndex in face.Indices)
                    {
                        indices.Add((uint)faceIndex);
                    }
                }
            }

            Mesh resultMesh = new Mesh(vertices, indices, materialIndex, meshName);
            return resultMesh;
        }

        private static List<string> ProcessMaterials(Scene scene)
        {
            List<string> materialNames = new List<string>();
            if (scene.HasMaterials)
            {
                foreach (var material in scene.Materials)
                {
                    materialNames.Add(material.HasName ? material.Name : "??");
                }
            }

            return materialNames;
        }

        private void CreateBoundingShpere(List<Mesh> meshes)
        {
            if (meshes.Count == 1) BoundingSphere = meshes[0].BoundingSphere;
            else if (meshes.Count > 1)
            {
                var minX = float.MaxValue;
                var minY = float.MaxValue;
                var minZ = float.MaxValue;

                var maxX = float.MinValue;
                var maxY = float.MinValue;
                var maxZ = float.MinValue;

                foreach (var mesh in meshes)
                {
                    minX = MathHelper.Min(minX, mesh.BoundingSphere.Position.X - mesh.BoundingSphere.Radius);
                    minY = MathHelper.Min(minY, mesh.BoundingSphere.Position.Y - mesh.BoundingSphere.Radius);
                    minZ = MathHelper.Min(minZ, mesh.BoundingSphere.Position.Z - mesh.BoundingSphere.Radius);

                    maxX = MathHelper.Max(maxX, mesh.BoundingSphere.Position.X + mesh.BoundingSphere.Radius);
                    maxY = MathHelper.Max(maxY, mesh.BoundingSphere.Position.Y + mesh.BoundingSphere.Radius);
                    maxZ = MathHelper.Max(maxZ, mesh.BoundingSphere.Position.Z + mesh.BoundingSphere.Radius);
                }

                var position = new Vector3(minX + maxX, minY + maxY, minZ + maxZ) / 2;
                var radius = MathHelper.Max(Vector3.Distance(position, new Vector3(minX, minY, minZ)), Vector3.Distance(position, new Vector3(maxX, maxY, maxZ)));
                BoundingSphere = new Sphere(position, radius);
            }

        }

        public void Dispose()
        {
            if (!IsDestroy)
            {
                foreach (var mesh in Meshes)
                {
                    mesh.Dispose();
                }
                IsDestroy = true;
            }
        }
    }
}