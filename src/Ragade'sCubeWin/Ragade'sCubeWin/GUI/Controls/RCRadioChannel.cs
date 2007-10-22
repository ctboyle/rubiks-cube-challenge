using System;
using System.Collections.Generic;
using System.Text;

namespace RagadesCubeWin.GUI
{
    
    /// <summary>
    /// Note that while RCRadioChannel deals heavily with controls,
    /// it has no graphical elements so it does not inherit from RCControl.
    /// Its members, which are of type RCRadioButton, do, however.
    /// </summary>
    class RCRadioChannel
    {
        private IDictionary<long,RCRadioButton> membersByID = new Dictionary<long, RCRadioButton>();
        private IDictionary<string,RCRadioButton> membersByName = new Dictionary<string,RCRadioButton>();
        private long numberOfMembers = 0;
        private long activeMemberIndex = -1;

        [notFullyImplemented("Missing most initalization features")]
        [needsXML]
        public RCRadioChannel(RCRadioButton firstButton)
        {

            firstButton.moveToGroup(this);

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



        /// <summary>
        /// Returns the active RCRadioButton amongst this RCRadioChannel
        /// </summary>
        /// <returns>The RCRadioButton that is marked.</returns>
        [incomplete("If the method of storing the active button changes.")]
        public RCRadioButton getActiveButton()
        {
            if (activeMemberIndex == -1)
            {
                return null;
            }
            else
            {
                return membersByID[activeMemberIndex];
            }
        }
    }
}
