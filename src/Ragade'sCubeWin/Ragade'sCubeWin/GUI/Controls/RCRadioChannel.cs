using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GUI.Panes;

namespace RagadesCubeWin.GUI
{
    
    /// <summary>
    /// Note that while RCRadioChannel deals heavily with controls,
    /// it has no graphical elements so it does not inherit from RCControl.
    /// Its members, which are of type RCRadioButton, do, however.
    /// </summary>
    class RCRadioChannel : RCPane
    {
        [needsXML]
        private long numberOfMembers = 0;
        [needsXML]
        private RCRadioButton activeMember = null;

        
        [needsXML]
        internal RCRadioChannel(
            float width, 
            float height,
            int screenWidth, 
            int screenHeight,
            RCRadioButton firstButton
            ) : base (
                width,
                height,
                screenWidth,
                screenHeight
            )
        {
        }

        #region Public read-only properties

        /// <summary>
        /// Returns the number of RCRadioButtons that are in the RCRadioChannel.
        /// </summary>
        [doneForNow]
        public long count
        { 
            get { return numberOfMembers; }
        }

        #endregion



        [placeHolder]
        /// <summary>
        /// Returns the active RCRadioButton amongst this RCRadioChannel
        /// </summary>
        /// <returns>The RCRadioButton that is marked.</returns>
        [incomplete("If the method of storing the active button changes.")]
        public RCRadioButton getActiveButton()
        {
            return null;
        }
    }
}
