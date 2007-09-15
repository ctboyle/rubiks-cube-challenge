#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RagadesCubeWin.SceneManagement;
using RagadesCubeWin.Rendering;
using RagadesCubeWin.Cameras;
#endregion

namespace RagadesCubeWin.SceneObjects
{
    /// <summary>
    /// This represents the smaller unit cubes on the Rubics Cube.
    /// </summary>
    public class RCCublet : RCSceneObject
    {
        protected Model _cubeletModel;

        private ModelMesh _currentMesh = null;

        private static string cubeletBoxAsset = "Content\\Models\\CubeletBox";

        public RCCublet()
            : base()
        {
        }

        public override void LoadGraphicsContent(
            GraphicsDevice graphics,
            ContentManager content
            )
        {
            _cubeletModel = content.Load<Model>(cubeletBoxAsset);
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

                RenderManager.SetEffectMaterial(
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
        public override void  Draw(GraphicsDevice graphicsDevice)
        {

            RenderManager.SetWorld(worldTrans);
            // Now, we will loop through each mesh in the model (in case there
            // are more than one present.
            foreach (ModelMesh mesh in _cubeletModel.Meshes)
            {
                _currentMesh = mesh;
                
                // Set the index buffer on the device once per mesh
                graphicsDevice.Indices = mesh.IndexBuffer;

                // Pass in rendering function
                RenderManager.Render(
                    graphicsDevice,
                    OnRender
                );

                
            }

        }

        // Constant sized bounding voulume
        protected override void UpdateWorldBound()
        {
            _worldBound.Center = worldTrans.Translation;

            // Box is 2 x 2 x 2
            // Therefore the smallest bounding sphere has the radius Sqrt(2) = 1.414
            _worldBound.Radius = 1.414f * worldTrans.Forward.Length();
        }
    }
}


