using Dear_ImGui_Sample;
using HaloMars.Assets;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace HaloMars
{
    public class Window : GameWindow
    {
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
        }

        /// <summary>The current scene contents
        /// to iterate over with game render and update loops.</summary>
        private List<GameObjectModel> CurrentSceneContents = new List<GameObjectModel>();

        /// <summary>The GUI contents instance.</summary>
        private ImGuiController guistuff;

        /// <summary>Called after an OpenGL context has been established, but before entering the main loop.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            Title += GL.GetString(StringName.Version);
            GL.ClearColor(0.1f, 0.1f, 0.3f, 1.0f);
            // Instead of providing a menu the game currently skips straight to the default demo scene loop.
            LanderScene.LoadLanderScene(CurrentSceneContents);
            // Sets the render parameters, first each vertex is defined as solid with both front and back facet filled.
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            // The Depth dimension is enabled, as well as transparency with the default transparency function being alpha channel of the fragment.
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            // Finally the GUI context is initiallized.
            guistuff = new ImGuiController(Width, Height);
            base.OnLoad(e);
        }

        /// <summary>Called when the frame is updated.</summary>
        /// <param name="e">Contains information necessary for frame updating.</param>
        /// <remarks>Subscribe to the <see cref="E:OpenTK.GameWindow.UpdateFrame"/> event instead of overriding this method.</remarks>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // GUI context refreshes it's content first. If any ImGui method is called before a new GUI frame begins
            // a memory exception will be thrown!
            guistuff.Update(this, (float)e.Time);
            // The GUI panel to draw elements on is created.
            // This ultimately should not be called every update. But it's not crucial, I can still spare CPU cycles.
            ImGui.Begin("Interfejs kontrolny");
            ImGui.SetWindowPos("Interfejs kontrolny", new System.Numerics.Vector2(0.0f, 0.0f));
            ImGui.SetWindowSize("Interfejs kontrolny", new System.Numerics.Vector2(400f, 600f));
            // Each object in the currently rendered scene performs it's game loop.
            foreach (var model in CurrentSceneContents)
            {
                model.Update(e);
            }

            base.OnUpdateFrame(e);
        }

        /// <summary>Called when the frame is rendered.</summary>
        /// <param name="e">Contains information necessary for frame rendering.</param>
        /// <remarks>Subscribe to the <see cref="E:OpenTK.GameWindow.RenderFrame"/> event instead of overriding this method.</remarks>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // The previous frame contents are completely cleared
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // The Z dimension and opacity parameters are reset (gui render sets it's own set of flags but doesn't reset the prev values so it's necessary)
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            // Each object in the currently rendered scene performs it's render loop.
            foreach (var model in CurrentSceneContents)
            {
                model.Render();
            }
            guistuff.Render();
            // The rendered frame is marked as current framebuffer once it's done being drawn, the previous frame becomes the next one to draw on during next iteration.
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        /// <summary>Called when this window is resized.</summary>
        /// <param name="e">Not used.</param>
        /// <remarks>
        /// You will typically wish to update your viewport whenever
        /// the window is resized. See the
        /// <see cref="M:OpenTK.Graphics.OpenGL.GL.Viewport(System.Int32,System.Int32,System.Int32,System.Int32)"/> method.
        /// </remarks>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            base.OnResize(e);
        }

        /// <summary>Called after GameWindow.Exit was called, but before destroying the OpenGL context.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnUnload(EventArgs e)
        {
            GL.UseProgram(0);

            base.OnUnload(e);
        }
    }
}