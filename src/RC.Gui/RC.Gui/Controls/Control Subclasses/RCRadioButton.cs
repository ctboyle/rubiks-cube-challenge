using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RC.Gui.Fonts;

namespace RC.Gui
{
 
    [needsXML]
    class RCRadioButton : RCButton
    {
        [needsXML]
        private RCRadioChannel channel;

        [placeHolder]
        [needsXML]
        internal RCRadioButton(
            float width, 
            float height,
            int screenWidth, 
            int screenHeight,
            RCRadioChannel channel,
            BitmapFont Font

        ) : base(
            width,
            height,
            screenWidth,
            screenHeight,
            Font
        )
        {
        }

        #region Public read-only properties.

        /// <summary>
        /// Whether or not this RCRadioButton is the selected one
        /// in its RCRadioChannel.
        /// </summary>
        public bool isMarked
        {
            get
            {
                return (channel.getActiveButton() == this);
            }

        }

        #endregion Public read-only properties.

        [needsXML]
        [placeHolder]
        internal void MoveToGroup(RCRadioChannel RCRadioChannel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        

        [needsXML]
        [placeHolder]
        internal void Mark()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        [needsXML]
        [placeHolder]
        internal void Unmark()
        {
            throw new Exception("The method or operation is not implemented.");
        }


        #region    ------------------------------Overridden Functions

        public override void LoadGraphicsContent(GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            throw new Exception("The method or operation is not implemented.");
            base.LoadGraphicsContent(graphics, content);
        }

        public override bool OnEvent(GUIEvent guiEvent)
        {
            //Idea: run base.OnEvent, then check again and change the BASE model.
            throw new Exception("The method or operation is not implemented.");
            return base.OnEvent(guiEvent);
        }

        #endregion ------------------------------Overridden Functions
    }
}
