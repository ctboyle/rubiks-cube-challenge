using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using RagadesCubeWin.GraphicsManagement.BoundingVolumes;

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

        IRCBoundingVolume WorldBound
        {
            get;
        }
    }
}
