using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;

namespace RagadesCubeWin.SceneObjects
{
    public class RCCubeletBox : RCSpatial
    {

        private static Model _cubeletModel = null;
        private ModelMesh _currentMesh = null;

        private static string cubeletBoxAsset = "Content\\Models\\CubeletBox";

        public RCCubeletBox()
            :base()
        {
        }

        public override void LoadGraphicsContent(
            GraphicsDevice graphics,
            ContentManager content
            )
        {
            if (_cubeletModel == null)
            {
                _cubeletModel = content.Load<Model>(cubeletBoxAsset);
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
                    currentEffect.AmbientLightColor,
                    currentEffect.DiffuseColor,
                    currentEffect.SpecularColor,
                    currentEffect.SpecularPower,
                    currentEffect.EmissiveColor,
                    currentEffect.Alpha
                    );


                // Change the device settings for each part to be rendered
                graphicsDevice.VertexDeclaration = part.VertexDeclaration;
                graphicsDevice.Vertices[0].SetSource(
                    _currentMesh.VertexBuffer,
                    part.StreamOffset,
                    part.VertexStride
                );

                // Make sure we use the texture for the current part also
                graphicsDevice.Textures[0] = currentEffect.Texture;

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
            foreach (ModelMesh mesh in _cubeletModel.Meshes)
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

        protected override void  UpdateWorldBound()
        {
            
        }

    }
}
