// SimpleFontTest.cs
// a simple test of the BitmapFont class

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAExtras;

namespace SimpleFontTest
{
    class SimpleFontTest : Microsoft.Xna.Framework.Game
    {
		private BitmapFont m_font;
        private Microsoft.Xna.Framework.Components.GraphicsComponent m_graphics;

		public SimpleFontTest()
        {
            m_graphics = new Microsoft.Xna.Framework.Components.GraphicsComponent();
			this.GameComponents.Add(m_graphics);
		}

		protected override void OnStarting()
		{
			base.OnStarting();
			m_graphics.GraphicsDevice.DeviceReset += new EventHandler(GraphicsDevice_DeviceReset);

			m_font = new BitmapFont("comic.xml");

			LoadResources();
		}

		void GraphicsDevice_DeviceReset(object sender, EventArgs e)
		{
			LoadResources();
		}

		void LoadResources()
		{
			m_font.Reset(m_graphics.GraphicsDevice);
		}

        protected override void Draw()
        {
            // Make sure we have a valid device
            if (!m_graphics.EnsureDevice())
                return;

            m_graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            m_graphics.GraphicsDevice.BeginScene();

			// draw the string
			m_font.DrawString(20, 20, Color.DarkBlue, "Hello GraphicsDevice!");

            m_graphics.GraphicsDevice.EndScene();
            m_graphics.GraphicsDevice.Present();
        }
    }
}
