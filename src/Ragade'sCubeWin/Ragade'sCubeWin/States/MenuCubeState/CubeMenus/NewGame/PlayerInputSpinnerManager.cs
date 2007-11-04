using System;
using System.Collections.Generic;
using System.Text;
using RagadesCubeWin.GUI.Controls.Control_Subclasses;
using System.Diagnostics;
using RagadesCubeWin.GUI.Fonts;
using RagadesCubeWin.GameLogic.InputSchemes;
using Microsoft.Xna.Framework;

namespace RagadesCubeWin.States.MenuCubeState.CubeMenus
{
    struct SpinItemMapEntry
    {
        public SpinItemMapEntry(string key, string filename, RCGLInputScheme inputScheme)
        {
            this.key = key;
            this.filename = filename;
            this.inputScheme = inputScheme;

        }
        public string key;
        public string filename;
        public RCGLInputScheme inputScheme;
    }

    class PlayerInputSpinnerManager
    {
        private SpinItemMapEntry[] _inputMap = new SpinItemMapEntry[]
            {
                new SpinItemMapEntry("Keyboard",    "Keyboard", new RCGLKeyboardInputScheme()),
                new SpinItemMapEntry("Mouse",       "Mouse",    new RCGLMouseInputScheme()),
                new SpinItemMapEntry("GamePad1",    "GamePad1", new RCGLGamePadInputScheme(PlayerIndex.One)),
                new SpinItemMapEntry("GamePad2",    "GamePad2", new RCGLGamePadInputScheme(PlayerIndex.Two)),
                new SpinItemMapEntry("GamePad3",    "GamePad3", new RCGLGamePadInputScheme(PlayerIndex.Three)),
                new SpinItemMapEntry("GamePad4",    "GamePad4", new RCGLGamePadInputScheme(PlayerIndex.Four)),
                new SpinItemMapEntry("None",        "NoPlayer", null)
            };


        private BitmapFont _spinnerFont;
        private RCSpinner[] _playerSpinners;       

        public PlayerInputSpinnerManager()
        {
            
        }

        public RCGLInputScheme GetInputSchemeFromKey(string key)
        {
            RCGLInputScheme scheme = null;
            foreach (SpinItemMapEntry entry in _inputMap)
            {
                if (entry.key == key)
                {
                    scheme = entry.inputScheme;
                    break;
                }
            }

            return scheme;
        }

        public RCGLInputScheme GetPlayerInputScheme(RCNewGame.SpinnerIndex player)
        {
            return GetSpinnerInputScheme(_playerSpinners[(int)player]);
        }

        public RCGLInputScheme GetSpinnerInputScheme(RCSpinner spinner)
        {
            return GetInputSchemeFromKey(spinner.currentKey);
        }

        public RCGLInputScheme[] GetPlayerSchemes()
        {
            List<RCGLInputScheme> schemeList = new List<RCGLInputScheme>();

            foreach (RCSpinner spinner in _playerSpinners)
            {
                RCGLInputScheme inputScheme = GetSpinnerInputScheme(spinner);
                if (inputScheme != null)
                {
                    schemeList.Add(inputScheme);
                }
            }

            return schemeList.ToArray();
        }


        public void InitializeSpinners(RCSpinner[] playerSpinners, BitmapFont spinnerFont)
        {
            _playerSpinners = playerSpinners;
            _spinnerFont = spinnerFont;


            if (_playerSpinners.Length != 4)
            {
                throw new ArgumentException(
                    "Spinner array is not exactly 4 elements in size",
                    "playerSpinners"
                    );
            }


            int iCurrentSpinner = 0;
            foreach (RCSpinner spinner in _playerSpinners)
            {
                int iCurrentSpinItem = 0;
                foreach (SpinItemMapEntry entry in _inputMap)
                {
                    spinner.addSpinItem(entry.key, "", entry.filename, _spinnerFont);

                    // Set default spinner kry to coresponding map entry
                    if (iCurrentSpinItem == iCurrentSpinner)
                    {
                        spinner.spinTo(entry.key);
                    }

                    iCurrentSpinItem++;                     
                }

                iCurrentSpinner++;
            }
        }



        

        

        


    }
}
