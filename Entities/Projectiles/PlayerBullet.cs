using AnotherLib.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moment.Effects;
using Moment.Entities.Enemies;

namespace Moment.Entities.Projectiles
{
    public class PlayerBullet : Projectile
    {
        public static Texture2D bulletTexture;

        private Vector2 velocity;
        private int lifeTimer = 0;
        private int direction;
        private readonly Point Size = new Point(7, 3);

        public override CollisionType collisionType => CollisionType.FriendlyProjectiles;
        public override CollisionType[] colliderTypes => new CollisionType[1] { CollisionType.Enemies };

        public static void NewBullet(Vector2 position, Vector2 velocity)
        {
            PlayerBullet playerBullet = new PlayerBullet();
            playerBullet.position = position;
            playerBullet.velocity = velocity;
            playerBullet.Initialize();
            Main.activeProjectiles.Add(playerBullet);
        }

        public override void Initialize()
        {
            if (velocity.X > 0)
                direction = 1;
            else
                direction = -1;
            hitbox = new Rectangle((int)position.X, (int)position.Y, Size.X, Size.Y);
        }

        public override void Update()
        {
            if (lifeTimer >= 6 * 60 || DetectTileCollisionsByCollisionStyle(position + new Vector2((Size.X / 2f) + ((Size.X / 2f) * direction) * 2, 0f)))
            {
                DestroyInstance();
                return;
            }

            position += velocity;
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            Smoke.NewSmokeParticle(position + new Vector2(Main.random.Next(0, Size.X + 1), Main.random.Next(0, Size.Y + 1)), -velocity * 0.02f, Color.Yellow, Color.Orange, 10, 10, 0);
            DetectCollisions(Main.activeEnemies);
        }

        public override void HandleCollisions(CollisionBody collider, CollisionType colliderType)
        {
            Enemy enemy = collider as Enemy;
            enemy.HurtEnemy(1);
            DestroyInstance();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, Color.White);
        }
    }
}
