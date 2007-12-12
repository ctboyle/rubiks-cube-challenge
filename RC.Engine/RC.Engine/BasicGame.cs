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

        protected override void LoadContent()
        {
            RCRenderManager.Load(graphics.GraphicsDevice);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            RCRenderManager.Unload();
            content.Unload();
            base.UnloadContent();
        }
    }
}
