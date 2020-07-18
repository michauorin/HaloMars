using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace HaloMars
{
    /// <summary>
    ///   Template for every renderable object to be used in the game.
    /// </summary>
    public abstract class GameObjectModel
    {
        public Asset RenderModel => _renderModel;
        public Vector4 Position => _position;
        private static int ModelCounter;
        protected readonly int _modelID;
        private protected Asset _renderModel;
        private protected Vector4 _position;
        private protected Vector4 _rotation;
        private protected Matrix4 _modelView;

        /// <summary>Initializes a new instance of the <see cref="GameObjectModel"/> class.</summary>
        /// <param name="model">The model vertices.</param>
        /// <param name="position">The model position in game space.</param>
        public GameObjectModel(Asset model, Vector4 position)
        {
            _renderModel = model;
            _position = position;
            _modelID = ModelCounter++;
        }

        /// <summary>  Model game loop.</summary>
        /// <param name="e">The <see cref="FrameEventArgs"/> instance containing the event data.</param>
        public virtual void Update(FrameEventArgs e)
        {
        }

        /// <summary>  Model render loop.</summary>
        public virtual void Render()
        {
            // Binds the model buffers and shader.
            _renderModel.Bind();
            // Rotation remains unused for now but in order to keep it proper MVP matrix model it's still present.
            // In case it's needed it'll be easy to add.
            var rotation1 = Matrix4.CreateRotationX(_rotation.X);
            var rotation2 = Matrix4.CreateRotationY(_rotation.Y);
            var rotation3 = Matrix4.CreateRotationZ(_rotation.Z);
            // Translation is the movement of model in simple terms.
            var translation = Matrix4.CreateTranslation(_position.X, _position.Y, _position.Z);
            _modelView = rotation1 * rotation2 * rotation3 * translation;
            // ModelView is passed as a uniform parameter to the shaders.
            GL.UniformMatrix4(20, false, ref _modelView);
            _renderModel.Render();
        }
    }
}