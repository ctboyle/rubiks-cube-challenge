using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace RagadesCubeWin
{
    public class FPS : DrawableGameComponent
    {
        ContentManager contentMgr;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        float fps;
        float updateInterval = 1.0f;
        float timeSinceLastUpdate = 0.0f;
        float framecount = 0;

        public FPS(Game game)
            : base(game)
        {
            contentMgr = new ContentManager(Game.Services);
        }

        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                spriteFont = contentMgr.Load<SpriteFont>("Content\\Fonts\\Courier New");
                spriteBatch = new SpriteBatch(GraphicsDevice);
            }

            base.LoadGraphicsContent(loadAllContent);
        }

        public override void Draw(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;
            framecount++;
            timeSinceLastUpdate += elapsed;

            if (timeSinceLastUpdate > updateInterval)
            {
                fps = framecount / timeSinceLastUpdate;
                framecount = 0;
                timeSinceLastUpdate -= updateInterval;
            }

            string output = string.Format("FPS:{0,9}\nRT:{1,11:f}\nGT:{2,11:f}",
                fps, gameTime.ElapsedRealTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds);
            Rectangle rect = Game.Window.ClientBounds;
            Vector2 position = new Vector2(rect.Width - 10, rect.Height - 10) - spriteFont.MeasureString(output);

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, output, position, Color.Black);
            spriteBatch.End();
        }
    }
}
