using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RC.Engine.GraphicsManagement.BoundingVolumes
{
    public interface IRCBoundingVolume
    {
        float? Intersects(Ray ray);
        PlaneIntersectionType Intersects(Plane plane);
        IRCBoundingVolume Transform (Matrix transform);
        RCBoundingSphere ToBoundingShpere();
    }
}
