using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


using NUnit.Framework;
using RC.Input.Events;
using RC.Input.Types;
using RC.Input.Watchers;

namespace NUnitTest
{
    [TestFixture]
    public class InputTest
    {
        [Test]
        public void TestKeyboardEvent()
        {
            RC.Input.Events.KeyboardEvent ke1 = new RC.Input.Events.KeyboardEvent(Keys.A,
                                                        RC.Input.Types.EventTypes.OnDown, kbe);
            

            Assert.AreEqual(ke1.getKey(), Keys.A);
            Assert.AreEqual(ke1.getEventType(), RC.Input.Types.EventTypes.OnDown);
        }

        [Test]
        public void TestMouseEvent()
        {
            RC.Input.Events.MouseEvent me1 = new RC.Input.Events.MouseEvent(RC.Input.Types.MouseInput.LeftButton,
                                                                            RC.Input.Types.EventTypes.OnDown, mbe);
                                             

            Assert.AreEqual(me1.getType(), RC.Input.Types.MouseInput.LeftButton);
            Assert.AreEqual(me1.getEvent(), RC.Input.Types.EventTypes.OnDown);
                                                                            
        }

        [Test]
        public void TestGamePadEvent()
        {
            RC.Input.Events.XBox360GamePadEvent ge1 = new RC.Input.Events.XBox360GamePadEvent(RC.Input.Types.XBox360GamePadTypes.B,
                                                                                              RC.Input.Types.EventTypes.Pressed,
                                                                                              kbe);

            Assert.AreEqual(ge1.getType(), RC.Input.Types.XBox360GamePadTypes.B);
            Assert.AreEqual(ge1.getEvent(), RC.Input.Types.EventTypes.Pressed);
        }

        [Test]
        public void TestKeyboardWatcher()
        {
            KeyboardWatcher kw = new KeyboardWatcher();
            Assert.IsTrue(kw.WatchEvent(new KeyboardEvent(Keys.A, EventTypes.Pressed, kbe)));
            Assert.IsTrue(kw.WatchEvent(new KeyboardEvent(Keys.B, EventTypes.Pressed, kbe)));
            
            // THESE THROW AN ERROR, WHY?  I'M NOT SURE, DEFECT!!!!
            Assert.IsTrue(kw.RemoveEvent(new KeyboardEvent(Keys.A, EventTypes.Pressed, kbe)));
            Assert.IsTrue(kw.RemoveEvent(new KeyboardEvent(Keys.B, EventTypes.Pressed, kbe)));
            
            // THIS IS FINE
            Assert.IsFalse(kw.RemoveEvent(new KeyboardEvent(Keys.C, EventTypes.Released, kbe)));
        }

        [Test]
        public void TestMouseWatcher()
        {
            MouseWatcher mw = new MouseWatcher();
            Assert.IsTrue(mw.WatchEvent(new MouseEvent(MouseInput.LeftButton, EventTypes.Pressed, mbe)));
            // PROBABLY SAME LIKE ABOVE, HAS AN ERROR!!!!  DEFECT!!!!
            Assert.IsTrue(mw.RemoveEvent(new MouseEvent(MouseInput.LeftButton, EventTypes.Pressed, mbe)));
            Assert.IsFalse(mw.RemoveEvent(new MouseEvent(MouseInput.LeftButton, EventTypes.Released, mbe)));
        }

        [Test]
        public void TestGamePadWatcher()
        {
            XBox360GamePad gw = new XBox360GamePad(PlayerIndex.One);

            Assert.IsTrue(gw.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DRIGHT, EventTypes.Pressed,
                                                                kbe)));
            
            //PROBABLY SAME LIKE ABOVE, NUNIT SHOWS AN ERROR, WOOT!
            Assert.IsTrue(gw.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DRIGHT, EventTypes.Pressed,
                                                                kbe)));

            Assert.IsFalse(gw.WatchEvent(new XBox360GamePadEvent(XBox360GamePadTypes.DRIGHT, EventTypes.Released,
                                                                kbe)));


        }

        protected void kbe()
        {
            // do nothing
        }

        protected void mbe(Vector2 v1, Vector2 v2)
        {
            // do nothing
        }

    }
}
