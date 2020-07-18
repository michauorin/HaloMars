using OpenTK;
using OpenTK.Graphics;

namespace HaloMars
{
    /// <summary>Template struct of a solid line vertex [with or without fill, decided by the rendering context].</summary>
    public struct SolidVertex
    {
        /// <summary>
        /// The size of a single vertex entry in bytes.
        /// A solid color vertex contains 2 arrays of single precision float numbers, each with 4 fields.
        /// </summary>
        public const int Size = (4 + 4) * 4;

        /// <summary>The vertex position in normalized space. 4 floats, uses built in OpenTK data stucture.</summary>
        private readonly Vector4 _position;

        /// <summary>The vertex body color. 4 floats, utilizing built in OpenTK data structure.</summary>
        private readonly Color4 _color;

        /// <summary>Initializes a new instance of the <see cref="SolidVertex"/> struct.</summary>
        /// <param name="positon">The positon.</param>
        /// <param name="color">The color.</param>
        public SolidVertex(Vector4 positon, Color4 color)
        {
            _position = positon;
            _color = color;
        }
    }

    /// <summary>Template struct of a textured vertex.</summary>
    public struct TexturedVertex
    {
        /// <summary>
        /// The size of a single vertex entry in bytes.
        /// A textured vertex contains 2 arrays of single precision float numbers, one with 4 fields, one with 2 fields.
        /// </summary>
        public const int Size = (4 + 2) * 4;

        /// <summary>The vertex position in normalized space. 4 floats, uses built in OpenTK data structure.</summary>
        private readonly Vector4 _position;

        /// <summary>The texture sampling coordinates. 2 floats, uses built in OpenTK data structure.</summary>
        private readonly Vector2 _textureCoordinates;

        /// <summary>Initializes a new instance of the <see cref="TexturedVertex"/> struct.</summary>
        /// <param name="position">The position.</param>
        /// <param name="textureCoordinates">The texture sampling coordinates.</param>
        public TexturedVertex(Vector4 position, Vector2 textureCoordinates)
        {
            _position = position;
            _textureCoordinates = textureCoordinates;
        }
    }
}