using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RC.Gui.Fonts
{

    public interface IFontManager
    {
        bool AddFont(
            string fontLabel,
            BitmapFont font
            );

        bool AddFontFromAsset(
           string fontLabel,
           string fontXmlFilePath
           );

         bool RemoveFont(
           string fontLabel,
           BitmapFont font
           );

        BitmapFont GetFont(
            string label
            );

        void LoadFonts(
            GraphicsDevice graphicsDevice,
            ContentManager content
            );

        void UnloadFonts();


    }

    class FontManager: IFontManager
    {
        
        private Dictionary<string, BitmapFont> _managedFonts;

        
        public FontManager(Game game)
        {
            game.Services.AddService(typeof(IFontManager), this);
            
            _managedFonts = new Dictionary<string, BitmapFont>();
        }

        public void LoadFonts(
            GraphicsDevice graphicsDevice,
            ContentManager content
            )
        {
            foreach (BitmapFont font in _managedFonts.Values)
            {
                font.Reset(
                    graphicsDevice,
                    content
                    );
            }
        }

        public void UnloadFonts()
        {
            // no need, when load is called again,
            // Reset will take care of making new content.
        }

        public bool AddFontFromAsset(
            string fontLabel,
            string fontXmlFilePath
            )
        {
            bool fAddSuccess = false;

            if (!_managedFonts.ContainsKey(fontLabel))
            {
                try
                {
                    BitmapFont font = new BitmapFont(fontXmlFilePath);
                    AddFont(fontLabel, font);
                    fAddSuccess = true;
                }
                catch
                {
                    // Font XML file could not be found.
                    Debug.Write("Error reading/finding XML font file.", "FontManager");
                    fAddSuccess = false;
                }
            }

            return fAddSuccess;
        }

        public bool AddFont(
            string fontLabel,
            BitmapFont font
            )
        {
            bool fAddSuccess = false;
            if (font != null)
            {
                if (!_managedFonts.ContainsKey(fontLabel))
                {
                    _managedFonts.Add(fontLabel, font);
                    fAddSuccess = true;
                }
                else
                {
                    Debug.Write("Font already exists in manager.", "FontManager");
                }
            }

            return fAddSuccess;
        }

        public bool RemoveFont(
            string fontLabel,
            BitmapFont font
            )
        {
            bool fRemoveSuccess = false;
            if (font != null)
            {
                if (_managedFonts.ContainsKey(fontLabel))
                {
                    _managedFonts.Remove(fontLabel);
                    fRemoveSuccess = true;
                }
            }

            return fRemoveSuccess;
        }

        public BitmapFont GetFont(string label)
        {
            BitmapFont retrivedFont = null;

            retrivedFont = _managedFonts[label] as BitmapFont;

            return retrivedFont;
        }
    }
}
