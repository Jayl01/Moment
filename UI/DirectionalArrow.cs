using AnotherLib;
using AnotherLib.UI;
using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Moment.UI
{
    public class DirectionalArrow : UIObject
    {
        public static Texture2D arrowTexture;

        private Vector2 position;
        private float rotation;
        private bool hidden = true;
        private int shakeFactor = 0;
        private Color drawColor;
        private readonly Vector2 origin = new Vector2(25f / 2f);
        private static DirectionalArrow directionalArrow;

        public static void CreateDirectionalArrowInstance()
        {
            DirectionalArrow newDirectionalArrow = new DirectionalArrow();
            directionalArrow = newDirectionalArrow;
            Main.uiList.Add(newDirectionalArrow);
        }

        public static void ShowArrow(Vector2 direction)
        {
            Vector2 difference = (GameScreen.center / 3f) + (direction * 64f);
            directionalArrow.position = difference;
            directionalArrow.rotation = (float)Math.Atan2(difference.Y - (GameScreen.center / 3f).Y, difference.X - (GameScreen.center / 3f).X);
            directionalArrow.hidden = false;
            directionalArrow.shakeFactor++;
            float lerpValue = Math.Clamp((Main.TimesAttempted / (float)Main.MaxMinigameAttempts) - (1f / (float)Main.MaxMinigameAttempts), 0f, 1f);
            directionalArrow.drawColor = Color.Lerp(Color.White, Color.Red, lerpValue);
        }

        public static void HideArrow()
        {
            directionalArrow.hidden = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (hidden)
                return;

            Vector2 randomOffset = new Vector2(Main.random.Next(-shakeFactor / 3, (shakeFactor / 3) + 1), Main.random.Next(-shakeFactor / 3, (shakeFactor / 3) + 1));
            spriteBatch.Draw(arrowTexture, position + randomOffset, null, drawColor, rotation + (float)(Math.PI / 2f), origin, 3f, SpriteEffects.None, 0f);
        }
    }
}
