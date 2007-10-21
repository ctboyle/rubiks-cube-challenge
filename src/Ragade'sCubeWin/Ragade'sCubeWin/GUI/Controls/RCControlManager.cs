using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GUI.Panes;


namespace RagadesCubeWin.GUI
{
    
    class RCControlManager
    {
        private IDictionary<long, RCControl> allControls = new Dictionary<long, RCControl>();
        private IDictionary<long, string> allControlNames = new Dictionary<long, string>();
        private IDictionary<long, RCPane> allPanes = new Dictionary<long, RCPane>();
        private long controlCount = 0;
        private long paneCount = 0;



        /// <summary>
        /// Property count returns the number of controls that 
        /// the RCControlManager is presently managing.
        /// </summary>
        [shouldBeDone]
        public long count
        {
            get { return controlCount; }
        }

        [needsXML]
        [placeHolder]
        public RCControlManager()
        {
            return;
        }

        /*

        /// <summary>
        /// Attempts to add a RCControl named <paramref name="controlName"/>
        /// and returns that control.  Returns a null RCControl if there 
        /// is already an RCControl with that name.
        /// </summary>
        /// <param name="controlName">The codeName to give to the new control.</param>
        /// <returns>The new RCControl, or a null RCControl if the 
        ///          name <paramref name="controlName"/> was already in use.</returns>
        [mayNeedExpansion("In the event that implementation for RCControl creation changes")]
        public RCControl addControl(string controlName)
        {
            #region Check to ensure that the name for the new RCControl is not already used; return null if it is.
            for (long controlIDToCheckNameWith = 0; controlIDToCheckNameWith < controlCount; controlIDToCheckNameWith++)
            {
                if (controlName == allControlNames[controlIDToCheckNameWith])
                { return null; }
            }
            #endregion Check to ensure that the name for the new RCControl is not already used; return null if it is.

            #region Create new control.
            RCControl newControl = new RCControl(controlName);
            allControls.Add(count, newControl);
            allControlNames.Add(count, controlName);
            controlCount++;
            #endregion Create new control.
        }

        */

        [needsXML]
        [placeHolder]
        public RCControl addControl(string controlName, RCControlType controlType)
        {
            return null;
        }

        [needsXML]
        [placeHolder]
        public RCControl addControl(RCControlType controlType)
        {
            return null;
        }

        [needsXML]
        [placeHolder]
        public RCControl addControl()
        {
            return null;
        }
    }
}