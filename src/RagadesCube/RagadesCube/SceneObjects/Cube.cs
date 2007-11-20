using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RC.Engine.GraphicsManagement;
using RC.Engine.Rendering;


namespace RagadesCube.SceneObjects
{
    public class RCCube : RCSceneNode
    {
        public enum FaceSide
        {
            Top,
            Left,
            Back,
            Right,
            Front,
            Bottom
        }

        public enum RotationDirection
        {
            Clockwise,
            CounterClockwise
        }

        protected Face[] _faces;
        
        
        protected int width;
        protected int length;
        protected int height;

        public Face[] Faces
        {
            get { return _faces; }
        }

        public int Length
        {
            get { return length; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public RCCube(int length, int width, int height)
            :base()
        {
            int faceCount = Enum.GetValues(typeof(FaceSide)).Length;

            _faces = new Face[faceCount];

            this.length = length;
            this.width = width;
            this.height = height;

            // Create / intialize each face and cubelet face collection.
            for (int iFace = 0; iFace < faceCount; iFace++)
            {
                switch ((FaceSide)iFace)
                {
                    case FaceSide.Top:
                        CreateFace(
                            (FaceSide)iFace,
                            width,
                            length,
                            Color.White
                            );
                        break;

                    case FaceSide.Bottom:
                        CreateFace(
                            (FaceSide)iFace,
                            width,
                            length,
                            Color.Yellow
                            );
                        break;

                    case FaceSide.Left:
                        CreateFace(
                            (FaceSide)iFace,
                            length,
                            height,
                            Color.Green
                            );
                        break;

                    case FaceSide.Right:
                        CreateFace(
                            (FaceSide)iFace,
                            length,
                            height,
                            Color.Blue
                            );
                        break;

                    case FaceSide.Front:
                        CreateFace(
                            (FaceSide)iFace,
                            width,
                            height,
                            Color.Red
                            );
                        break;

                    case FaceSide.Back:
                        CreateFace(
                            (FaceSide)iFace,
                            width,
                            height,
                            Color.DarkOrange
                            );
                        break;
                }
            }

            ConstructChildren();
        }

        private void CreateFace(
            FaceSide face,
            int rows,
            int cols,
            Color faceColor
            )
        {
            // Create Face
            _faces[(int)face] = new Face(rows, cols);
            _faces[(int)face].Color = faceColor;
        }

        public Vector3 GetFaceNormal(FaceSide face)
        {
            Vector3 planeNormal;
            switch (face)
            {
                case FaceSide.Top:
                    planeNormal = Vector3.Up;
                    break;
                case FaceSide.Bottom:
                    planeNormal = Vector3.Down;
                    break;
                case FaceSide.Left:
                    planeNormal = Vector3.Left;
                    break;
                case FaceSide.Right:
                    planeNormal = Vector3.Right;
                    break;
                case FaceSide.Front:
                    planeNormal = Vector3.Backward;
                    break;
                case FaceSide.Back:
                    planeNormal = Vector3.Forward;
                    break;
                default:
                    throw new ArgumentException("Invalid enum specified.");
            }

            return planeNormal;
        }

        public List<RCFacelet> GetFaceletsOnFace(FaceSide face)
        {
            List<RCFacelet> facelets = new List<RCFacelet>();

            foreach (RCSpatial sceneObject in listChildren)
            {
                if (sceneObject is RCCublet)
                {
                    RCCublet cublet = sceneObject as RCCublet;
                    Vector3 faceNormal = GetFaceNormal(face);

                    foreach (RCFacelet facelet in cublet.Facelets)
                    {
                        if (facelet != null)
                        {
                            Vector3 faceletNormal = facelet.WorldNormal;

                            if (Vector3.Dot(faceletNormal, faceNormal) > 0.9)
                                facelets.Add(facelet);
                        }
                    }
                }
            }

            return facelets;
        }

        public List<RCCublet> GetCubletsOnFace(FaceSide face)
        {
            // Check intersection with plane on specified face

            int xRange = width/2;
            int yRange = height/2;
            int zRange = length/2;

            Vector3 planeNormal = GetFaceNormal(face);
            float d = 0.0f;

            switch (face)
            {
                case FaceSide.Top:
                    d = yRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Bottom:
                    d = yRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Left:
                    d = xRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Right:
                    d = xRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Front:
                    d = zRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Back:
                    d = zRange * RCCublet.CubeletSize;
                    break;
                default:
                    throw new ArgumentException("Invalid enum specified.");
            }
            
            Vector3 worldTranslate;
            Quaternion worldRot;
            Vector3 worldScale;


            WorldTrans.Decompose(
                out worldScale, 
                out worldRot, 
                out worldTranslate
                );

            Vector3 worldNormal = Vector3.Transform(-planeNormal, worldRot);

            float dWorld  = (worldTranslate + worldNormal * d).Length() ;

            Plane facePlane = new Plane(worldNormal, dWorld);

            List<RCCublet> listCubelets = new List<RCCublet>();
            foreach (RCSpatial sceneObject in listChildren)
            {
                if (sceneObject.GetType() == typeof(RCCublet))
                {
                    PlaneIntersectionType result = sceneObject.WorldBound.Intersects(facePlane);
                    if (result == PlaneIntersectionType.Intersecting)
                    {
                        listCubelets.Add((RCCublet)sceneObject);
                    }
                }
            }

            return listCubelets;
        }

        private void ConstructChildren()
        {
 	        int xRange = width/2;
            int yRange = height/2;
            int zRange = length/2;

            for (int iX = -xRange, xRow = 0; iX <= xRange ; iX++, xRow++)
            {
                for (int iZ = -zRange, zRow = 0; iZ <= zRange; iZ++, zRow++)
                {
                    for (int iY = -yRange, yRow = 0; iY <= yRange ; iY++, yRow++)
                    {
                        // Check to see if we are inside the cube, (not on the surface)
                        if ( ((iX > -xRange) && (iX < xRange) ) &&
                             ((iY > -yRange) && (iY < yRange) ) &&
                             ((iZ > -zRange) && (iZ < zRange) )    )
                        {
                            // Skip these spots, there are no cubelets on the inside
                            continue;
                        }

                        RCCublet currentCubelet = new RCCublet(iX,iY,iZ);

                        // Check for faces and attach faces as neccissary.
                        RCFacelet facelet = null;

                        // Rows and columns for Facelets
                        int iRow = 0;
                        int iCol = 0;                        
                        // Top face
                        if(iY == yRange)
                        {
                            iRow = xRow;
                            iCol = zRow;

                            AttachFacelet(
                                currentCubelet,
                                FaceSide.Top,
                                iRow,
                                iCol
                            );

                            
                        }
                        // Bottom face
                        else if(iY == -yRange)
                        {
                            iRow = xRow;
                            iCol = zRow;

                            AttachFacelet(
                                currentCubelet,
                                FaceSide.Bottom,
                                iRow,
                                iCol
                            );
                        }

                        // Right face
                        if(iX == xRange)
                        {
                            iRow = zRow;
                            iCol = yRow;

                            AttachFacelet(
                                currentCubelet,
                                FaceSide.Right,
                                iRow,
                                iCol
                            );
                        }
                        // Left face
                        else if(iX == -xRange)
                        {
                            iRow = zRow;
                            iCol = yRow;

                            AttachFacelet(
                                currentCubelet,
                                FaceSide.Left,
                                iRow,
                                iCol
                            );
                        }

                        // Front face
                        if(iZ == zRange)
                        {
                            iRow = xRow;
                            iCol = yRow;

                            AttachFacelet(
                                currentCubelet,
                                FaceSide.Front,
                                iRow,
                                iCol
                            );
                        }
                        // Back face
                        else if(iZ == -zRange)
                        {
                            iRow = xRow;
                            iCol = yRow;

                            AttachFacelet(
                                currentCubelet,
                                FaceSide.Back,
                                iRow,
                                iCol
                            );
                        }

                        // Finaly, add the cubelet to the tree.
                        AddChild(currentCubelet);

                    }
                }
            }
        }

        private void AttachFacelet(
            RCCublet cubeletToAdd,
            FaceSide faceSide,
            int iRow,
            int iCol
            )
        {
            RCFacelet facelet = GetFacelet(faceSide, iRow, iCol);

            // Map face side to cubelet position
            RCCublet.FaceletPosition faceletPosition = (RCCublet.FaceletPosition)faceSide;
            // TODO: place in switch case block to do the mapping

            cubeletToAdd.AttachFacelet(
                faceletPosition,
                facelet
                );
        }

        private RCFacelet GetFacelet(FaceSide faceSide, int iRow, int iCol)
        {
            RCFacelet facelet = _faces[(int)faceSide].GetFacelet(iRow, iCol);
            return facelet;
        }
    }
}
