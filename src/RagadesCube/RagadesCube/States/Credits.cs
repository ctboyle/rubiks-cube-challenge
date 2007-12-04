using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCube.States;
using RagadesCube.Controllers;
using RC.Gui.Primitives;
using RC.Gui.Fonts;


namespace RagadesCube.States
{
     class RCOptions : RCCubeMenu
    {

        public RCOptions(Game game)
            : base(game)
        {
            _menuPos = RCMenuCameraController.CameraPositions.Left;
        }


        public override void Initialize()
        {
            base.Initialize();   
        }

        protected override void ConstructGuiElements()
        {
            _titleText.Text = "Credits";

            BitmapFont mediumFont = _fontManager.GetFont("Ragade's Cube Medium");
            BitmapFont smallFont = _fontManager.GetFont("Ragade's Cube Small");

            // Prompt
            RCText promptText = new RCText(smallFont, 1, 1, 600, 75);
            promptText.Text = "Design, Dev, and Testing by";
            promptText.CenterText = true;
            _menuPane.AddChild(promptText, 0, 240, 0.0f);

            RCText ChrisLText = new RCText(smallFont, 1, 1, 600, 75);
            ChrisLText.Text = "Chris Boyle";
            ChrisLText.CenterText = true;
            _menuPane.AddChild(ChrisLText, 0, 300, 0.0f);

            RCText ChrisBText = new RCText(smallFont, 1, 1, 600, 75);
            ChrisBText.Text = "Chris Lockhart";
            ChrisBText.CenterText = true;
            _menuPane.AddChild(ChrisBText, 0, 340, 0.0f);

            RCText StevenText = new RCText(smallFont, 1, 1, 600, 75);
            StevenText.Text = "Steven Shofner";
            StevenText.CenterText = true;
            _menuPane.AddChild(StevenText, 0, 380, 0.0f);

            RCText JasonText = new RCText(smallFont, 1, 1, 600, 75);
            JasonText.Text = "Jason Spruill";
            JasonText.CenterText = true;
            _menuPane.AddChild(JasonText, 0, 420, 0.0f);
        }
    }
}

