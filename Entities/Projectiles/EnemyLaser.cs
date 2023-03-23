using AnotherLib.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moment.Effects;
using Moment.Entities.Players;

namespace Moment.Entities.Projectiles
{
    public class EnemyLaser : Projectile
    {
        public static Texture2D laserTexture;

        private Vector2 velocity;
        private int lifeTimer = 0;
        private int direction;
        private readonly Point Size = new Point(7, 3);

        public override CollisionType collisionType => CollisionType.EnemyProjectiles;
        public override CollisionType[] colliderTypes => new CollisionType[1] { CollisionType.Player };

        public static void NewLaser(Vector2 position, Vector2 velocity)
        {
            EnemyLaser playerLaser = new EnemyLaser();
            playerLaser.position = position;
            playerLaser.velocity = velocity;
            playerLaser.Initialize();
            Main.activeProjectiles.Add(playerLaser);
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
            Smoke.NewSmokeParticle(position + new Vector2(Main.random.Next(0, Size.X + 1), Main.random.Next(0, Size.Y + 1)), -velocity * 0.02f, Color.Pink, Color.Red, 10, 10, 0);
            DetectCollisions(Main.activePlayers);
        }

        public override void HandleCollisions(CollisionBody collider, CollisionType colliderType)
        {
            Player player = collider as Player;
            player.AttemptHurt();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(laserTexture, position, Color.White);
        }
    }
}
