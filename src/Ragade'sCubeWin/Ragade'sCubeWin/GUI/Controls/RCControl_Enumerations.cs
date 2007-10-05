using System;
using System.Collections.Generic;
using System.Text;

namespace Ragades_Cube_2D.Classes.Controls
{
    /// <summary>
    /// Specifies if the coordinate should be interpreted as a
    /// relative or absolute location (i.e. "pixel 20" or 
    /// "20% from the origin"
    /// </summary>
    public enum CoordinateMode2D
    {
        /// <summary>
        /// The value should be taken as relative,
        /// as a percent value (i.e. 20 is 20% from 
        /// the origin to the opposite end of the
        /// screen)
        /// </summary>
        relative,

        /// <summary>
        /// The value should be taken as absolute,
        /// as a pixel locationvalue (i.e. 20 is 20 
        /// pixels away from the origin)
        /// </summary>
        absolute
    };

    /// <summary>
    /// Specifies the type of RCControl that a control
    /// is.
    /// </summary>
    public enum RCControlType
    {
        textBox,
        label,
        button,
        checkBox,
        image,
        radioButton,
        radioChannel
        
    };
}
