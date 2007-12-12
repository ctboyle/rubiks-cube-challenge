using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace RagadesCubeWin.SoundManagement
{
    public static class SoundManager
    {
        // needed to make sound
        private static AudioEngine audioengine;
        private static WaveBank wavebank;
        private static SoundBank soundbank;

        private static List<Cue> lstCue;

        public static void Initialize(string AudioEngineFile,
                            string WaveBankFile,
                            string SoundBankFile)
        {
            // sound related
            audioengine = new AudioEngine( AudioEngineFile);
            wavebank = new WaveBank(audioengine, WaveBankFile);
            soundbank = new SoundBank(audioengine, SoundBankFile);
            lstCue = new List<Cue>();
        }

        public static void Dispose()
        {
            audioengine.Dispose();
            wavebank.Dispose();
            soundbank.Dispose();
        }

        public static void PlaySound(string soundname)
        {
            soundbank.PlayCue(soundname);
        }

        public static void PlayCue(string soundname)
        {
            Cue cue = soundbank.GetCue(soundname);
            cue.Play();
            lstCue.Add(cue);
        }

        public static void Stop(string soundname)
        {
            
            foreach (Cue c in lstCue)
            {
                
                if (c.Name == soundname)
                {   
                    c.Stop(AudioStopOptions.Immediate);
                }
                
            }
        }

        public static void Stop()
        {
            foreach (Cue c in lstCue)
            {
                c.Stop(AudioStopOptions.Immediate);
            }
        }
    }
}
