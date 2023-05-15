using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rabbit_core.Maths;
using Rabbit_core.Rendering.Resources;

namespace Rabbit_Sandbox
{
    internal class Window : GameWindow
    {
        private float width;
        private float height;

        private Shader? _shader;
        private Texture2D? _texture01;
        private Model? _myModel;

        public Window(int width, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            _myModel = Model.Create(@"C:\Users\tkzc\Desktop\5.fbx");
            _shader = Shader.Create(@"E:\Project\C\C#\Rabbit-core\Rabbit-Sandbox\Test.glsl");
            _texture01 = Texture2D.Create(@"E:\Project\C\C#\Rabbit-core\Rabbit-Sandbox\wallhaven-5wwwr7.jpg");
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

            _shader.Bind();

            _model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians((float)(_totalTime * 10))) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians((float)(_totalTime * 10)));
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

            foreach (var mesh in _myModel.Meshes)
            {
                mesh.Bind();
                GL.Enable(EnableCap.DepthTest);
                if (mesh.IndexCount > 3)
                {
                    GL.DrawElements(PrimitiveType.Triangles, mesh.IndexCount, DrawElementsType.UnsignedInt, 0);
                }
                else
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.VertexCount);
                }
            }

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