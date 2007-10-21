using System;
using System.Collections.Generic;
using System.Text;

namespace RagadesCubeWin.GUI
{
    [placeHolder]
    [needsXML]
    class RCRadioButton : RCControl
    {
        private RCRadioChannel channel;

        [placeHolder]
        [needsXML]
        internal RCRadioButton(
            float width, 
            float height,
            int screenWidth, 
            int screenHeight,
            RCRadioChannel channel

        ) : base(
            width,
            height,
            screenWidth,
            screenHeight
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


        internal void moveToGroup(RCRadioChannel rCRadioChannel)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
