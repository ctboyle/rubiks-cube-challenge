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
        /// <summary>
        /// The sides of a cube.
        /// </summary>
        public enum FaceSide
        {
            Top,
            Left,
            Back,
            Right,
            Front,
            Bottom
        }

        /// <summary>
        /// The directions that a cube column can be rotated.
        /// </summary>
        public enum RotationDirection
        {
            Clockwise,
            CounterClockwise
        }

        /// <summary>
        /// The faces on the cube.
        /// </summary>
        protected Face[] _faces;

        /// <summary>
        /// The width of the cube. 
        /// </summary>
        protected int _width;

        /// <summary>
        /// The length of the cube.
        /// </summary>
        protected int _length;

        /// <summary>
        /// The height of the cube.
        /// </summary>
        protected int _height;

        /// <summary>
        /// Creates a new instance of the <see cref="RCCube"/> class.
        /// </summary>
        /// <param name="length">Length of the cube.</param>
        /// <param name="width">Width of the cube.</param>
        /// <param name="height">Height of the cube.</param>
        public RCCube(int length, int width, int height)
            : base()
        {
            // intitialize the member variables
            _faces = new Face[Enum.GetValues(typeof(RCCube.FaceSide)).Length];
            _length = length;
            _width = width;
            _height = height;

            // create / intialize each face and cubelet face collection
            foreach (FaceSide face in Enum.GetValues(typeof(RCCube.FaceSide)))
            {
                Color color = new Color();

                switch (face)
                {
                    case FaceSide.Top:
                        color = Color.White;
                        break;
                    case FaceSide.Bottom:
                        color = Color.Yellow;
                        break;
                    case FaceSide.Left:
                        color = Color.Green;
                        break;
                    case FaceSide.Right:
                        color = Color.Blue;
                        break;
                    case FaceSide.Front:
                        color = Color.Red;
                        break;
                    case FaceSide.Back:
                        color = Color.DarkOrange;
                        break;
                }

                CreateFace(face, color);
            }

            ConstructChildren();
        }

        /// <summary>
        /// The faces of the cube.
        /// </summary>
        public Face[] Faces
        {
            get { return _faces; }
        }

        /// <summary>
        /// Then length of the cube.
        /// </summary>
        public int Length
        {
            get { return _length; }
        }

        /// <summary>
        /// The width of the cube.
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// The height of the cube.
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Gets the local normal of a face on the cube.
        /// </summary>
        /// <param name="face">The face side.</param>
        /// <returns>The normal vector.</returns>
        public Vector3 GetLocalFaceNormal(FaceSide face)
        {
            Vector3 planeNormal = Vector3.Zero;

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
            }

            return planeNormal;
        }

        /// <summary>
        /// Gets the world normal for a face on the cube.
        /// </summary>
        /// <param name="faceSide">The face side.</param>
        /// <returns>The normal vector.</returns>
        public Vector3 GetWorldFaceNormal(FaceSide faceSide)
        {
            Vector3 planeNormal = GetLocalFaceNormal(faceSide);

            Vector3 worldTranslate;
            Quaternion worldRot;
            Vector3 worldScale;

            WorldTrans.Decompose(
                out worldScale,
                out worldRot,
                out worldTranslate
            );

            return Vector3.Transform(-planeNormal, worldRot);
        }

        /// <summary>
        /// Get the current facelets on a face.
        /// </summary>
        /// <param name="face">The face side.</param>
        /// <returns>A list of facelets.</returns>
        public List<RCFacelet> GetFaceletsOnFace(FaceSide face)
        {
            List<RCFacelet> facelets = new List<RCFacelet>();

            foreach (RCSpatial sceneObject in listChildren)
            {
                // for every cublets that is a child of the cube,
                // check to see if which face shares the same normal vector
                // as the world normal for the given face side.

                if (sceneObject is RCCublet)
                {
                    RCCublet cublet = sceneObject as RCCublet;
                    Vector3 worldFaceNormal = GetWorldFaceNormal(face);

                    foreach (RCFacelet facelet in cublet.Facelets)
                    {
                        if (facelet != null)
                        {
                            Vector3 faceletNormal = facelet.WorldNormal;

                            // a dot product threshold of 0.9 was picked as an
                            // acceptable value for determining if the two 
                            // normal vectors were close enough.
                            if (Vector3.Dot(faceletNormal, worldFaceNormal) > 0.9)
                                facelets.Add(facelet);
                        }
                    }
                }
            }

            return facelets;
        }

        /// <summary>
        /// Gets the current cublets on a face.
        /// </summary>
        /// <param name="face">The face side.</param>
        /// <returns>A list of cublets.</returns>
        public List<RCCublet> GetCubletsOnFace(FaceSide face)
        {
            int xRange = _width / 2;
            int yRange = _height / 2;
            int zRange = _length / 2;

            float distance = 0.0f;

            // calculate the length of the range
            switch (face)
            {
                case FaceSide.Top:
                    distance = yRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Bottom:
                    distance = yRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Left:
                    distance = xRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Right:
                    distance = xRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Front:
                    distance = zRange * RCCublet.CubeletSize;
                    break;
                case FaceSide.Back:
                    distance = zRange * RCCublet.CubeletSize;
                    break;
            }

            // create the face plane
            Vector3 worldNormal = GetWorldFaceNormal(face);
            float distanceWorld = (WorldTrans.Translation + worldNormal * distance).Length();
            Plane facePlane = new Plane(worldNormal, distanceWorld);

            List<RCCublet> listCubelets = new List<RCCublet>();

            foreach (RCSpatial sceneObject in listChildren)
            {
                // for every child of the cube, check to see if the cublet
                // intersects with the face plane.

                if (sceneObject is RCCublet)
                {
                    PlaneIntersectionType result = sceneObject.WorldBound.Intersects(facePlane);
                    if (result == PlaneIntersectionType.Intersecting)
                        listCubelets.Add((RCCublet)sceneObject);
                }
            }

            return listCubelets;
        }

        /// <summary>
        /// Constructs all of the children of the cube.  Used for initialization.
        /// </summary>
        private void ConstructChildren()
        {
 	        int xRange = _width / 2;
            int yRange = _height / 2;
            int zRange = _length / 2;

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

                        // Finally, add the cubelet to the tree.
                        AddChild(currentCubelet);
                    }
                }
            }
        }

        /// <summary>
        /// Attaches a facelet to a cublet.
        /// </summary>
        /// <param name="cubeletToAdd">The cublet to attach the facelet.</param>
        /// <param name="faceSide">The face side.</param>
        /// <param name="row">The row for the facelet.</param>
        /// <param name="col">The column for the facelet.</param>
        private void AttachFacelet(
            RCCublet cubeletToAdd,
            FaceSide faceSide,
            int row,
            int col
            )
        {
            RCFacelet facelet = GetFacelet(faceSide, row, col);

            // Map face side to cubelet position
            RCCublet.FaceletPosition faceletPosition = (RCCublet.FaceletPosition)faceSide;

            // TODO: place in switch case block to do the mapping

            cubeletToAdd.AttachFacelet(
                faceletPosition,
                facelet
                );
        }

        /// <summary>
        /// Get the facelets from the original face configuration by row/col.
        /// </summary>
        /// <param name="faceSide">The face side.</param>
        /// <param name="row">The row.</param>
        /// <param name="col">The column.</param>
        /// <returns>The facelet.</returns>
        private RCFacelet GetFacelet(FaceSide faceSide, int row, int col)
        {
            return _faces[(int)faceSide].GetFacelet(row, col);
        }

        /// <summary>
        /// Initializes a face on the cubelet with a paticular color.
        /// </summary>
        /// <param name="face">The face side.</param>
        /// <param name="faceColor">The color.</param>
        private void CreateFace(FaceSide face, Color faceColor)
        {
            _faces[(int)face] = new Face(_width, _length);
            _faces[(int)face].Color = faceColor;
        }
    }
}
