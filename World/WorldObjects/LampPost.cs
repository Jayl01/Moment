using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Moment.World.WorldObjects
{
    public class LampPost : WorldObject
    {
        public static Texture2D lampPostTexture;

        private readonly Vector2 Origin = new Vector2(8, 80);

        public static LampPost NewLampPost(Vector2 position)
        {
            LampPost lampPost = new LampPost();
            lampPost.position = position;
            return lampPost;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(lampPostTexture, position, null, Color.LightGray, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
