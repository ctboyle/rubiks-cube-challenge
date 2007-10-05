using System;
using System.Collections.Generic;
using System.Text;

namespace Ragades_Cube_2D.Classes.Controls.Control_Subclasses
{
    class RCRadioButton : RCControl
    {
        private RCRadioChannel channel;

        public RCRadioButton(RCRadioChannel startingChannel)
        {
            channel = startingChannel;
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
