using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Rabbit_core.Log;
using StbImageSharp;

namespace Rabbit_core.Rendering.Resources
{
    public class Texture2D : IDisposable
    {
        public int Id { get; }
        public string? Path { get; }
        public TextureWrapMode WrapModeS { get; set; }
        public TextureWrapMode WrapModeT { get; set; }
        public TextureMagFilter TextureMagFilter { get; set; }
        public TextureMinFilter TextureMinFilter { get; set; }
        public bool IsMinmap { get; set; }
        public bool IsDestory { get; private set; } = false;

        private Texture2D(
            string path,
            TextureWrapMode wrapModeS,
            TextureWrapMode wrapModeT,
            TextureMagFilter magFilter,
            TextureMinFilter minFilter,
            bool isMinmap
            )
        {
            ImageResult? image = LoadTexture2DFromDisk(path);
            if (image != null)
            {
                Path = path;
                Id = GL.GenTexture();
                IsMinmap = isMinmap;
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, Id);
                WrapModeS = wrapModeS;
                WrapModeT = wrapModeT;
                TextureMagFilter = magFilter;
                TextureMinFilter = minFilter;
                int ws = (int)wrapModeS;
                int wt = (int)wrapModeT;
                int magF = (int)magFilter;
                int minF = (int)minFilter;
                GL.TextureParameterI(Id, TextureParameterName.TextureWrapS, ref ws);
                GL.TextureParameterI(Id, TextureParameterName.TextureWrapT, ref wt);
                GL.TextureParameterI(Id, TextureParameterName.TextureMagFilter, ref magF);
                GL.TextureParameterI(Id, TextureParameterName.TextureMinFilter, ref minF);

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    image.Data
                );

                if (IsMinmap) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private Texture2D(Color4 color)
        {
            Id = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);
            WrapModeS = TextureWrapMode.Repeat;
            WrapModeT = TextureWrapMode.Repeat;
            TextureMagFilter = TextureMagFilter.Linear;
            TextureMinFilter = TextureMinFilter.Nearest;
            int ws = (int)WrapModeS;
            int wt = (int)WrapModeT;
            int magF = (int)TextureMagFilter;
            int minF = (int)TextureMinFilter;
            GL.TextureParameterI(Id, TextureParameterName.TextureWrapS, ref ws);
            GL.TextureParameterI(Id, TextureParameterName.TextureWrapT, ref wt);
            GL.TextureParameterI(Id, TextureParameterName.TextureMagFilter, ref magF);
            GL.TextureParameterI(Id, TextureParameterName.TextureMinFilter, ref minF);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
                1,
                1,
                0,
                PixelFormat.Rgba,
                PixelType.Float,
                new[] { color.R, color.G, color.B, color.A }
            );
        }

        public static Texture2D? Create(string path,
            TextureWrapMode wrapModeS = TextureWrapMode.Repeat,
            TextureWrapMode wrapModeT = TextureWrapMode.Repeat,
            TextureMagFilter magFilter = TextureMagFilter.Linear,
            TextureMinFilter minFilter = TextureMinFilter.Nearest,
            bool isMinmap = false
        )
        {
            Texture2D? texture = null;
            try
            {
                texture = new Texture2D(path, wrapModeS, wrapModeT, magFilter, minFilter, isMinmap);
            }
            catch (Exception e)
            {
                RaLog.ErrorLogCore(e.Message);
            }
            return texture;
        }

        public static Texture2D Create(Color4 color) => new Texture2D(color);

        public void Bind(int slot = 0)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + slot);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private ImageResult? LoadTexture2DFromDisk(string path)
        {
            // 对图片进行上下翻转
            StbImage.stbi_set_flip_vertically_on_load(1);
            // 加载图片
            return ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
        }

        private void ReleaseUnmanagedResources()
        {
            GL.DeleteTexture(Id);
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

        ~Texture2D()
        {
            ReleaseUnmanagedResources();
        }
    }
}
