using System;
using System.Collections.Generic;
using System.Text;

namespace RagadesCubeWin.GUI
{
    #region Attribute doneForNow
    /// <summary>
    /// The code bearing this attribute should not require further rework
    /// unless fundamental implementation changes are enacted.
    /// </summary>
    public class doneForNow : Attribute{ }
    #endregion Attribute doneForNow

    #region Attribute incomplete(string)
    /// <summary>
    /// The code bearing this attribute works but may not
    /// have all of the funtionality intended for the final
    /// version.
    /// </summary>
    public class incomplete : Attribute { public incomplete(string reason) {} }
    #endregion Attribute incomplete(string)

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

    #region Attribute scaffolding
    /// <summary>
    /// The code with this attribute is scaffolding and should not
    /// be present in the final version of the code.
    /// </summary>
    public class scaffolding : Attribute { }
    #endregion Attribute scaffolding

    #region Attribute needsXML
    /// <summary>
    /// The code with this attribute is 
    /// completely missing its XML.
    /// </summary>
    public class needsXML : Attribute { }
    #endregion Attribute needsXML
}
 