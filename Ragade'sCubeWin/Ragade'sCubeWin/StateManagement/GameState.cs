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
using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Input;

namespace RagadesCubeWin.StateManagement
{
    public abstract partial class RCGameState : DrawableGameComponent
    {
        protected IGameStateManager gameManager;
        protected InputManager input;
        protected Rectangle TitleSafeArea;
        protected ContentManager content;
        protected IGraphicsDeviceService graphics;
        protected RCSceneManager _sceneManager;
        protected Cue music;

        public RCGameState(Game game)
            : base(game)
        {
            content = new ContentManager(Game.Services);
            gameManager = (IGameStateManager)game.Services.GetService(typeof(IGameStateManager));
            graphics = (IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService));
            _sceneManager = new RCSceneManager(graphics, content);
            input = new InputManager(game);
            input.Initialize();
            
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
          
            _sceneManager.Load(
                    content
                    );

            base.LoadGraphicsContent(loadAllContent);
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {


            _sceneManager.Unload();
            base.UnloadGraphicsContent(unloadAllContent);
        }

        public override void Draw(GameTime gameTime)
        {
            _sceneManager.Draw();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            _sceneManager.Update(gameTime);
            base.Update(gameTime);
        }

        internal protected virtual void StateChanged(
            RCGameState newState,
            RCGameState oldState
            )
        {
            if (newState == this)
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
