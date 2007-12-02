using System;
using System.Collections.Generic;
using System.Text;
using RC.Gui;
using System.Diagnostics;
using RC.Gui.Fonts;
using RagadesCube.GameLogic.InputSchemes;
using Microsoft.Xna.Framework;
using RC.Gui.Controls.Control_Subclasses;
using RagadesCube.States;

namespace RagadesCube.Misc
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
                new SpinItemMapEntry("None",        "NoPlayer", null),
                new SpinItemMapEntry("Keyboard",    "Keyboard", new RCGLKeyboardInputScheme()),
                new SpinItemMapEntry("Mouse",       "Mouse",    new RCGLMouseInputScheme()),
                new SpinItemMapEntry("GamePad1",    "GamePad1", new RCGLGamePadInputScheme(PlayerIndex.One)),
                new SpinItemMapEntry("GamePad2",    "GamePad2", new RCGLGamePadInputScheme(PlayerIndex.Two)),
                new SpinItemMapEntry("GamePad3",    "GamePad3", new RCGLGamePadInputScheme(PlayerIndex.Three)),
                new SpinItemMapEntry("GamePad4",    "GamePad4", new RCGLGamePadInputScheme(PlayerIndex.Four))
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

                    // Set default spinner key to first map entry.
                    if (iCurrentSpinItem == 0)
                    {
                        spinner.spinTo(_inputMap[0].key);        
                    }

                    iCurrentSpinItem++;                     
                }

                spinner.SelectionChange += OnSpinnerSelChange;

                iCurrentSpinner++;
            }
        }


        private void OnSpinnerSelChange(RCSpinner sender)
        {
            // Enable all the keys.
            foreach (RCSpinner spinner in _playerSpinners)
            {
                spinner.enableAllKeys();
            }

            // Disable currently shown items in each spinner from all ther spinners.
            foreach (RCSpinner spinner in _playerSpinners)
            {
                string currentSpinnerKey = spinner.currentKey;
                if (spinner.currentKey != "None")
                {
                    foreach (RCSpinner otherSpinner in _playerSpinners)
                    {
                        if (spinner == otherSpinner)
                        {
                            continue;
                        }

                        // Disable the key;
                        otherSpinner.enableKey(spinner.currentKey, false);
                    }
                }
            }
        }
        

        

        


    }
}
