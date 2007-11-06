using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace RC.Engine.GraphicsManagement.BoundingVolumes
{
    public class RCAxisAlignedBoundingBox: IRCBoundingVolume
    {
        private BoundingBox _aaBox;


        public Vector3 Min
        {
            get { return _aaBox.Min; }
            set { _aaBox.Min = value; }
        }

        public Vector3 Max
        {
            get { return _aaBox.Max; }
            set { _aaBox.Max = value; }
        }

        public RCAxisAlignedBoundingBox()
        {
            _aaBox = new BoundingBox();
        }

        public RCAxisAlignedBoundingBox(
            Vector3 min,
            Vector3 max
            )
        {
            _aaBox = new BoundingBox(
                min,
                max
                );
        }

        #region RCIBoundingVolume Members

        public Nullable<float> Intersects(Ray ray)
        {
            return _aaBox.Intersects(ray);
        }

        public PlaneIntersectionType Intersects(Plane plane)
        {
            return _aaBox.Intersects(plane);
        }

        // Transforms the bounding box by the translation and rotation in
        // the specified transform;
        public IRCBoundingVolume Transform(Matrix transform)
        {

            float[] translation = new float[3]
            {
                transform.Translation.X,
                transform.Translation.Y,
                transform.Translation.Z
            };

            // Get rotation matrix component from matrix transform
            float[,] m = new float[3,3]
            {
                {transform.M11, transform.M12, transform.M13},
                {transform.M21, transform.M22, transform.M23},
                {transform.M31, transform.M32, transform.M33}
            };

            float[] aMin = new float[3] { Min.X, Min.Y, Min.Z };
            float[] aMax = new float[3] { Max.X, Max.Y, Max.Z };
            float[] bMin = new float[3];
            float[] bMax = new float[3];

            // Go through eah axis
            for (int i = 0; i < 3; i++)
            {
                // Start by adding in translation.
                bMin[i] = bMax[i] = translation[i];

                // Find extreme extents
                for (int j = 0; j < 3; j++)
                {
                    float e = m[i, j] * aMin[j];
                    float f = m[i, j] * aMax[j];

                    if (e < f)
                    {
                        bMin[i] += e;
                        bMax[i] += f;
                    }
                    else
                    {
                        bMin[i] += f;
                        bMax[i] += e;
                    }

                }
            }

            return new RCAxisAlignedBoundingBox(
                new Vector3(bMin[0], bMin[1], bMin[2]),
                new Vector3(bMax[0], bMax[1], bMax[2])
            );

            
        }

        public RCBoundingSphere ToBoundingShpere()
        {
            Vector3 center;
            float radius;

            Vector3 MinToCenter = (Max - Min)/2.0f;

            center = Min + MinToCenter;

            radius = MinToCenter.Length();

            return new RCBoundingSphere(
                center,
                radius
                );
            
        }

        #endregion
    }
}


