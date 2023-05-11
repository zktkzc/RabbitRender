using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Rabbit_core.Rendering
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

        public Texture2D(
            string path,
            TextureWrapMode wrapModeS = TextureWrapMode.Repeat,
            TextureWrapMode wrapModeT = TextureWrapMode.Repeat,
            TextureMagFilter magFilter = TextureMagFilter.Linear,
            TextureMinFilter minFilter = TextureMinFilter.Nearest,
            bool isMinmap = false
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

        public Texture2D(
            Color4 color,
            TextureWrapMode wrapModeS = TextureWrapMode.Repeat,
            TextureWrapMode wrapModeT = TextureWrapMode.Repeat,
            TextureMagFilter magFilter = TextureMagFilter.Linear,
            TextureMinFilter minFilter = TextureMinFilter.Nearest
            )
        {
            Id = GL.GenTexture();
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
                1,
                1,
                0,
                PixelFormat.Rgba,
                PixelType.Float,
                new[] { color.R, color.G, color.B, color.A }
            );
        }

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
            try
            {
                // 对图片进行上下翻转
                StbImage.stbi_set_flip_vertically_on_load(1);
                // 加载图片
                return ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private void ReleaseUnmanagedResources()
        {
            GL.DeleteTexture(Id);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Texture2D()
        {
            ReleaseUnmanagedResources();
        }
    }
}
