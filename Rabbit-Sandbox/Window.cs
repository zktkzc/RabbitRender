using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Rabbit_core.Rendering;

namespace Rabbit_Sandbox
{
    internal class Window : GameWindow
    {
        private VertexArrayObject _vao;
        private VertexBufferObject _vbo;
        private IndexBufferObject _ibo;
        private int _program;

        float[] _vertices =
        {
             0.5f,  0.5f, 0.0f, 1, 0, 0, // 右上角
             0.5f, -0.5f, 0.0f, 0, 1, 0, // 右下角
            -0.5f, -0.5f, 0.0f, 0, 0, 1, // 左下角
            -0.5f,  0.5f, 0.0f, 1, 0, 1  // 左上角
        };

        uint[] _indices =
        {
            // 注意索引从0开始
            // 此例的索引（0，1，2，3）就是顶点数组_vertices的下标
            // 这样可以由下表代表顶点组合成矩形

            0, 1, 3, // 第一个三角形
            1, 2, 3  // 第二个三角形
        };

        public Window(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); // 绘制线框

            _vbo = new VertexBufferObject(_vertices);
            VertexBufferLayout layout = new VertexBufferLayout();
            layout.AddElement(new VertexBufferLayoutElement(0, 3), new VertexBufferLayoutElement(1, 3));
            _vbo.AddLayout(layout);

            // 创建索引缓冲对象
            _ibo = new IndexBufferObject(_indices);

            _vao = new VertexArrayObject(_ibo, _vbo);

            string vertexSource = @"#version 460 core
                layout (location = 0) in vec3 aPos;
                layout (location = 1) in vec3 aColor;
                layout (location = 0) out vec3 color;

                void main()
                {
                    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
                    color = aColor;
                }";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader); // 创建顶点着色器
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader); // 编译顶点着色器

            string fragmentSource = @"#version 330 core
                out vec4 FragColor;
                layout (location = 0) in vec3 color;

                void main()
                {
                    FragColor = vec4(color, 1.0f);
                }";
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader); // 创建片段着色器
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader); // 编译片段着色器

            _program = GL.CreateProgram();
            GL.AttachShader(_program, vertexShader);
            GL.AttachShader(_program, fragmentShader);
            GL.LinkProgram(_program);
            // GL.UseProgram(_program);
        }

        // 每帧进行更新
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));
            _vao.Bind();
            GL.UseProgram(_program);
            if (_vao.IndexBufferObject == null)
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }
            else
            {
                GL.DrawElements(PrimitiveType.Triangles, _ibo.Length, DrawElementsType.UnsignedInt, 0);
            }
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
        }

        // 固定时间进行更新
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Console.WriteLine(e.Time);
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
