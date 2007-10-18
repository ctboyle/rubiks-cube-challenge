using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RagadesCubeWin.GraphicsManagement.BoundingVolumes
{
    public class RCBoundingRect: IRCBoundingVolume
    {
        Vector3 _p1;
        Vector3 _p2;
        Vector3 _p3;

        public Vector3 P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

        public Vector3 P2
        {
            get { return _p3; }
            set { _p3 = value; }
        }

        public Vector3 P3
        {
            get { return _p3; }
            set { _p3 = value; }
        }

        public RCBoundingRect()
        {
            _p1 = new Vector3(-1.0f, 1.0f, 0.0f);
            _p2 = new Vector3(1.0f, 1.0f, 0.0f);
            _p3 = new Vector3(1.0f, -1.0f, 0.0f);
        }

        public RCBoundingRect(
            Vector3 p1,
            Vector3 p2,
            Vector3 p3
            )
        {
            _p1 = p1;
            _p2 = p2;
            _p3 = p3;
        }




        #region RCIBoundingVolume Members

        public float? Intersects(Ray ray)
        {
            // Find plane of rectangle.
            Plane p = new Plane(
                _p1,
                _p2,
                _p3
                );

            float? dist = ray.Intersects(p);

            // We have a collision with the plane, find the
            // if it collides with the rectangle
            if (dist != null)
            {
                // First, find the point of intersection.
                Vector3 pIntersect = (Vector3)(ray.Position + ray.Direction * dist);

                // Test intersection.
                Vector3 v1 = _p2 - _p1;
                Vector3 v3 = -v1;
                Vector3 v4 = pIntersect - _p1;
                Vector3 v5 = pIntersect - _p3;

                v1.Normalize();
                v3.Normalize();
                v4.Normalize();
                v5.Normalize();

                if (Vector3.Dot(v1, v4) < 0 &&
                    Vector3.Dot(v3, v5) < 0)
                {
                    // No colision, ensure dist is null.
                    dist = null;
                }
                
            }

            // If there was a collision dist will be non null.
            return dist;
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            // Dot each vertex + plane origin with plane normal. If they
            // all have same sign, no collision.
            
            throw new Exception("The method or operation is not implemented.");
            
        }

        public IRCBoundingVolume Transform(Matrix transform)
        {
            return new RCBoundingRect(
                Vector3.Transform(_p1, transform),
                Vector3.Transform(_p2, transform),
                Vector3.Transform(_p3, transform)
            );
        }

        public RCBoundingSphere ToBoundingShpere()
        {
            Vector3 center;
            float radius;

            Vector3 p1ToCenter = (_p2 - _p3) / 2.0f;

            center = _p1 + p1ToCenter;
            radius = p1ToCenter.Length();

            return new RCBoundingSphere(
                center,
                radius
                );
            
        }

        #endregion
    }
}
