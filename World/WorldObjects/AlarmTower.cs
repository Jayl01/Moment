using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Moment.World.WorldObjects
{
    public class AlarmTower : WorldObject
    {
        public static Texture2D alarmTowerTexture;

        private readonly Vector2 Origin = new Vector2(8, 112);

        public static AlarmTower NewAlarmTower(Vector2 position)
        {
            AlarmTower alarmTower = new AlarmTower();
            alarmTower.position = position;
            return alarmTower;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(alarmTowerTexture, position, null, Color.LightGray, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
