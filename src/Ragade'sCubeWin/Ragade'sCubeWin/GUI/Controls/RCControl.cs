using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace Ragades_Cube_2D.Classes.Controls
{
    

    abstract class RCControl
    {

        private String codeName = null;
        private Single xCoord = 0;
        private Single yCoord = 0;
        private Single xLength;
        private Single yLength;
        private Double zOrder = 0;
        private CoordinateMode2D xCoordMode = CoordinateMode2D.absolute;
        private CoordinateMode2D yCoordMode = CoordinateMode2D.absolute;
        private CoordinateMode2D xLengthMode = CoordinateMode2D.absolute;
        private CoordinateMode2D yLengthMode = CoordinateMode2D.absolute;
        
        /// <summary>
        /// Used to group controls, to cause them to move together.
        /// </summary>
        private RCControlGroup attachedTo = null;

        /// <summary>
        /// Parent of all controls, used to maintain unique code-names.
        /// </summary>
        private RCControlManager manager = null;

        #region Attributes that may not ever be implemented
        private String mouseOverText = "";
        #endregion Attributes that may not ever be implemented

        /// <summary>
        /// Returns the X location as an absolute value.
        /// </summary>
        public Single absoluteX
        {
            
            get
            {
                if (xCoordMode == CoordinateMode2D.absolute)
                {  return xCoord;  }
                else
                {  return xCoord;  }
            }
        }
    }
}
