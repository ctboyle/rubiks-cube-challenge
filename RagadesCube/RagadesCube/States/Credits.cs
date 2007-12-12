using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RagadesCube.States;
using RagadesCube.Controllers;
using RC.Gui.Primitives;
using RC.Gui.Fonts;
using Microsoft.Xna.Framework.Graphics;


namespace RagadesCube.States
{
     class RCOptions : RCCubeMenu
    {
        public RCOptions(Game game)
            : base(game)
        {
            _menuPos = RCMenuCameraController.CameraPositions.Top;
        }

        protected override void ConstructGuiElements()
        {
            _titleText.Text = "Credits";
            

            BitmapFont smallFont = _fontManager.GetFont("Ragade's Cube Small");
            smallFont.KernEnable = false;

            // Prompt
            RCText promptText = new RCText(smallFont, 1, 1, 600, 75);
            promptText.Text = "Design, Dev, and Testing by";
            promptText.Color = Color.Red;
            promptText.CenterText = true;
            _menuPane.AddChild(promptText, 0, 150, 0.1f);

            RCText ChrisLText = new RCText(smallFont, 1, 1, 600, 75);
            ChrisLText.Text = "Chris Boyle";
            ChrisLText.Color = Color.Red;
            ChrisLText.CenterText = true;
            _menuPane.AddChild(ChrisLText, 0, 410, 0.1f);

            RCText ChrisBText = new RCText(smallFont, 1, 1, 600, 75);
            ChrisBText.Text = "Chris Lockhart";
            ChrisBText.Color = Color.Red;
            ChrisBText.CenterText = true;
            _menuPane.AddChild(ChrisBText, 0, 450, 0.1f);

            RCText StevenText = new RCText(smallFont, 1, 1, 600, 75);
            StevenText.Text = "Steven Shofner";
            StevenText.Color = Color.Red;
            StevenText.CenterText = true;
            _menuPane.AddChild(StevenText, 0, 490, 0.1f);

            RCText JasonText = new RCText(smallFont, 1, 1, 600, 75);
            JasonText.Text = "Jason Spruill";
            JasonText.Color = Color.Red;
            JasonText.CenterText = true;
            _menuPane.AddChild(JasonText, 0, 530, 0.1f);
        }
    }
}

