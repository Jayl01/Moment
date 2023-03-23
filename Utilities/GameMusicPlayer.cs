using AnotherLib.Utilities;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Moment.Utilities
{
    public class GameMusicPlayer
    {
        public static SoundEffectInstance theme1;
        public static SoundEffectInstance theme2;

        public static void Update()
        {
            if (theme1.State == SoundState.Stopped)
            {
                theme1.Play();
                theme2.Play();
            }
            float volume1 = 0.4f * (1f - (Main.TimesAttempted / (float)Main.MaxMinigameAttempts));
            volume1 = Math.Clamp(volume1, 0f, 1f);
            float volume2 = 0.6f * (Main.TimesAttempted / (float)Main.MaxMinigameAttempts);
            volume2 = Math.Clamp(volume2, 0f, 1f);
            theme1.Volume = volume1;
            theme2.Volume = volume2;
        }

        public static void StopMusic()
        {
            theme1.Stop();
            theme2.Stop();
        }
    }
}
