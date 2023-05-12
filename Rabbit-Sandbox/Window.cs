using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rabbit_core.Rendering;

namespace Rabbit_Sandbox
{
    internal class Window : GameWindow
    {
        private float width;
        private float height;
        private VertexArrayObject _vao;
        private VertexBufferObject _vbo;
        private IndexBufferObject _ibo;
        private Shader _shader;
        private Texture2D _texture01;

        float[] _vertices =
        {
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
            0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,

            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
            -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,

            -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,

            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f
        };

        uint[] _indices =
        {
            // 注意索引从0开始
            // 此例的索引（0，1，2，3）就是顶点数组_vertices的下标
            // 这样可以由下表代表顶点组合成矩形

            0, 1, 3, // 第一个三角形
            1, 2, 3 // 第二个三角形
        };

        public Window(int width, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); // 绘制线框

            _vbo = new VertexBufferObject(_vertices);
            VertexBufferLayout layout = new VertexBufferLayout();
            layout.AddElement(
                new VertexBufferLayoutElement(0, 3), // 顶点数据
                new VertexBufferLayoutElement(1, 2) // 纹理坐标
            );
            _vbo.AddLayout(layout);

            // 创建索引缓冲对象
            _ibo = new IndexBufferObject(_indices);

            _vao = new VertexArrayObject(null, _vbo);

            _shader = new Shader("""E:\Project\C\C#\Rabbit-core\Rabbit-Sandbox\Test.glsl""");

            _texture01 = new Texture2D(@"E:\Project\C\C#\Rabbit-core\Rabbit-Sandbox\wallhaven-5wwwr7.jpg");
        }

        private double _totalTime;
        private Matrix4 _model;
        private Matrix4 _view;
        private Matrix4 _perspective;

        // 每帧进行更新
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

            _vao.Bind();
            _shader.Bind();

            _model = Matrix4.CreateScale(0.5f, 0.5f, 0.5f) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(30)) *
                     Matrix4.CreateTranslation(-1, 0, 0);
            _view = Matrix4.LookAt(new Vector3(0, 0, -3), Vector3.Zero, Vector3.UnitY);
            _perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45),
                width / height,
                0.1f,
                1000f
            );

            _shader.SetUniform("mainTex", 0);
            _texture01.Bind();

            _shader.SetUniform("model", _model);
            _shader.SetUniform("view", _view);
            _shader.SetUniform("perspective", _perspective);

            GL.Enable(EnableCap.DepthTest); // 开启深度测试
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            _totalTime += args.Time; // args.Time是每帧运行的时间
            SwapBuffers();
        }

        // 固定时间进行更新
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Console.WriteLine(e.Time);
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            width = e.Width;
            height = e.Height;
        }
    }
}