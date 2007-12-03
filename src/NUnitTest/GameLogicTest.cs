using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using RagadesCube.GameLogic;

using NUnit.Framework;

namespace NUnitTest
{
    [TestFixture]
    public class GameLogicTest
    {
        [Test]
        public void TestActionCube()
        {
            RCActionCube acube = new RCActionCube(new Game());

            acube.Move(Vector3.Up, Vector2.One);

            Assert.IsTrue(acube.IsMoving);

            // will need to test more here
        }

        [Test]
        public void TestCubeShuffler()
        {
            // Private class, can not be tested
        }

        [Test]
        public void TestGameLogic()
        {
            RCGameLogic gamelog = new RCGameLogic(new Game());

            Assert.AreEqual(0, gamelog.PlayerCount);

        }

        [Test]
        public void TestGamePlayer()
        {
            // ADD TEST HERE
        }

        [Test]
        public void TestPlayerIndex()
        {
            // ADD TEST HERE
        }
    }
}
