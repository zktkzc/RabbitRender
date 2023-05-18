using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rabbit_core.ECS.Components;
using Rabbit_core.Rendering.Resources;
using Rabbit_core.Tools;

namespace Rabbit_Sandbox
{
    internal class Window : GameWindow
    {
        private float _width;
        private float _height;

        private Shader? _shader;
        private Texture2D? _texture01;
        private Model? _myModel;
        private CCamera _camera;

        public Window(int width, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnLoad()
        {
            _camera = new CCamera(Guid.NewGuid());
            _transform = new CTransform(Guid.NewGuid());
            _transform.LocalPosition += new Vector3(0, 0, -10);

            /*CTransform c1 = new CTransform(Guid.NewGuid());
            c1.LocalPosition += new Vector3(1, 1, 1);
            SerializeHelper.Serialize(c1, @"C:\Users\zqzkz\Desktop\a.xml");
            SerializeHelper.Deserialize(@"C:\Users\zqzkz\Desktop\a.xml", out CTransform? c2);*/

            _myModel = Model.Create(
                @"C:\Users\zqzkz\Desktop\LearnOpenGL-master\resources\objects\backpack\backpack.obj");
            _shader = Shader.Create(@"D:\Project\C\C#\Rabbit-core\Rabbit-Sandbox\Test.glsl");
            _texture01 =
                Texture2D.Create(@"C:\Users\zqzkz\Desktop\LearnOpenGL-master\resources\objects\backpack\diffuse.jpg");
        }

        private double _totalTime;
        private Matrix4 _model;
        private CTransform _transform;

        // 每帧进行更新
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(_camera.ClearColor);

            _shader.Bind();

            _model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians((float)(_totalTime * 10)));

            _shader.SetUniform("mainTex", 0);
            _texture01.Bind();
            _camera.UpdateMatrices(_transform, _width, _height);
            _shader.SetUniform("model", _model);
            _shader.SetUniform("view", _camera.ViewMatrix);
            _shader.SetUniform("perspective", _camera.PerspectiveMatrix);

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
            _width = e.Width;
            _height = e.Height;
        }
    }
}