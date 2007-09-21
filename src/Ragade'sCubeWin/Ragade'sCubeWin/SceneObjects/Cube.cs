using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Rendering;


namespace RagadesCubeWin.SceneObjects
{
    class RCCube : RCNode
    {
        enum FaceSide
        {
            Top,
            Left,
            Back,
            Right,
            Front,
            Bottom,
            Count
        }


        protected Face[] _faces;
        
        
        protected int width;
        protected int length;
        protected int height;

        public Face[] Faces
        {
            get { return _faces; }
        }

        public RCCube(int length, int width, int height)
            :base()
        {
            _faces = new Face[(int)FaceSide.Count];

            this.length = length;
            this.width = width;
            this.height = height;
            
            _faces[(int)FaceSide.Top] = new Face(width, length);
            _faces[(int)FaceSide.Top].Color = Color.White;

            _faces[(int)FaceSide.Bottom] = new Face(width, length);
            _faces[(int)FaceSide.Bottom].Color = Color.Yellow;

            _faces[(int)FaceSide.Left] = new Face(length, height);
            _faces[(int)FaceSide.Left].Color = Color.Green;

            _faces[(int)FaceSide.Right] = new Face(length, height);
            _faces[(int)FaceSide.Right].Color = Color.Blue;

            _faces[(int)FaceSide.Back] = new Face(width, height);
            _faces[(int)FaceSide.Back].Color = Color.Orange;

            _faces[(int)FaceSide.Front] = new Face(width, height);
            _faces[(int)FaceSide.Front].Color = Color.Red;
            
            ConstructChildren();

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

                        // Rows and columns for Facelets
                        int iRow = 0;
                        int iCol = 0;
                        RCFacelet facelet = null;
                        // Top face
                        if(iY == yRange)
                        {
                            iRow = xRow;
                            iCol = zRow;

                            facelet = _faces[(int)FaceSide.Top].GetFacelet(iRow, iCol);

                            currentCubelet.AttachFacelet(
                                RCCublet.FaceletPosition.Top,
                                facelet
                                );
                        }
                        // Bottom face
                        else if(iY == -yRange)
                        {
                            iRow = xRow;
                            iCol = zRow;
                            
                            facelet = _faces[(int)FaceSide.Bottom].GetFacelet(iRow, iCol);

                            currentCubelet.AttachFacelet(
                                RCCublet.FaceletPosition.Bottom,
                                facelet
                                );
                        }

                        // Right face
                        if(iX == xRange)
                        {
                            iRow = zRow;
                            iCol = yRow;
                            
                            facelet = _faces[(int)FaceSide.Right].GetFacelet(iRow, iCol);

                            currentCubelet.AttachFacelet(
                                RCCublet.FaceletPosition.Right,
                                facelet
                                );
                        }
                        // Left face
                        else if(iX == -xRange)
                        {
                            iRow = zRow;
                            iCol = yRow;
                            
                            facelet = _faces[(int)FaceSide.Left].GetFacelet(iRow, iCol);

                            currentCubelet.AttachFacelet(
                                RCCublet.FaceletPosition.Left,
                                facelet
                                );
                        }

                        // Front face
                        if(iZ == zRange)
                        {
                            iRow = xRow;
                            iCol = yRow;

                            facelet = _faces[(int)FaceSide.Front].GetFacelet(iRow, iCol);

                            currentCubelet.AttachFacelet(
                                RCCublet.FaceletPosition.Front,
                                facelet
                                );
                        }
                        // Back face
                        else if(iZ == -yRange)
                        {
                            iRow = xRow;
                            iCol = yRow;

                            facelet = _faces[(int)FaceSide.Back].GetFacelet(iRow, iCol);

                            currentCubelet.AttachFacelet(
                                RCCublet.FaceletPosition.Back,
                                facelet
                                );
                        }

                        // Finaly, add the cubelet to the tree.
                        AddChild(currentCubelet);
                    }
                }
            }
        }
    }
}
