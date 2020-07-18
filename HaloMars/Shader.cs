using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;

namespace HaloMars
{
    /// <summary>Encapsulation of an OpenGL Shader object in GPU memory. Contains both vertex and fragment shaders, already compiled and linked, provides binding and disposal methods.</summary>
    public class Shader
    {
        /// <summary>
        /// Access handler to a shader instance, assigned by OpenGL internally, akin to a pointer to GPU memory at shader entry point.
        /// </summary>
        private readonly int Handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shader"/> class, takes source of the shaders, allocates, compiles, links into GPU memory, returns hanlder for in-context binding.
        /// </summary>
        /// <param name="vertexPath">The vertex shader source file path.</param>
        /// <param name="fragmentPath">The fragment shader source file path.</param>
        public Shader(string vertexPath, string fragmentPath)
        {
            /// Integers to store OpenGL intermediate handlers for both shaders
            int VertexShader;
            int FragmentShader;

            /// Source for both shaders is being read from pointed paths
            string VertexShaderSource;
            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;
            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            /// For both shaders first an empty shader object is created
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            /// The previously read source code is attached
            GL.ShaderSource(VertexShader, VertexShaderSource);
            /// And compiled
            GL.CompileShader(VertexShader);
            /// Compilation error log is collected and sent to stdout
            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (!string.IsNullOrEmpty(infoLogVert))
                System.Console.WriteLine(infoLogVert);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(FragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);
            if (!string.IsNullOrEmpty(infoLogFrag))
                System.Console.WriteLine(infoLogFrag);

            /// A shader program object is linked to the instance handler
            Handle = GL.CreateProgram();
            /// Both compiled shaders are attached
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);
            /// Linked into a single executable procedure on the GPU
            GL.LinkProgram(Handle);
            /// Finally the intermediate objects are deleted
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        /// <summary>
        /// Allows to retrieve the location of a parameter passed to shader program dynamically instead of hardcoding a predefined one
        /// OpenGL can freely optimize the shader input and output data thanks to that
        /// </summary>
        /// <param name="attribName">Name of shader parameter to retrieve the location of</param>
        /// <returns></returns>
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        /// <summary>
        /// Binds the instance of shader within the OpenGL context it was called from.
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        /// <summary>
        /// Internal cleanup flag
        /// </summary>
        private bool disposed = false;

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (!disposed)
            {
                GL.DeleteProgram(Handle);
                disposed = true;
            }
        }

        /// <summary>Finalizes an instance of the <see cref="Shader"/> class.</summary>
        ~Shader()
        {
            //GL.DeleteProgram(Handle);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}