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
        private Shader _shader;

        float[] _vertices =
        {
             0.5f,  0.5f, 0.0f, // 右上角
             0.5f, -0.5f, 0.0f, // 右下角
            -0.5f, -0.5f, 0.0f, // 左下角
            -0.5f,  0.5f, 0.0f  // 左上角
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
            layout.AddElement(new VertexBufferLayoutElement(0, 3));
            _vbo.AddLayout(layout);

            // 创建索引缓冲对象
            _ibo = new IndexBufferObject(_indices);

            _vao = new VertexArrayObject(_ibo, _vbo);

            _shader = new Shader("""E:\Project\C\C#\Rabbit-core\Rabbit-Sandbox\Test.glsl""");
        }

        private double _totalTime;

        // 每帧进行更新
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));
            _vao.Bind();
            _shader.Bind();
            _shader.SetUniform("color", new Vector3(MathF.Sin((float)_totalTime), MathF.Cos((float)_totalTime), MathF.Atan((float)_totalTime)));
            if (_vao.IndexBufferObject == null)
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }
            else
            {
                GL.DrawElements(PrimitiveType.Triangles, _ibo.Length, DrawElementsType.UnsignedInt, 0);
            }
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            _totalTime += args.Time; // args.Time是每帧运行的时间
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
