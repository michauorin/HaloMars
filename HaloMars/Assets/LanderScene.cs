using OpenTK;
using System.Collections.Generic;

namespace HaloMars.Assets
{
    /// <summary>Example of invoking an entire scene (loading it's assets) into render context.</summary>
    public static class LanderScene
    {
        /// <summary>Loads the lander scene - the default demo scene for PID control of a free falling rocket.</summary>
        /// <param name="SceneContents">The scene contents.</param>
        public static void LoadLanderScene(List<GameObjectModel> SceneContents)
        {
            SceneContents.Clear();
            Shader shader = new Shader("shader.vert", "shader.frag");

            TexturedVertex[] texturedVertices =
            {
                new TexturedVertex(new Vector4(-0.25f, -0.25f, 0, 1-0f), new Vector2(0,180)),
                new TexturedVertex(new Vector4( 0.25f, -0.25f, 0, 1-0f), new Vector2(320,180)),
                new TexturedVertex(new Vector4( 0.25f,  0.25f, 0, 1-0f), new Vector2(320,0)),
                new TexturedVertex(new Vector4( 0.25f,  0.25f, 0, 1-0f), new Vector2(320,0)),
                new TexturedVertex(new Vector4(-0.25f,  0.25f, 0, 1-0f), new Vector2(0,0)),
                new TexturedVertex(new Vector4(-0.25f, -0.25f, 0, 1-0f), new Vector2(0,180))
            };

            SceneContents.Add(
                    new Lander(
                              new TexturedAsset(texturedVertices, shader, @"..\..\Textures\Lander.png"),
                              new Vector4(0f, 0.9f, 0.1f, 1f)
                              ));
        }
    }
}