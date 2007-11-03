using System;
using RagadesCubeWin.StateManagement;
using Microsoft.Xna.Framework;
using RagadesCubeWin.GameLogic;
using RagadesCubeWin.GUI.Primitives;
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.GUI;
using Microsoft.Xna.Framework.Graphics;

namespace RagadesCubeWin.States
{
    class RCGameCompleteState : RCGameState
    {
        private RCText _outText;
        private double _time;
        private IRCGamePlayerViewer[] _winners;

        public RCGameCompleteState(Game game, IRCGamePlayerViewer[] winners)
            : base(game)
        {
            _winners = winners;

            IFontManager fontManager = (IFontManager)Game.Services.GetService(typeof(IFontManager));
            BitmapFont mediumFont = fontManager.GetFont("Ragade's Cube Large");

            _outText = new RCText(
               mediumFont,
               graphics.GraphicsDevice.Viewport.Width,
               graphics.GraphicsDevice.Viewport.Height / 2,
               graphics.GraphicsDevice.Viewport.Width,
               graphics.GraphicsDevice.Viewport.Height / 2
            );

            RCScreenScene scene = new RCScreenScene(graphics.GraphicsDevice.Viewport);

            scene.Camera.ClearScreen = true;
            scene.Camera.ClearColor = Color.Black;
            scene.Camera.ClearOptions = ClearOptions.DepthBuffer | ClearOptions.Target;

            scene.ScreenPane.AddChild(
                _outText,
                graphics.GraphicsDevice.Viewport.Width / 2,
                graphics.GraphicsDevice.Viewport.Height / 2,
                1.0f
            );

            _sceneManager.AddScene(scene);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
