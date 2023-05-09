﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace Rabbit_Sandbox
{
    internal class Window : GameWindow
    {
        private int _vao;
        private int _vbo;
        private int _ebo;
        private int _program;

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
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); // 绘制线框

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            string vertexSource = @"#version 330 core
                layout (location = 0) in vec3 aPos;

                void main()
                {
                    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
                }";

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader); // 创建顶点着色器
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader); // 编译顶点着色器

            string fragmentSource = @"#version 330 core
                out vec4 FragColor;

                void main()
                {
                    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
                }";
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader); // 创建片段着色器
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader); // 编译片段着色器

            _program = GL.CreateProgram();
            GL.AttachShader(_program, vertexShader);
            GL.AttachShader(_program, fragmentShader);
            GL.LinkProgram(_program);
            // GL.UseProgram(_program);

            // 创建索引缓冲对象
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        }

        // 每帧进行更新
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.AntiqueWhite);
            GL.BindVertexArray(_vao);
            GL.UseProgram(_program);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
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
