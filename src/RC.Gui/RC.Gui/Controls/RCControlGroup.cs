using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

#region STRETCH GOAL
namespace RC.Gui
{
    [needsXML]
    class RCControlGroup
    {
        #region private data members
        private RCControl[] _groupMembers = null;
        private IDictionary _controlsByCodeName = new Dictionary<string, RCControl>();
        private IDictionary<long, RCControl> _controlsByID;
        private long groupMemberCount = 0;
        #endregion private data members


        #region public read-only properties
        /// <summary>
        /// This read-only property returns the members of 
        /// the RCControlGroup as an array of RCControls.
        /// </summary>
        [doneForNow]
        public RCControl[] members
        {
            get{ return _groupMembers; }
        }

        /// <summary>
        /// Read-only property that returns the number of members in the RCControlGroup.
        /// </summary>
        [doneForNow]
        public long count
        {
            get { return groupMemberCount; }
        }

        #endregion public read-only properties

        #region public functions and methods

        #region addMember (2 overloads)
        
        [needsXML]
        [placeHolder]
        public RCControl addMember(string name, RCControlType controlType )
        {
            return null;
        }

        [needsXML]
        [placeHolder]
        public RCControl addMember(RCControlType controlType)
        {
            return null;
        }

        #endregion addMember (2 overloads)


        #region deleteMember (3 overloads)

        [needsXML]
        [placeHolder]
        public void deleteMember(RCControl memberToDelete)
        {
        }

        [needsXML]
        [placeHolder]
        public void deleteMember(long controlID)
        {

        }

        [needsXML]
        [placeHolder]
        public void deleteMember(string codeName)
        {
        }

        #endregion deleteMember (3 overloads)

        #endregion public functions and methods

        #region private helper functions


        [needsXML]
        [placeHolder]
        private bool controlIsInGroup(RCControl controlToCheck)
        {
            return false;
        }


        #endregion private helper functions
    }
}
#endregion STRETCH GOAL