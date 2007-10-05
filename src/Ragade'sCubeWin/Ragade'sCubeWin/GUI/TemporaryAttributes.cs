using System;
using System.Collections.Generic;
using System.Text;

namespace Ragades_Cube_2D.Classes
{
    #region Attribute shouldBeDone
    /// <summary>
    /// The code bearing this attribute should not require further rework
    /// unless fundamental implementation changes are enacted.
    /// </summary>
    public class shouldBeDone : Attribute{ }
    #endregion Attribute shouldBeDone

    #region Attribute mayNeedExpansion(string)
    /// <summary>
    /// The code bearing this attribute works but may not
    /// have all of the funtionality intended for the final
    /// version.
    /// </summary>
    public class mayNeedExpansion : Attribute { public mayNeedExpansion(string reason) {} }
    #endregion Attribute mayNeedExpansion(string)

    #region Attribute notFullyImplemented(string)
    /// <summary>
    /// This code does not perform all of its intended
    /// functionality.  If it works at all, there is
    /// hard-code written to ignore certain cases.
    /// </summary>
    public class notFullyImplemented : Attribute { public notFullyImplemented(string reason) { } }
    #endregion Attribute notFullyImplemented(string)

    #region Attribute placeHolder
    /// <summary>
    /// The code with this attribute requires extensive work
    /// before it should be used.  It bears little or none
    /// of its intended functionality.
    /// </summary>
    public class placeHolder : Attribute { }
    #endregion Attribute placeHolder

    #region Attribute needsXML
    /// <summary>
    /// The code with this attribute is 
    /// completely missing its XML.
    /// </summary>
    public class needsXML : Attribute { }
    #endregion Attribute needsXML
}
 