using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.GUI.Panes;
using RagadesCubeWin.Rendering;

namespace RagadesCubeWin.GUI.Primitives
{
    public class RCText : RCPane
    {
        string _text;
        bool _textOld;

        BitmapFont _font;
        Color _color;

        public string Text
        {
            get { return _text; }
            set 
            {
                _text = value;
                _textOld = true;
            }
        }

        public BitmapFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public RCText(
            BitmapFont font,
            float width,
            float height,
            int scrWidth,
            int scrHeight
            )
            :base(width, height, scrWidth, scrHeight)
        {
            _font = font;
            _color = Color.White;
        }

        public override void LoadGraphicsContent(GraphicsDevice graphics, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            // Force regeneration on next update.
            _textOld = true;
            base.LoadGraphicsContent(graphics, content);
        }

        public override void UnloadGraphicsContent()
        {
            
            base.UnloadGraphicsContent();
        }

        protected override void UpdateWorldData(GameTime gameTime)
        {
            // Update text if it has been changed before this frame.
            if (_textOld)
            {
                GenerateCharacterQuads();
                _textOld = false;
            }
            
            base.UpdateWorldData(gameTime);
        }

        public void ScaleText(float PixelToWorldRatio)
        {
            if (PixelToWorldRatio != 0)
            {
                float WorldToPixelRatio = 1.0f / PixelToWorldRatio;

                LocalTrans *= Matrix.CreateScale(
                    new Vector3(
                        WorldToPixelRatio,
                        WorldToPixelRatio,
                        0.0f
                        )
                    );
            }
        }

        public void GenerateCharacterQuads()
        {
            if (_font != null)
            {
                // Get rid of the old text.
                _listChildren.Clear();

                GlyphTextureString textureString = Font.GenerateGlyphTexturesString(
                    Vector2.Zero, 
                    _text
                    );
                
                foreach (GlyphTextureInfo textureInfo in textureString.TexureInfoList)
                {
                    RCQuad charQuad = new RCQuad(
                        0, 0,
                        textureInfo.size.X,
                        textureInfo.size.Y
                        );

                    charQuad.Image = textureInfo.glyphTexture;
                    
                    charQuad.Color = _color;

                    AddChild(
                        charQuad,
                        textureInfo.position.X,
                        textureInfo.position.Y,
                        0.0f
                        );
                }
                
            }
        }



    }
}
