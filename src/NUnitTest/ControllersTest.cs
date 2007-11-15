using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using NUnit.Framework;

using RagadesCube.Controllers;

namespace NUnitTest
{
    [TestFixture]
    public class ControllersTest
    {
        [Test]
        public void TestCubeController()
        {
            RCCubeController rccont = new RCCubeController();

            rccont.AttachToObject(new RagadesCube.SceneObjects.RCCube(3, 3, 3));
            

            Assert.IsFalse(rccont.IsAnimating);

            rccont.RotateFace(RagadesCube.SceneObjects.RCCube.FaceSide.Front,
                                RagadesCube.SceneObjects.RCCube.RotationDirection.Clockwise);

            Assert.IsTrue(rccont.IsAnimating);
            
        }

        [Test]
        public void TestMenuCameraController()
        {
            // ADD TEST HERE
        }

        [Test]
        public void TestScaleController()
        {
            // ADD TEST HERE
        }

        [Test]
        public void TestSpinController()
        {
            // ADD TEST HERE
        }

        [Test]
        public void TestWobbleController()
        {
            // ADD TEST HERE
        }
    }
}
