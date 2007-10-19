using System;
using System.Collections.Generic;
using System.Text;

using RagadesCubeWin.GraphicsManagement;
using RagadesCubeWin.Cameras;
namespace RagadesCubeWin.GUI
{
    public class RCGUIManager
    {
        private RCSpatial _sceneRoot;
        private RCCamera _sceneCamera;
        private List<IGUIElement> _guiObjects;

        public RCGUIManager(RCSpatial sceneRoot, RCCamera sceneCamera)
        {
            _sceneCamera = sceneCamera;
            _sceneRoot = _sceneRoot;
            _guiObjects = new List<IGUIElement>();

        }

        public void RebuildGuiObjectList()
        {
            _guiObjects.Clear();

            BuildGuiObjectListRecursive(_sceneRoot);
        }

        private void BuildGuiObjectListRecursive(ISpatial root)
        {
            // Find all gui elements under this root.
            // First check to see if it is a GUIElement itself
            if (root is IGUIElement)
            {
                _guiObjects.Add((IGUIElement)root);
            }

            // Check root's children are GUIElements, if any.
            if (root is INode)
            {
                INode rootNode = (INode)root;

                List<ISpatial> listChildren = rootNode.GetChildren();

                foreach (ISpatial child in listChildren)
                {
                    BuildGuiObjectListRecursive(child);
                }
            }
        }

        
    }
}
