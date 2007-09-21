#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using RagadesCubeWin.StateManagement;
using RagadesCubeWin.States;
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;
#endregion

namespace RagadesCubeWin
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RagadesCube : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;
        RCGameStateManager stateManager;

        public RagadesCube()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            stateManager = new RCGameStateManager(this);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize the rendermanager
            RCRenderManager.Initialize(graphics.GraphicsDevice);

            // Add frame per second counter.
            // Components.Add(new FPS(this));

            // Begin by putting our first state on the stack.
            stateManager.PushState(new RCTestState(this));

            base.Initialize();
        }

    }
}
