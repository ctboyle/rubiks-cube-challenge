using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagadesCubeWin.Cameras;
using RagadesCubeWin.SceneManagement;

namespace RagadesCubeWin.Rendering
{
    /// <summary>
    /// Central functionality for Rendering the Scene.
    /// </summary>
    class RCRenderManager
    {
        /// <summary>
        /// Delegate for rendering objects.
        /// </summary>
        public delegate void RenderFunc(GraphicsDevice device);
       
        public enum DirectionalLightIndex
        {
            Light0 = 0,
            Light1,
            Light2,
            Count
        }

        private static BasicEffect _sceneEffect;
        private static int _countEnabledLights = 0;
        private static Color _clearColor = Color.CornflowerBlue;


        public static void Initialize(GraphicsDevice device)
        {
            _sceneEffect = new BasicEffect(device, null);
            device.RenderState.DepthBufferEnable = true;
            device.RenderState.StencilEnable = true;
            
        }

        /// <summary>
        /// Enables the RenderManagers effect with the lightNode's Index light
        /// and assgins the appropriate effect properties.
        /// </summary>
        public static void EnableDirectionalLight(RCDirectionalLight lightNode)
        {
            if (_sceneEffect != null)
            {
                BasicDirectionalLight effectLight = null;

                switch (lightNode.LightIndex)
                {
                    case RCRenderManager.DirectionalLightIndex.Light0:
                        effectLight = _sceneEffect.DirectionalLight0;
                        break;
                    case RCRenderManager.DirectionalLightIndex.Light1:
                        effectLight = _sceneEffect.DirectionalLight1;
                        break;
                    case RCRenderManager.DirectionalLightIndex.Light2:
                        effectLight = _sceneEffect.DirectionalLight2;
                        break;
                    default:
                        throw (new Exception("Light Index is invalid."));

                }

                if (effectLight.Enabled)
                {
                    throw (new Exception("Light is already enabled."));
                }

                // Enable Lighting
                _sceneEffect.LightingEnabled = true;
                effectLight.Enabled = true;


                // Set Effect Light properties to reflect the LightNode
                effectLight.DiffuseColor = lightNode.Diffuse;
                effectLight.SpecularColor = lightNode.Specular;
                effectLight.Direction = lightNode.Direction;

                // increment local count of enabled lights
                _countEnabledLights++;

                if (_countEnabledLights > (int)DirectionalLightIndex.Count)
                {
                    throw (new Exception("Tried to enable more lights than RenderManager provides."));
                }
            }
        }


        /// <summary>
        /// Disables the light refrenced by lightNode's index. Disables lighting if
        /// enabled light count goes to zero.
        /// </summary>
        public static void DisableDirectionalLight(RCDirectionalLight lightNode)
        {
            if (_sceneEffect != null)
            {
                BasicDirectionalLight effectLight = null;

                switch (lightNode.LightIndex)
                {
                    case RCRenderManager.DirectionalLightIndex.Light0:
                        effectLight = _sceneEffect.DirectionalLight0;
                        break;
                    case RCRenderManager.DirectionalLightIndex.Light1:
                        effectLight = _sceneEffect.DirectionalLight1;
                        break;
                    case RCRenderManager.DirectionalLightIndex.Light2:
                        effectLight = _sceneEffect.DirectionalLight2;
                        break;
                    default:
                        throw (new Exception("Light Index is invalid."));

                }

                effectLight.Enabled = false;

                _countEnabledLights--;

                // Trun off lighting if there are no lights enabled.
                if (_countEnabledLights <= 0)
                {
                    _sceneEffect.LightingEnabled = false;
                }
            }
        }

        /// <summary>
        /// Sets the current material properties to be rendered.
        /// </summary>
        public static void SetEffectMaterial(
            Vector3 ambient,
            Vector3 diffuse,
            Vector3 specular,
            float specularPower,
            Vector3 emissive,
            float alpha
            )
        {
            if (_sceneEffect != null)
            {
                _sceneEffect.AmbientLightColor = ambient;
                _sceneEffect.DiffuseColor = diffuse;
                _sceneEffect.SpecularColor = specular;
                _sceneEffect.EmissiveColor = emissive;
                _sceneEffect.Alpha = alpha;
                _sceneEffect.SpecularPower = specularPower;

                _sceneEffect.CommitChanges();
            }
        }

        /// <summary>
        /// Sets the effects world transform property
        /// </summary>
        public static void SetWorld(Matrix world)
        {
            if (_sceneEffect != null)
            {
                _sceneEffect.World = world;
            }
        }


        /// <summary>
        /// Renders the logic defined in the renderLogic function delegate.
        /// 
        /// Applys all passes of the effect to the geometry.
        /// </summary>
        public static void Render(GraphicsDevice device, RenderFunc renderLogic)
        {   
            if (_sceneEffect != null)
            {
                _sceneEffect.Begin(SaveStateMode.SaveState);

                foreach (EffectPass pass in _sceneEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();

                    // Do the specific rendering.
                    renderLogic(device);

                    pass.End();
                }

                _sceneEffect.End();
            }
        }

        /// <summary>
        /// Use to render a scene.
        /// 
        /// Will ensure that the Correct camera and viewport are used!
        /// </summary>
        public static void DrawScene(RCSceneObject sceneRoot)
        {
            if (_sceneEffect != null)
            {
                bool fCameraSuccess = false;

                _sceneEffect.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
                _sceneEffect.GraphicsDevice.RenderState.DepthBufferEnable = true;



                fCameraSuccess = UpdateSceneCameraParameters();

                // Clear screen using current clear color.
                ClearScreen();

                if (_sceneEffect != null && fCameraSuccess)
                {
                    sceneRoot.Draw(_sceneEffect.GraphicsDevice);
                }
            }
        }

        protected static void ClearScreen()
        {
            if (_sceneEffect != null)
            {
                
                _sceneEffect.GraphicsDevice.Clear(
                        ClearOptions.DepthBuffer | ClearOptions.Target,
                        _clearColor,
                        1.0f,
                        0
                        );
                
            }
        }

        
        protected static bool UpdateSceneCameraParameters()
        {
            bool fUpdatedCameraParameters = false;

            if (_sceneEffect != null)
            {
                if (RCCameraManager.ActiveCamera != null)
                {
                    // Ensure that the correct viewport is drawn to.
                    _sceneEffect.GraphicsDevice.Viewport = RCCameraManager.ActiveCamera.Viewport;
                    _sceneEffect.View = RCCameraManager.ActiveCamera.View;
                    _sceneEffect.Projection = RCCameraManager.ActiveCamera.Projection;

                    _clearColor = RCCameraManager.ActiveCamera.ClearColor;

                    fUpdatedCameraParameters = true;
                }
            }

            return fUpdatedCameraParameters;
        }
    }
}
