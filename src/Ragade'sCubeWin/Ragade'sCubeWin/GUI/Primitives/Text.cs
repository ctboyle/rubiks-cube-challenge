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

        bool _sizeTextToWidth;
        bool _centerText;
        bool _usingScaling;


        Matrix _currentScaling;

        BitmapFont _font;
        Color _color;

        public bool SizeTextToWidth
        {
            get { return _sizeTextToWidth; }
            set 
            {
                if (_sizeTextToWidth != value)
                {
                    _sizeTextToWidth = value;
                    _textOld = true;
                }
            }
        }

        public bool CenterText
        {
            get { return _centerText; }
            set 
            { 
                _centerText = value;
                _textOld = true;
            }
        }

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
            _text = "";
            _usingScaling = false;
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
                if (_sizeTextToWidth)
                {
                    ScaleTextToWidth();
                }
                else
                {
                    ResetScale();
                }
                _textOld = false;
            }

            if (_usingScaling)
            {
                LocalTrans = _currentScaling * LocalTrans;
            }
            
            base.UpdateWorldData(gameTime);
        }

        private void ResetScale()
        {
            // Only undo scaling if it has been applied.
            if (_usingScaling)
            {
                _currentScaling = Matrix.Identity;

                _usingScaling = false;
            }
        }

        public void ScaleText(float PixelToWorldRatio)
        {
            if (PixelToWorldRatio != 0)
            {
                // Enure no scaling is in effect.
                ResetScale();

                float WorldToPixelRatio = 1.0f / PixelToWorldRatio;
                _currentScaling = Matrix.CreateScale(
                    new Vector3(
                        this.PixelsPerWorldUnitWidth * WorldToPixelRatio,
                        this.PixelsPerWorldUnitHeight * WorldToPixelRatio,
                        1.0f
                        )
                    );

                _usingScaling = true;
            }
        }

        public void GenerateCharacterQuads()
        {
            if (_font != null)
            {
                Point drawLocation = new Point(0,0);

                if (CenterText)
                {
                    int length = _font.MeasureString(_text);
                    drawLocation.X =(ScreenWidth  - length) / 2;
                }

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
                        drawLocation.X + textureInfo.position.X,
                        drawLocation.Y + textureInfo.position.Y,
                        0.0f
                        );
                }
                
            }
        }

        private void ScaleTextToWidth()
        {
            int textPixelLength = _font.MeasureString(_text);
            ScaleText( textPixelLength / WorldWidth);
        }



    }
}
