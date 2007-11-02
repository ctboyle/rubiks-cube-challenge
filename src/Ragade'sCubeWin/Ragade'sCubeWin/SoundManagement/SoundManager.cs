using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace RagadesCubeWin.SoundManagement
{
    class SoundManager
    {
        // needed to make sound
        private static AudioEngine audioengine;
        private static WaveBank wavebank;
        private static SoundBank soundbank;

        public SoundManager()
        {
            // sound related
            audioengine = new AudioEngine(@"Content/Sounds/RCSoundBank.xgs");
            wavebank = new WaveBank(audioengine, @"Content/Sounds/wave bank.xwb");
            soundbank = new SoundBank(audioengine, @"Content/Sounds/sound bank.xsb");
        }

        public void PlaySound(string soundname)
        {
            soundbank.PlayCue("notify");
        }
    }
}
