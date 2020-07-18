using OpenTK.Graphics.OpenGL;
using System;

namespace HaloMars
{
    /// <summary>Implements the abstract construct of a game asset.</summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class Asset : IDisposable
    {
        /// <summary>
        /// The is initialized flag for garbage collection.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// The assigned shader object.
        /// </summary>
        protected readonly Shader AssignedShader;

        /// <summary>
        /// The vertex array object pointer.
        /// </summary>
        protected readonly int VertexArrayObject;

        /// <summary>
        /// The vertex buffer object pointer.
        /// </summary>
        protected readonly int VertexBufferObject;

        /// <summary>
        /// The vertices count in the built model.
        /// </summary>
        protected readonly int VerticesCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        /// <param name="shader">The shader to be used with the particular model instance.</param>
        /// <param name="verticesCount">The vertices count of the particular model instance.</param>
        protected Asset(Shader shader, int verticesCount)
        {
            AssignedShader = shader;
            this.VerticesCount = verticesCount;
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        }

        /// <summary>
        /// Binds this instance in openGL context within which it's called.
        /// </summary>
        public virtual void Bind()
        {
            AssignedShader.Use();
            GL.BindVertexArray(VertexArrayObject);
        }

        /// <summary>
        /// Makes the draw call in openGL context within which it's called.
        /// Do note, the draw call will operate on any bound buffer!
        /// </summary>
        public virtual void Render()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, VerticesCount);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (isInitialized)
                {
                    GL.DeleteVertexArray(VertexArrayObject);
                    GL.DeleteBuffer(VertexBufferObject);
                    isInitialized = false;
                }
            }
        }
    }
}