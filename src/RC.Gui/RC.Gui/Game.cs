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

        // Loading and unloading for games services and singletons.
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            IGraphicsDeviceService gds = Services.GetService(
                typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;

            fontManager.LoadFonts(
                gds.GraphicsDevice,
                ContentManager
                );
          
            base.LoadGraphicsContent(loadAllContent);
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            fontManager.UnloadFonts();
            base.UnloadGraphicsContent(unloadAllContent);
        }
    }
}
