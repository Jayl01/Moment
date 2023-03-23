using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Moment.Effects;
using Moment.Entities.Projectiles;
using Moment.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moment.Entities.Enemies
{
    public class Turret : Enemy
    {
        public static Texture2D turretTexture;

        private const float ShootRange = 12f * 16f;
        private const float ShootSpeed = 12f;

        private Vector2 center;
        private bool targetInRange = false;
        private int direction = 1;
        private int shootTimer = 0;
        private bool playedChargeSound = false;

        public override int EnemyWidth => 24;
        public override int EnemyHeight => 24;
        public override int EnemyHealth => 5;
        public override CollisionType collisionType => CollisionType.Enemies;
        public override CollisionType[] colliderTypes => new CollisionType[2] { CollisionType.Player, CollisionType.FriendlyProjectiles };

        public static void NewTurret(Vector2 position)
        {
            Turret newTurret = new Turret();
            newTurret.position = position;
            newTurret.Initialize();
            Main.activeEnemies.Add(newTurret);
        }

        public override void Initialize()
        {
            health = EnemyHealth;
            hitbox = new Rectangle((int)position.X, (int)position.Y, EnemyWidth, EnemyHeight);
            center = hitbox.Center.ToVector2();
            if (Main.random.Next(0, 1 + 1) == 0)
                direction = -1;
        }

        public override void Update()
        {
            targetInRange = Vector2.Distance(Main.currentPlayer.playerCenter, center) <= ShootRange;
            if (Main.currentPlayer.playerCenter.X > position.X)
                direction = 1;
            else
                direction = -1;

            if (!targetInRange)
            {
                shootTimer = 0;
                playedChargeSound = false;
            }
            else
            {
                shootTimer++;
                if (!playedChargeSound)
                {
                    SoundPlayer.PlaySoundFromOtherSource(Sounds.TurretShootCharge, position, 16, 0.8f);
                    playedChargeSound = true;
                }
                if (shootTimer >= 120)
                {
                    shootTimer = 100;
                    SoundPlayer.PlaySoundFromOtherSource(Sounds.PlayerShoot, center, 16, soundPitch: Main.random.Next(-10, -8 + 1) / 10f);
                    TurretLaser.NewLaser(center + new Vector2(6f * direction, -4f), new Vector2(ShootSpeed * direction, 0f));
                }
            }
        }

        public override void HurtEffects()
        {
            SoundPlayer.PlaySoundFromOtherSource(Sounds.TurretHurt, center, 12, soundPitch: Main.random.Next(-4, 4 + 1) / 10f);
        }

        public override void DeathEffects()
        {
            int amountOfBlackSmoke = Main.random.Next(24, 36 + 1);
            for (int i = 0; i < amountOfBlackSmoke; i++)
            {
                Vector2 smokePos = position + new Vector2(Main.random.Next(0, EnemyWidth), Main.random.Next(0, EnemyHeight));
                Vector2 smokeVelocity = new Vector2(Main.random.Next(-12, 12 + 1) / 10f, Main.random.Next(-16, -8 + 1) / 10f);
                Smoke.NewSmokeParticle(smokePos, smokeVelocity, Color.DarkGray, Color.DarkGray, 80, 80, 60);
            }

            int amountOfOrangeSmoke = Main.random.Next(20, 30 + 1);
            for (int i = 0; i < amountOfOrangeSmoke; i++)
            {
                Vector2 smokePos = position + new Vector2(Main.random.Next(0, EnemyWidth), Main.random.Next(0, EnemyHeight));
                Vector2 smokeVelocity = new Vector2(Main.random.Next(-24, 24 + 1) / 10f, Main.random.Next(-24, 24 + 1) / 10f);
                Smoke.NewSmokeParticle(smokePos, smokeVelocity, Color.Orange, Color.Red, 80, 80, 60);
            }

            int amountOfYellowSmoke = Main.random.Next(16, 24 + 1);
            for (int i = 0; i < amountOfYellowSmoke; i++)
            {
                Vector2 smokePos = position + new Vector2(Main.random.Next(0, EnemyWidth), Main.random.Next(0, EnemyHeight));
                Vector2 smokeVelocity = new Vector2(Main.random.Next(-36, 36 + 1) / 10f, Main.random.Next(-36, 36 + 1) / 10f);
                Smoke.NewSmokeParticle(smokePos, smokeVelocity, Color.Yellow, Color.OrangeRed, 40, 40, 20);
            }

            int amountOfGore = Main.random.Next(2, 5 + 1);
            for (int i = 0; i < amountOfGore; i++)
            {
                Vector2 gorePos = position + new Vector2(Main.random.Next(0, EnemyWidth), Main.random.Next(0, EnemyHeight / 2));
                Vector2 goreVelocity = new Vector2(Main.random.Next(-24, 24 + 1) / 10f, Main.random.Next(-16, -6 + 1) / 10f);
                Gore.NewGore(Main.random.Next(Gore.Turret_1, Gore.Turret_3 + 1), gorePos, goreVelocity);
            }
            SoundPlayer.PlaySoundFromOtherSource(Sounds.TurretExplosionSound, position, 16, 0.8f);
            if (Vector2.Distance(Main.currentPlayer.playerCenter, center) <= 16 * 16f)
                Main.camera.ShakeCamera((int)(6 * (1f - (Vector2.Distance(Main.currentPlayer.playerCenter, center) / (16f * 16f)))), (int)(30 * (1f - (Vector2.Distance(Main.currentPlayer.playerCenter, center) / (16f * 16f)))));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (direction == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(turretTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
        }
    }
}
