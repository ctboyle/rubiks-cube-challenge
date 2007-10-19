using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GraphicsManagement
{
    public interface INode
    {
        List<ISpatial> GetChildren();
    }

    public interface ISpatial
    {
        Matrix LocalTrans
        {
            get;
            set;
        }

        Matrix WorldTrans
        {
            get;
        }
    }
}
