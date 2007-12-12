using System;
using RC.Engine;
using RC.Gui.Fonts;
using Microsoft.Xna.Framework.Graphics;

namespace RC.Gui
{
    public class RCGuiGame : RCBasicGame
    {
        private IFontManager fontManager;

        public RCGuiGame()
            : base()
        {
            fontManager = new FontManager(this);
        }

        protected override void LoadContent()
        {
            IGraphicsDeviceService gds = Services.GetService(
                typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;

            fontManager.LoadFonts(
                gds.GraphicsDevice,
                ContentManager
            );

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            fontManager.UnloadFonts();
            base.UnloadContent();
        }
    }
}
