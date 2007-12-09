using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RC.Engine.GraphicsManagement;
using RC.Engine.Cameras;
using RC.Engine.Rendering;

namespace RC.Engine.SceneManagement
{
    public class RCSceneManager : IEnumerable<RCScene>
    {
        List<RCScene> _sceneList;
        IGraphicsDeviceService _graphics;
        ContentManager _content;
        
        public RCSceneManager(
            IGraphicsDeviceService graphics,
            ContentManager content
            )
        {
            _sceneList = new List<RCScene>();
            _graphics = graphics;
            _content = content;
        }
        

        public bool AddScene(RCScene scene)
        {
            bool addSucceeded = false;
            if (scene != null)
            {
                if (!scene.IsLoaded)
                {
                    scene.Load(
                        _graphics.GraphicsDevice,
                        _content
                        );
                }
                _sceneList.Add(scene);

                addSucceeded = true;
                
            }

            return addSucceeded;
        }

        public bool RemoveScene(RCScene scene)
        {
            bool removeSucceeded = false;

            if (scene != null)
            {
                removeSucceeded = _sceneList.Remove(scene);
                if (removeSucceeded)
                {
                    scene.Unload();
                }
            }

            return removeSucceeded;

        }

        public void Load(
            ContentManager contentManager
            )
        {
            _content = contentManager;

            foreach (RCScene scene in _sceneList)
            {
                scene.Load(
                    _graphics.GraphicsDevice,
                    contentManager
                );
            }
        }

        public void Unload()
        {
            foreach (RCScene scene in _sceneList)
            {
                scene.Unload();
            }
        }
    

        public void Update(GameTime gameTime)
        {
            foreach (RCScene scene in _sceneList)
            {
                scene.Update(gameTime);
            }
        }
        
        public void Draw()
        {
            foreach (RCScene scene in _sceneList)
            {
                scene.Draw(_graphics.GraphicsDevice);
            }
        }

        public IEnumerator<RCScene> GetEnumerator()
        {
            return _sceneList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _sceneList.GetEnumerator();
        }
    }
}
