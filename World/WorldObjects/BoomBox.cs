using AnotherLib;
using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Moment.Utilities;

namespace Moment.World.WorldObjects
{
    public class BoomBox : WorldObject
    {
        public static Texture2D boomBoxTexture;
        public static SoundEffect boomBoxSong;

        public TrackedSoundEffectInstance radioTheme;
        public int soundType = 0;
        public int soundTimer = 0;

        public static BoomBox NewBoomBox(Vector2 position)
        {
            BoomBox boomBox = new BoomBox();
            boomBox.position = position;
            boomBox.Initialize();
            return boomBox;
        }

        public override void Initialize()
        {
            soundType = Main.random.Next(0, 1 + 1);
            if (soundType == 0)
            {
                radioTheme = TrackedSoundEffectInstance.CreateTrackedSound(boomBoxSong.CreateInstance(), position + new Vector2(16f), true, 4 * 16, 0.75f);
                radioTheme.soundInstance.Play();
                radioTheme.soundInstance.Volume = 0f;
            }
        }

        public override void Update()
        {
            if (soundType == 0)
            {
                radioTheme.UpdateAudioInformation();
                if (radioTheme.soundInstance.State == SoundState.Stopped)
                    radioTheme.soundInstance.Play();
            }
            else
            {
                soundTimer++;
                if (soundTimer >= 60 * (1f - ((Main.TimesAttempted / (float)Main.MaxMinigameAttempts) / 2f)))
                {
                    SoundPlayer.PlaySoundFromOtherSource(Main.random.Next(Sounds.One, Sounds.Four + 1), position + new Vector2(16f), 4, 0.2f);
                    soundTimer = 0;
                }
            }
        }

        public override void ClearRemainingData()
        {
            if (radioTheme == null)
                return;

            if (radioTheme.soundInstance.State != SoundState.Stopped)
                radioTheme.soundInstance.Stop();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(boomBoxTexture, position, Color.White);
        }
    }
}
