using System;
using RagadesCubeWin.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.States
{
    public class FadeState : RCGameState
    {
        private Viewport _gameScreen;
        private Texture2D _fadeTexture;
        private float _fadeAmount;
        private double _fadeStartTime;
        private Color _fadeColor;
        private RCGameState _targetState;
        private SpriteBatch _spriteBatch;

        public FadeState(Game game, RCGameState targetState)
            : base(game)
        {
            _targetState = targetState;
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                // Create a viewport that is the size of the game window.
                _gameScreen = new Viewport();

                _gameScreen.X = 0;
                _gameScreen.Y = 0;
                _gameScreen.Width = Game.Window.ClientBounds.Width;
                _gameScreen.Height = Game.Window.ClientBounds.Height;

                _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                _fadeTexture = new Texture2D(
                    graphics.GraphicsDevice,
                    _gameScreen.Width,
                    _gameScreen.Height,
                    1,
                    ResourceUsage.None,
                    SurfaceFormat.Color,
                    ResourceManagementMode.Automatic
                );

                int pixelCount = _gameScreen.Width * _gameScreen.Height;
                Color[] pixelData = new Color[pixelCount];

                for (int i = 0; i < pixelCount; ++i)
                    pixelData[i] = Color.White;

                _fadeTexture.SetData<Color>(pixelData);
            }

            base.LoadGraphicsContent(loadAllContent);
        }

        public override void Update(GameTime gameTime)
        {
            if (_fadeStartTime == 0)
                _fadeStartTime = gameTime.TotalGameTime.TotalMilliseconds;

            // TODO: Make the fade duration adjustable.
            _fadeAmount += (0.10f * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (gameTime.TotalGameTime.TotalMilliseconds > _fadeStartTime + 1000)
            {
                gameManager.PopState();
                gameManager.PushState(_targetState);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            graphics.GraphicsDevice.Viewport = _gameScreen;

            Vector4 fadeColor = _fadeColor.ToVector4();
            fadeColor.W = _fadeAmount;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_fadeTexture, Vector2.Zero, new Color(fadeColor));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
