using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.IO;
using Rabbit_core.Log;
using Vector2 = OpenTK.Mathematics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector4 = OpenTK.Mathematics.Vector4;

namespace Rabbit_core.Rendering.Resources
{
    public class Shader : IDisposable
    {
        public int Id { get; private set; }
        public string? Path { get; }
        private Dictionary<string, int> _cache = new Dictionary<string, int>();
        public bool IsDestory { get; private set; } = false;
        public List<UniformInfo> UniformInfos { get; } = new List<UniformInfo>();

        private Shader(string path)
        {
            Path = path;
            (string vertexShaderSource, string fragmentShaderSource) = LoadShaderFromDisk(path);
            CreateProgram(vertexShaderSource, fragmentShaderSource);
            QueryUniforms();
        }

        private static (string vertexShaderSource, string fragmentShaderSource) LoadShaderFromDisk(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int vertexIndex = Array.IndexOf(lines, "#shader vertex");
            int fragmentIndex = Array.IndexOf(lines, "#shader fragment");
            string[] vertexLines = lines.Skip(vertexIndex + 1).Take(fragmentIndex - vertexIndex - 1).ToArray();
            string[] fragmentLines = lines.Skip(fragmentIndex + 1).ToArray();
            string vertexShader = string.Join("\n", vertexLines);
            string fragmentShader = string.Join("\n", fragmentLines);
            return (vertexShader, fragmentShader);
        }

        private Shader(string vertexShaderSource, string fragmentShaderSource)
        {
            CreateProgram(vertexShaderSource, fragmentShaderSource);
            QueryUniforms();
        }

        public static Shader? Create(string path)
        {
            Shader? shader = null;
            try
            {
                shader = new Shader(path);
            }
            catch (Exception e)
            {
                RaLog.ErrorLogCore(e.Message);
            }

            return shader;
        }

        public static Shader Create(string vertexShaderSource, string fragmentShaderSource) =>
            new Shader(vertexShaderSource, fragmentShaderSource);

        private void CreateProgram(string vertexShaderSource, string fragmentShaderSource)
        {
            int vertexShader = CreateShader(ShaderType.VertexShader, vertexShaderSource);
            int fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderSource);

