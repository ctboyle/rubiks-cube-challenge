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
    public class Face
    {
        private Color _color;
        
        private int _rows;
        private int _cols;

        private RCFacelet[,] _facelets;


        public Color Color
        {
            get { return _color; }
            set
            {
                // Set all associated facelets to the specified color
                for (int iRow = 0; iRow < _rows; iRow++)
                {
                    for (int iCol = 0; iCol < _cols; iCol++)
                    {
                        _color = value;
                        _facelets[iRow, iCol].Color = value;
                    }
                }
            }
        }

        public Face(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;

            _facelets = new RCFacelet[rows,cols];

            CreateFacelets();
        }

        public RCFacelet GetFacelet(int iRow, int iCol)
        {
            return _facelets[iRow, iCol] as RCFacelet;
        }

        private void CreateFacelets()
        {
            for (int iRow = 0; iRow < _rows; iRow++)
            {
                for (int iCol = 0; iCol < _cols; iCol++)
                {
                    _facelets[iRow,iCol] = new RCFacelet(this);
                }
            }
        }

        

    }
}
