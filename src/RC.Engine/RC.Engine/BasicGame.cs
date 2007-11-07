using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RC.Engine.StateManagement;
using RC.Engine.Rendering;

namespace RC.Engine
{
    public class RCBasicGame : Game
    {
        private GraphicsDeviceManager graphics;
        private IGameStateManager stateManager;
        private ContentManager content;

        public RCBasicGame()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            stateManager = new RCGameStateManager(this);           
        }

        protected ContentManager ContentManager
        {
            get { return content; }
        }

        // Loading and unloading for games services and singletons.
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            RCRenderManager.Load(graphics.GraphicsDevice);
            base.LoadGraphicsContent(loadAllContent);
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            RCRenderManager.Unload();
            content.Unload();
            base.UnloadGraphicsContent(unloadAllContent);
        }
    }
}