            Id = GL.CreateProgram();
            GL.AttachShader(Id, vertexShader);
            GL.AttachShader(Id, fragmentShader);
            GL.LinkProgram(Id);
            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                GL.GetShaderInfoLog(Id, out string info);
                RaLog.ErrorLogCore(info);
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Bind()
        {
            GL.UseProgram(Id);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public void SetUniform(string name, int v) => GL.Uniform1(GetUniformLocation(name), v);
        public void SetUniform(string name, float v) => GL.Uniform1(GetUniformLocation(name), v);
        public void SetUniform(string name, Vector2 v) => GL.Uniform2(GetUniformLocation(name), v);
        public void SetUniform(string name, Vector3 v) => GL.Uniform3(GetUniformLocation(name), v);
        public void SetUniform(string name, Vector4 v) => GL.Uniform4(GetUniformLocation(name), v);
        public void SetUniform(string name, Matrix2x3 v) => GL.UniformMatrix2x3(GetUniformLocation(name), false, ref v);
        public void SetUniform(string name, Matrix2x4 v) => GL.UniformMatrix2x4(GetUniformLocation(name), false, ref v);
        public void SetUniform(string name, Matrix3x4 v) => GL.UniformMatrix3x4(GetUniformLocation(name), false, ref v);
        public void SetUniform(string name, Matrix2 v) => GL.UniformMatrix2(GetUniformLocation(name), false, ref v);
        public void SetUniform(string name, Matrix3 v) => GL.UniformMatrix3(GetUniformLocation(name), false, ref v);
        public void SetUniform(string name, Matrix4 v) => GL.UniformMatrix4(GetUniformLocation(name), false, ref v);
        public void GetUniform(string name, out int v) => GL.GetUniform(Id, GetUniformLocation(name), out v);
        public void GetUniform(string name, out float v) => GL.GetUniform(Id, GetUniformLocation(name), out v);

        public void GetUniform(string name, out Vector2 v)
        {
            float[] res = new float[2];
            GL.GetUniform(Id, GetUniformLocation(name), res);
            v = new Vector2(res[0], res[1]);
        }

        public void GetUniform(string name, out Vector3 v)
        {
            float[] res = new float[3];
            GL.GetUniform(Id, GetUniformLocation(name), res);
            v = new Vector3(res[0], res[1], res[2]);
        }

        public void GetUniform(string name, out Vector4 v)
        {
            float[] res = new float[4];
            GL.GetUniform(Id, GetUniformLocation(name), res);
            v = new Vector4(res[0], res[1], res[2], res[3]);
        }

        public void GetUniform(string name, out Matrix3 v)
        {
            float[] res = new float[9];
            GL.GetUniform(Id, GetUniformLocation(name), res);
            v = new Matrix3(res[0], res[1], res[2], res[3], res[4], res[5], res[6], res[7], res[8]);
        }

        public void GetUniform(string name, out Matrix4 v)
        {
            float[] res = new float[16];
            GL.GetUniform(Id, GetUniformLocation(name), res);
            v = new Matrix4(res[0], res[1], res[2], res[3], res[4], res[5], res[6], res[7], res[8], res[9], res[10],
                res[11], res[12], res[13], res[14], res[15]);
        }

        // 重新编译着色器
        public void ReCompile()
        {
            if (Path != null)
            {
                GL.DeleteProgram(Id);
                (string vertexShaderSource, string fragmentShaderSource) = LoadShaderFromDisk(Path);
                CreateProgram(vertexShaderSource, fragmentShaderSource);
            }
        }

        public void QueryUniforms()
        {
            UniformInfos.Clear(); // 清除之前的值
            GL.GetProgram(Id, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            for (int i = 0; i < uniformCount; i++)
            {
                GL.GetActiveUniform(Id, i, 1024, out _, out _, out ActiveUniformType uniformType, out string name);
                object? value = null;
                switch (uniformType)
                {
                    case ActiveUniformType.Bool:
                        int v;
                        GetUniform(name, out v);
                        value = v;
                        break;
                    case ActiveUniformType.Int:
                        int v1;
                        GetUniform(name, out v1);
                        value = v1;
                        break;
                    case ActiveUniformType.Float:
                        float v2;
                        GetUniform(name, out v2);
                        value = v2;
                        break;
                    case ActiveUniformType.FloatVec2:
                        Vector2 v3;
                        GetUniform(name, out v3);
                        value = v3;
                        break;
                    case ActiveUniformType.FloatVec3:
                        Vector3 v4;
                        GetUniform(name, out v4);
                        value = v4;
                        break;
                    case ActiveUniformType.FloatVec4:
                        Vector4 v5;
                        GetUniform(name, out v5);
                        value = v5;
                        break;
                    case ActiveUniformType.FloatMat3:
                        Matrix3 v6;
                        GetUniform(name, out v6);
                        value = v6;
                        break;
                    case ActiveUniformType.FloatMat4:
                        Matrix4 v7;
                        GetUniform(name, out v7);
                        value = v7;
                        break;
                    case ActiveUniformType.Sampler2D:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                UniformInfos.Add(new UniformInfo(name, GetUniformLocation(name), uniformType, value));
            }
        }

        // TODO 查询所有激活的uniform，这样就可以在材质中对它赋值

        private int GetUniformLocation(string name)
        {
            if (_cache.ContainsKey(name)) return _cache[name];
            int location = GL.GetUniformLocation(Id, name);
            _cache.Add(name, location);
            return location;
        }

        private int CreateShader(ShaderType type, string source)
        {
            int id = GL.CreateShader(type);
            GL.ShaderSource(id, source);
            GL.CompileShader(id);
            GL.GetShader(id, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                GL.GetShaderInfoLog(id, out string info); // 获取报错信息

                RaLog.ErrorLogCore($"{type.ToString()}: {info}");
            }

            return id;
        }

        private void ReleaseUnmanagedResources()
        {
            GL.DeleteProgram(Id);
        }

        public void Dispose()
        {
            if (!IsDestory)
            {
                ReleaseUnmanagedResources();
                GC.SuppressFinalize(this);
                IsDestory = true;
            }
        }

        ~Shader()
        {
            ReleaseUnmanagedResources();
        }
    }
}