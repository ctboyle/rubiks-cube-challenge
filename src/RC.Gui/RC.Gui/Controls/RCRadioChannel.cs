using System;
using System.Collections.Generic;
using System.Text;
using RC.Gui.Panes;

namespace RC.Gui
{
    
    /// <summary>
    /// Note that while RCRadioChannel deals heavily with controls,
    /// it has no graphical elements so it does not inherit from RCControl.
    /// Its members, which are of type RCRadioButton, do, however.
    /// </summary>
    class RCRadioChannel : RCPane
    {
        [needsXML]
        private RCRadioButton markedMember = null;

        
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
            get { return _listChildren.Count; }
        }

        #endregion

        #region    ------------------------------Private Helper Functions

        /// <summary>
        /// Returns true if <paramref name="buttonToCheck"/> is found in 
        /// the RCRadioChannel's list of children, and false if it is not.
        /// </summary>
        /// <param name="buttonToCheck">The button whose membership we are checking.</param>
        /// <returns>True if <paramref name="buttonToCheck"/> is found in 
        ///          the RCRadioChannel's list of children, and false if it is not.</returns>
        public bool ButtonIsInChannel(RCRadioButton buttonToCheck)
        {
            return _listChildren.Contains(buttonToCheck);
        }

        #endregion ------------------------------Private Helper Functions

        #region ------------------------------Public manipulations of the RCRadioButton children

        /// <summary>
        /// Adds <paramref name="newButton"/> to the RCRadioChannel.
        /// </summary>
        /// <param name="newButton">The RCRadioButton to add to the RCRadioChannel.</param>
        /// <param name="screenCoordX">The X-coordinates of the button, from the upper-left corner of the pane.</param>
        /// <param name="screenCoordY">The Y-coordinates of the button, from the upper-left corner of the pane.</param>
        /// <param name="ZOrder">The Z-order of the button amonogst the other buttons on the radio channel.</param>
        /// <returns>Returns true if the button was added successfully, and false if it was not.</returns>
        public bool AddRadioButton(RCRadioButton newButton,int screenCoordX, int screenCoordY, float ZOrder)
        {
            if (newButton == null)
            {
                return false;
            }
            base.AddChild(newButton, screenCoordX, screenCoordY, ZOrder);
            return true;
        }

        /// <summary>
        /// Makes <paramref name="buttonToMakeActive"/> the active radio button in the channel.
        /// </summary>
        /// <param name="buttonToMakeActive">The button to make active.</param>
        /// <returns>True if the button was made active successfully, and false if it was not.</returns>
        public bool MarkButton(RCRadioButton buttonToMakeActive)
        {
            if (buttonToMakeActive == null)
            { return false; }
            if (markedMember != null)
            { markedMember.Unmark(); }
            
            markedMember = buttonToMakeActive;
            markedMember.Mark();
            return true;
        }

        /// <summary>
        /// Marks all buttons as inactive.
        /// </summary>
        public void UnmarkButton()
        {
            markedMember.Unmark();
            markedMember = null;
        }

        #endregion ------------------------------Public manipulations of the RCRadioButton children



        [placeHolder]
        /// <summary>
        /// Returns the active RCRadioButton amongst this RCRadioChannel
        /// </summary>
        /// <returns>The RCRadioButton that is marked.</returns>
        [incomplete("If the method of storing the active button changes.")]
        public RCRadioButton getActiveButton()
        {
            return markedMember;
        }

        #region    ------------------------------Overridden Functions

        public override void AddChild(RCFlatSpatial newChild, int screenCoordX, int screenCoordY, float zOrder)
        {
            if(newChild is RCRadioButton)
            base.AddChild(newChild, screenCoordX, screenCoordY, zOrder);
        }

        #endregion ------------------------------
    }
}
