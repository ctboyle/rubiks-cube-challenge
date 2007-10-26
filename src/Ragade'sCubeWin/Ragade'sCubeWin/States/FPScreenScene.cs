using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.StateManagement;
using RagadesCubeWin.GUI;
using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.GUI.Controls.Control_Subclasses;
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.GUI.Primitives;

namespace RagadesCubeWin.States
{
    class FPScreenScene : RCScreenScene
    {
        float fps;
        float updateInterval = 1.0f;
        float timeSinceLastUpdate = 0.0f;
        float framecount = 0;
        RCText testStateControl;

        public FPScreenScene(Viewport vp, IServiceProvider svcProvider)
            : base(vp)
        {
            // Get fonts.
            IFontManager fontManager = (IFontManager)svcProvider.GetService(typeof(IFontManager));
            BitmapFont LucidaFont = fontManager.GetFont("Lucida Console");
            testStateControl = new RCText(LucidaFont, 10, 10, 10, 10);
            ScreenPane.AddChild(testStateControl, 0, 0, 0);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
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

            testStateControl.Text = string.Format("FPS:{0,9} RT:{1,11:f} GT:{2,11:f}",
                fps, gameTime.ElapsedRealTime.TotalSeconds, gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }
    }
}