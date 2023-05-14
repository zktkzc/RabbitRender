using OpenTK.Graphics.OpenGL4;

namespace Rabbit_core.Rendering.Resources
{
    public struct UniformInfo
    {
        public string Name;
        public int Location;
        public ActiveUniformType Type;
        public object? Value;

        public UniformInfo(string name, int location, ActiveUniformType type, object? value)
        {
            Name = name;
            Location = location;
            Type = type;
            Value = value;
        }
    }
}
