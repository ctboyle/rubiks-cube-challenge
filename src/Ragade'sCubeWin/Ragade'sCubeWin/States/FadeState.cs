using System;
using RagadesCubeWin.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.States
{
    public class FadeState : RCGameState
    {
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
                _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                _fadeTexture = new Texture2D(
                    graphics.GraphicsDevice,
                    graphics.GraphicsDevice.Viewport.Width,
                    graphics.GraphicsDevice.Viewport.Height,
                    1,
                    ResourceUsage.None,
                    SurfaceFormat.Color,
                    ResourceManagementMode.Automatic
                );

                int pixelCount = graphics.GraphicsDevice.Viewport.Width * graphics.GraphicsDevice.Viewport.Height;
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

            _fadeAmount += (.25f * (float)gameTime.ElapsedGameTime.TotalSeconds);

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

            Vector4 fadeColor = _fadeColor.ToVector4();
            fadeColor.W = _fadeAmount;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_fadeTexture, Vector2.Zero, new Color(fadeColor));
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
