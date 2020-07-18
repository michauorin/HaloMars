using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace HaloMars.Assets
{
    /// <summary>
    /// Implements the base asset concept for textured models.
    /// </summary>
    /// <seealso cref="HaloMars.Asset" />
    public class TexturedAsset : Asset
    {
        private readonly int Texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TexturedAsset"/> class.
        /// </summary>
        /// <param name="vertices">The vertices including texture hook locations.</param>
        /// <param name="shader">The shader to be used for model rendering, has to include texel sampling!</param>
        /// <param name="filename">The filename of texture used for the model.</param>
        public TexturedAsset(TexturedVertex[] vertices, Shader shader, string filename) : base(shader, vertices.Length)
        {
            // GPU memory is reserved and filled with provided array of vertices.
            GL.NamedBufferStorage(
               VertexBufferObject,
               TexturedVertex.Size * vertices.Length,
               vertices,
               BufferStorageFlags.MapWriteBit);

            // Vertex attributes as defined in vertex class are bound to allow shaders to use them.
            // The first one is Vec4 of vertex position.
            GL.VertexArrayAttribBinding(VertexArrayObject, 0, 0);
            GL.EnableVertexArrayAttrib(VertexArrayObject, 0);
            GL.VertexArrayAttribFormat(
                VertexArrayObject,
                0,
                4,
                VertexAttribType.Float,
                false,
                0);

            // The second one is texture hook position.
            GL.VertexArrayAttribBinding(VertexArrayObject, 1, 0);
            GL.EnableVertexArrayAttrib(VertexArrayObject, 1);
            GL.VertexArrayAttribFormat(
                VertexArrayObject,
                1,
                2,
                VertexAttribType.Float,
                false,
                16);

            // Vertex array containing attributes is bound to the buffer of vertex data
            GL.VertexArrayVertexBuffer(VertexArrayObject, 0, VertexBufferObject, IntPtr.Zero, TexturedVertex.Size);

            // Invokes texture initialization into GPU memory.
            Texture = InitTextures(filename);
        }

        /// <summary>
        /// Initializes the texture into texture buffer.
        /// </summary>
        /// <param name="filename">The filename of loaded texture.</param>
        /// <returns></returns>
        private int InitTextures(string filename)
        {
            // Loads texture into a temporary variable.
            var data = LoadTexture(filename, out int width, out int height);
            // Reserves the slot in GPU texture buffer.
            GL.CreateTextures(TextureTarget.Texture2D, 1, out int texture);
            // Inserts the texture definition into GPU memory with specified formatting.
            GL.TextureStorage2D(
                texture,
                1,
                SizedInternalFormat.Rgba32f,
                width,
                height);

            // Binds texture address for subsequent operations .
            GL.BindTexture(TextureTarget.Texture2D, texture);
            // Fills texture bitmap data reading supplied bitmap data using supplied format hints.
            GL.TextureSubImage2D(texture,
                0,
                0,
                0,
                width,
                height,
                PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Item1.Scan0);
            // Texture handler is returned.
            data.Item2.Dispose();
            return texture;
        }

        /// <summary>
        /// Loads the texture bitmap from specified file.
        /// </summary>
        /// <param name="filename">The filename to read data from.</param>
        /// <param name="width">The width of input image.</param>
        /// <param name="height">The height of input image.</param>
        /// <returns></returns>
        private static Tuple<BitmapData, Bitmap> LoadTexture(string filename, out int width, out int height)
        {
            Bitmap source = new Bitmap(filename);
            width = source.Width;
            height = source.Height;
            BitmapData data = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            source.UnlockBits(data);
            return Tuple.Create(data, source);
        }

        /// <summary>
        /// Binds this instance in openGL context within which it's called.
        /// </summary>
        public override void Bind()
        {
            base.Bind();
            GL.BindTexture(TextureTarget.Texture2D, Texture);
        }

        public override void Render()
        {
            base.Render();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteTexture(Texture);
            }
            base.Dispose(disposing);
        }
    }
}