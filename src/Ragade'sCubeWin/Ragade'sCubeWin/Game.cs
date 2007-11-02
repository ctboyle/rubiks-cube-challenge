#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using RagadesCubeWin.StateManagement;
using RagadesCubeWin.States;
using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.SceneObjects;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;

using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.States.TitleScreen;

#endregion

namespace RagadesCubeWin
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RagadesCube : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;
        RCGameStateManager stateManager;
        FontManager fontManager;

        public RagadesCube()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            stateManager = new RCGameStateManager(this);
            fontManager = new FontManager(this);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //FPS fps = new FPS(this);
            //fps.DrawOrder = int.MaxValue;
            //Components.Add(fps);
         
            IsMouseVisible = true;
            IsFixedTimeStep = false;

            // Add a font to the font manager
            fontManager.AddFontFromAsset(
                "Lucida Console",
                "Content\\Fonts\\Lucida Console\\font.xml"
                );

            fontManager.AddFontFromAsset(
                "Rockwell Extra Bold",
                "Content\\Fonts\\Rockwell Extra Bold -48pt\\font.xml"
                );

            fontManager.AddFontFromAsset(
                "Ragade's Cube Small",
                "Content\\Fonts\\Ragade's Cube\\26\\RCfont.xml"
                );

            fontManager.AddFontFromAsset(
                "Ragade's Cube Medium",
                "Content\\Fonts\\Ragade's Cube\\36\\RCfont.xml"
                );

            fontManager.AddFontFromAsset(
                "Ragade's Cube Large",
                "Content\\Fonts\\Ragade's Cube\\48\\RCfont.xml"
                );

            fontManager.AddFontFromAsset(
               "Ragade's Cube Extra Large",
               "Content\\Fonts\\Ragade's Cube\\72\\RCfont.xml"
               );


            // Begin by putting our first state on the stack.
            stateManager.PushState(new States.TitleScreen.RCTitleScreenState(this));
            

            base.Initialize();
        }

        // Loading and unloading for games services and singletons.
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            // Initialize the rendermanager
            RCRenderManager.Load(graphics.GraphicsDevice);
            
            fontManager.LoadFonts(
                graphics.GraphicsDevice,
                content
                );

            base.LoadGraphicsContent(loadAllContent);
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            RCRenderManager.Unload();

            fontManager.UnloadFonts();

            content.Unload();
            base.UnloadGraphicsContent(unloadAllContent);
           
        }

    }
}
