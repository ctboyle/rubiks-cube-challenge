using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RC.Engine.GraphicsManagement.BoundingVolumes
{
    public class RCBoundingSphere: IRCBoundingVolume
    {
        private BoundingSphere _sphere;
        #region RCIBoundingVolume Members

        public Vector3 Center
        {
            get { return _sphere.Center; }
            set { _sphere.Center = value;}
        }

        public float Radius
        {
            get {return _sphere.Radius;}
            set { _sphere.Radius = value;}
        }

        public RCBoundingSphere()
        {
            _sphere = new BoundingSphere();
        }

        public RCBoundingSphere(
            Vector3 center,
            float radius
        )
        {
            _sphere = new BoundingSphere(
                center,
                radius
                );
        }

        public static RCBoundingSphere CreateMerged(
            IRCBoundingVolume original,
            IRCBoundingVolume additional
            )
        {
            RCBoundingSphere sphereOrig = original.ToBoundingShpere();
            RCBoundingSphere sphereAdd = additional.ToBoundingShpere();

            BoundingSphere merged = BoundingSphere.CreateMerged(
                sphereOrig._sphere,
                sphereAdd._sphere
            );

            return new RCBoundingSphere(
                merged.Center,
                merged.Radius
                );
        }

        public float? Intersects(Ray ray)
        {
            return _sphere.Intersects(ray);
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            return _sphere.Intersects(plane);
        }

        public IRCBoundingVolume Transform(Matrix transform)
        {
            return new RCBoundingSphere(
                Center + transform.Translation,
                Radius * transform.Forward.Length()
            );
        }

        public RCBoundingSphere ToBoundingShpere()
        {
            return this;
        }

        #endregion
    }
}
