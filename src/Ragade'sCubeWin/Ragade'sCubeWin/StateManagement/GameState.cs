using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using RagadesCubeWin.StateManagement;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;

namespace RagadesCubeWin.StateManagement
{
    public abstract partial class RCGameState : DrawableGameComponent
    {
        protected IGameStateManager GameManager;
        protected Input.InputManager input;
        protected Rectangle TitleSafeArea;
        protected ContentManager content;
        protected IGraphicsDeviceService graphics;


        public RCGameState(Game game)
            : base(game)
        {
            content = new ContentManager(Game.Services);
            GameManager = (IGameStateManager)game.Services.GetService(typeof(IGameStateManager));
            graphics = (IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService));
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                //TitleSafeArea = Utility.GetTitleSafeArea(GraphicsDevice, 0.85f);
            }

            base.LoadGraphicsContent(loadAllContent);
        }

        internal protected virtual void StateChanged(object sender, EventArgs e)
        {
            if (GameManager.State == this.Value)
            {
                Visible = Enabled = true;
            }
            else
            {
                Visible = Enabled = false;
            }
        }

        public RCGameState Value
        {
            get { return (this); }
        }
    }
}
