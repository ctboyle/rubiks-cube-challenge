using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RC.Engine.GraphicsManagement.BoundingVolumes
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
            get { return _p2; }
            set { _p2 = value; }
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

        public Vector3 Normal
        {
            get
            {
                Vector3 right = _p2 - _p1;
                Vector3 up = _p2 - _p3;
                Vector3 normal = Vector3.Cross(right, up);
                normal.Normalize();
                return normal;
            }
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
                Vector3 pIntersect = (ray.Position + ray.Direction * dist.Value);

                // Test intersection.
                Vector3 right = _p2 - _p1;
                Vector3 up = _p2 - _p3;

                Vector3 v1 = pIntersect - _p1;

                Vector3 v3 = pIntersect - _p3;


                up.Normalize();
                right.Normalize();
                v1.Normalize();
                v3.Normalize();
                

                if (!(Vector3.Dot(-up, v1)    > 0     && 
                      Vector3.Dot(right, v1)  > 0     &&
                      Vector3.Dot(-right, v3) > 0     &&
                      Vector3.Dot(up,v3)      > 0 ))
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

            // Find plane of rectangle.
            Plane rectPlane = new Plane(
                _p1,
                _p2,
                _p3
                );

            // L = positionV + t * directionV

            // Compute direction of intersection line
            Vector3 direction = Vector3.Cross(rectPlane.Normal, plane.Normal);

            // If direction is (near) zero, the planes are parallel (and separated)
            // or coincident, so they’re not considered intersecting
            float denom = direction.Length();
            if (denom < 0.00001f) return PlaneIntersectionType.Back;

            // compute position vector on intersection line
            Vector3 position = Vector3.Cross(
                rectPlane.D * plane.Normal - plane.D * rectPlane.Normal, 
                direction);

            position /= denom;

            return PlaneIntersectionType.Intersecting;
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

            Vector3 p1ToCenter = (_p3 - _p1) / 2.0f;

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
