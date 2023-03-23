using AnotherLib.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Moment.Utilities
{
    public class Shaders
    {
        public static Effect greyscaleEffect;
        public static Effect gradientEffect;
        private static readonly Color bottomGradientColor = new Color(59, 13, 81);
        private static readonly Color topGradientColor = new Color(25, 4, 56);


        public static void DrawAllScreenEffectShaders(RenderTarget2D gameTarget)
        {
            if (Main.InDodgeMinigame)
            {
                ShaderBatch.DrawScreenShader(gameTarget, greyscaleEffect);
            }

            gradientEffect.Parameters["bottomColor"].SetValue(bottomGradientColor.ToVector3());
            gradientEffect.Parameters["topColor"].SetValue(topGradientColor.ToVector3());
        }
    }
}
