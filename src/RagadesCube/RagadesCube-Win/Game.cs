#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using RC.Engine.StateManagement;
using RagadesCube.States;
using RC.Engine.GraphicsManagement;
using RagadesCube.SceneObjects;
using RC.Engine.Rendering;
using RC.Engine.Cameras;
using RC.Gui.Fonts;
using RC.Engine.SoundManagement;
using RC.Gui;
#endregion

namespace RagadesCube
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RagadesCube : RCGuiGame
    {
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            IFontManager fontManager = Services.GetService(typeof(IFontManager)) as IFontManager;

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

            SoundManager.Initialize(@"content/sounds/RCSoundBank.xgs",
                                    @"content/sounds/Wave Bank.xwb",
                                    @"content/sounds/Sound Bank.xsb"); 

            base.LoadGraphicsContent(loadAllContent);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            base.Initialize();
        }

        protected override void BeginRun()
        {
            IGameStateManager stateManager = Services.GetService(
                typeof(IGameStateManager)) as IGameStateManager;

            // Begin by putting our first state on the stack.
            //stateManager.PushState(new RCGuiTestState(this));
            stateManager.PushState(new RCTitleScreenState(this));
            SoundManager.PlayCue("musicbeat");
            base.BeginRun();
        }

        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            SoundManager.Stop();
            base.UnloadGraphicsContent(unloadAllContent);
        }
    }
}
