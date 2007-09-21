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
    public class RCFacelet : RCSceneObject
    {
        protected Face _parentFace;

        private static string faceletBoxAsset = "Content\\Models\\Facelet";

        private static Model _faceletModel;
        private ModelMesh _currentMesh = null;

        Color _color;

        private Vector3 AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
        private Vector3 DiffuseColor;
        private Vector3 SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);
        private float SpecularPower = 100.0f;
        private Vector3 EmissiveColor = new Vector3(0.0f, 0.0f, 0.0f);
        private float Alpha = 1.0f;


        public Color Color
        {
            get { return _color; }
            set 
            {
                _color = value;
                DiffuseColor = _color.ToVector3();
            }
        }
        

        public RCFacelet(Face parentFace)
            :base()
        {
            _parentFace = parentFace;
        }

        public override void LoadGraphicsContent(
            GraphicsDevice graphics,
            ContentManager content
            )
        {
            if (_faceletModel == null)
            {
                _faceletModel = content.Load<Model>(faceletBoxAsset);
            }

            // TODO: override unload and set refrence to null.
        }


        /// <summary>
        /// Is called by the render manager. Specifies the exact drawing code.
        /// 
        /// See RenderManager.Render()
        /// </summary>
        public void OnRender(GraphicsDevice graphicsDevice)
        {
            // Each mesh is made of parts (grouped by texture, etc.)
            foreach (ModelMeshPart part in _currentMesh.MeshParts)
            {
                BasicEffect currentEffect = (BasicEffect)part.Effect;

                RCRenderManager.SetEffectMaterial(
                    AmbientLightColor,
                    DiffuseColor,
                    SpecularColor,
                    SpecularPower,
                    EmissiveColor,
                    Alpha
                    );


                // Change the device settings for each part to be rendered
                graphicsDevice.VertexDeclaration = part.VertexDeclaration;
                graphicsDevice.Vertices[0].SetSource(
                    _currentMesh.VertexBuffer,
                    part.StreamOffset,
                    part.VertexStride
                );
                

                // Finally draw the actual triangles on the screen
                graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    part.BaseVertex, 0,
                    part.NumVertices,
                    part.StartIndex,
                    part.PrimitiveCount
                );
            }
        }

        /// <summary>
        /// Draws the cubelet mesh
        /// </summary>
        public override void Draw(GraphicsDevice graphicsDevice)
        {

            RCRenderManager.SetWorld(worldTrans);
            // Now, we will loop through each mesh in the model (in case there
            // are more than one present.
            foreach (ModelMesh mesh in _faceletModel.Meshes)
            {
                _currentMesh = mesh;

                // Set the index buffer on the device once per mesh
                graphicsDevice.Indices = mesh.IndexBuffer;

                // Pass in rendering function
                RCRenderManager.Render(
                    graphicsDevice,
                    OnRender
                );
            }
        }

        protected override void UpdateWorldBound()
        {

        }
        
    }
}
