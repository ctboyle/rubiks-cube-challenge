using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RagadesCubeWin.GraphicsManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace RagadesCubeWin.SceneObjects
{
    /// <summary>
    /// The cube cursor is used to visibly select a cube face to aid with
    /// rotating the selected cube face.
    /// </summary>
    public class RCCubeCursor : RCSpatial
    {
        RCCube _theCube;
        Color _color;
        bool _isVisible;
        RCCube.FaceSide _selectedFace;
        VertexPositionNormalTexture[] _vertexArray;
        short[] _vertexIndices;
        IndexBuffer _indexBuffer;
        VertexBuffer _vertexBuffer;

        /// <summary>
        /// Creates a new instance of the cursor.
        /// </summary>
        /// <param name="cube">The cube that the cursor will be applied to.</param>
        public RCCubeCursor(RCCube cube) : this(cube, Color.Yellow)
        {
        }

        /// <summary>
        /// Creates a new instance of the cursor.
        /// </summary>
        /// <param name="cube">The cube that the cursor will be applied to.</param>
        /// <param name="color">The color of the cursor.</param>
        public RCCubeCursor(RCCube cube, Color color)
        {
            _theCube = cube;
            _color = color;
            _isVisible = true;
            _selectedFace = RCCube.FaceSide.Front;

            InitVertices();
        }

        /// <summary>
        /// Determines if the cursor should be drawn on the screen.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        /// <summary>
        /// The current selected face of the cube.
        /// </summary>
        public RCCube.FaceSide SelectedFace
        {
            get { return _selectedFace; }
            set { _selectedFace = value; }
        }

        /// <summary>
        /// The color of the cursor.
        /// </summary>
        public Color HighlightColor
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Initializes graphics specific content.
        /// </summary>
        /// <param name="graphics">The graphics device.</param>
        /// <param name="content">Content manager for loading assets.</param>
        public override void LoadGraphicsContent(GraphicsDevice graphics, ContentManager content)
        {
            // initialize the vertex buffer and its data
            _vertexBuffer = new VertexBuffer(
                graphics, 
                typeof(VertexPositionNormalTexture), 
                24, 
                ResourceUsage.WriteOnly, 
                ResourceManagementMode.Automatic
            );

            _vertexBuffer.SetData<VertexPositionNormalTexture>(_vertexArray);

            // initialize the index buffer and its data
            _indexBuffer = new IndexBuffer(
                graphics, 
                sizeof(short) * _vertexIndices.Length,
                ResourceUsage.None, 
                ResourceManagementMode.Automatic, 
                IndexElementSize.SixteenBits
            );

            _indexBuffer.SetData<short>(_vertexIndices);
        }

        /// <summary>
        /// Starts the drawing.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public override void Draw(GraphicsDevice graphicsDevice)
        {
            if (!_isVisible) return;

            UpdateLocalTrans();

            Vector3 color = _color.ToVector3();

            Rendering.RCRenderManager.SetWorld(WorldTrans);
            Rendering.RCRenderManager.SetEffectMaterial(color, color, color, 1.0f, color, 0.4f);
            Rendering.RCRenderManager.Render(graphicsDevice, OnRender);
        }

        protected override void UpdateWorldBound()
        {
            // nothing to do
        }

        /// <summary>
        /// Updates the local transform to the currently selected face.
        /// </summary>
        private void UpdateLocalTrans()
        {
            // scale the cursor cube to cover an entire face
            Vector3 scale = new Vector3((float)_theCube.Width - 0.01f, (float)_theCube.Height, 1.0f);
            LocalTrans = Matrix.CreateScale(scale);

            Vector3 faceNormal = _theCube.GetFaceNormal(_selectedFace);

            // translate and rotate the cursor cube to the correct position
            switch(_selectedFace)
            {
                case RCCube.FaceSide.Top:
                case RCCube.FaceSide.Bottom:
                    LocalTrans *= Matrix.CreateRotationX(MathHelper.PiOver2);
                    LocalTrans *= Matrix.CreateTranslation(faceNormal * (_theCube.Height - 1));
                    break;
                case RCCube.FaceSide.Left:
                case RCCube.FaceSide.Right:
                    LocalTrans *= Matrix.CreateRotationY(MathHelper.PiOver2);
                    LocalTrans *= Matrix.CreateTranslation(faceNormal * (_theCube.Width - 1));
                    break;
                case RCCube.FaceSide.Front:
                case RCCube.FaceSide.Back:
                    LocalTrans *= Matrix.CreateTranslation(faceNormal * (_theCube.Length - 1));
                    break;
            }
        }

        /// <summary>
        /// Called when it is time to render the cursor.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        private void OnRender(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice, VertexPositionNormalTexture.VertexElements);
            graphicsDevice.Indices = _indexBuffer;
            graphicsDevice.Vertices[0].SetSource(_vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
        }

        /// <summary>
        /// Initializes the vertex indices and the vertex array.
        /// </summary>
        private void InitVertices()
        {
            // initialize the vertex indices

            _vertexIndices = new short[2 * 3 * 6];

            for (int i = 0; i < 6; ++i)
            {
                _vertexIndices[i * 6 + 0] = (short)(i * 4 + 0);
                _vertexIndices[i * 6 + 1] = (short)(i * 4 + 1);
                _vertexIndices[i * 6 + 2] = (short)(i * 4 + 2);
                _vertexIndices[i * 6 + 3] = (short)(i * 4 + 0);
                _vertexIndices[i * 6 + 4] = (short)(i * 4 + 2);
                _vertexIndices[i * 6 + 5] = (short)(i * 4 + 3);
            }

            // initialize the vertex array

            Vector3[] vertexPositions = new Vector3[8];
            vertexPositions[0] = new Vector3(-1.0f, -1.0f, 1.0f);
            vertexPositions[1] = new Vector3(-1.0f, 1.0f, 1.0f);
            vertexPositions[2] = new Vector3(1.0f, 1.0f, 1.0f);
            vertexPositions[3] = new Vector3(1.0f, -1.0f, 1.0f);
            vertexPositions[4] = new Vector3(-1.0f, -1.0f, -1.0f);
            vertexPositions[5] = new Vector3(-1.0f, 1.0f, -1.0f);
            vertexPositions[6] = new Vector3(1.0f, 1.0f, -1.0f);
            vertexPositions[7] = new Vector3(1.0f, -1.0f, -1.0f);

            Vector2[] textureCoords = new Vector2[4];
            textureCoords[0] = new Vector2(0.0f, 0.0f);
            textureCoords[1] = new Vector2(1.0f, 0.0f);
            textureCoords[2] = new Vector2(1.0f, 1.0f);
            textureCoords[3] = new Vector2(0.0f, 1.0f);

            _vertexArray = new VertexPositionNormalTexture[24];

            int nextVertex = 0;

            // front face
            SetVertex(vertexPositions[0], Vector3.Backward, textureCoords[3], nextVertex++);
            SetVertex(vertexPositions[1], Vector3.Backward, textureCoords[0], nextVertex++);
            SetVertex(vertexPositions[2], Vector3.Backward, textureCoords[1], nextVertex++);
            SetVertex(vertexPositions[3], Vector3.Backward, textureCoords[2], nextVertex++);

            // right face
            SetVertex(vertexPositions[3], Vector3.Right, textureCoords[3], nextVertex++);
            SetVertex(vertexPositions[2], Vector3.Right, textureCoords[0], nextVertex++);
            SetVertex(vertexPositions[6], Vector3.Right, textureCoords[1], nextVertex++);
            SetVertex(vertexPositions[7], Vector3.Right, textureCoords[2], nextVertex++);

            // rear face
            SetVertex(vertexPositions[7], Vector3.Forward, textureCoords[3], nextVertex++);
            SetVertex(vertexPositions[6], Vector3.Forward, textureCoords[0], nextVertex++);
            SetVertex(vertexPositions[5], Vector3.Forward, textureCoords[1], nextVertex++);
            SetVertex(vertexPositions[4], Vector3.Forward, textureCoords[2], nextVertex++);

            // left face   
            SetVertex(vertexPositions[4], Vector3.Left, textureCoords[3], nextVertex++);
            SetVertex(vertexPositions[5], Vector3.Left, textureCoords[0], nextVertex++);
            SetVertex(vertexPositions[1], Vector3.Left, textureCoords[1], nextVertex++);
            SetVertex(vertexPositions[0], Vector3.Left, textureCoords[2], nextVertex++);

            // top face
            SetVertex(vertexPositions[1], Vector3.Up, textureCoords[3], nextVertex++);
            SetVertex(vertexPositions[5], Vector3.Up, textureCoords[0], nextVertex++);
            SetVertex(vertexPositions[6], Vector3.Up, textureCoords[1], nextVertex++);
            SetVertex(vertexPositions[2], Vector3.Up, textureCoords[2], nextVertex++);

            // bottom face
            SetVertex(vertexPositions[4], Vector3.Down, textureCoords[3], nextVertex++);
            SetVertex(vertexPositions[0], Vector3.Down, textureCoords[0], nextVertex++);
            SetVertex(vertexPositions[3], Vector3.Down, textureCoords[1], nextVertex++);
            SetVertex(vertexPositions[7], Vector3.Down, textureCoords[2], nextVertex++);

        }

        /// <summary>
        /// Sets the data in the vertex array.
        /// </summary>
        /// <param name="position">The position vector.</param>
        /// <param name="normal">The normal vector.</param>
        /// <param name="texture">The texture coordinate.</param>
        /// <param name="nextVertex">The index.</param>
        private void SetVertex(Vector3 position, Vector3 normal, Vector2 texture, int nextVertex)
        {
            _vertexArray[nextVertex].Position = position;
            _vertexArray[nextVertex].Normal = normal;
            _vertexArray[nextVertex].TextureCoordinate = texture;
        }
    }
}
