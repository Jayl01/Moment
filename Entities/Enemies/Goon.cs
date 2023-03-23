using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moment.Effects;
using Moment.Entities.Projectiles;
using Moment.Utilities;

namespace Moment.Entities.Enemies
{
    public class Goon : Enemy
    {
        public static Texture2D goonIdleSpritesheet;
        public static Texture2D goonWalkSpritesheet;
        private Texture2D currentTexture;

        private const float DetectionDistance = 8f * 16f;
        private const float ShootRange = 6f * 16f;
        private const float ShootSpeed = 12f;
        private const float MoveSpeed = 1.1f;
        private const float GravityStrength = 0.18f;
        private const float MaxFallSpeed = 18f;
        private const float JumpStrength = 3.6f;

        private Vector2 center;
        private bool targetFound;
        private bool targetInRange = false;
        private int direction = 1;
        private int frame = 0;
        private int frameCounter = 0;
        private AnimState animState;
        private Rectangle animRect;
        private int shootTimer = 0;
        private float currentYVelocity = 0f;
        private bool playedChargeSound = false;
        private Vector2 deathVelocity;

        public override int EnemyWidth => 24;
        public override int EnemyHeight => 24;
        public override int EnemyHealth => 3;
        public override CollisionType collisionType => CollisionType.Enemies;
        public override CollisionType[] colliderTypes => new CollisionType[2] { CollisionType.Player, CollisionType.FriendlyProjectiles };

        private enum AnimState
        {
            Idle,
            Walk,
            Shooting
        }

        public static void NewGoon(Vector2 position)
        {
            Goon newGoon = new Goon();
            newGoon.position = position;
            newGoon.Initialize();
            Main.activeEnemies.Add(newGoon);
        }

        public override void Initialize()
        {
            health = EnemyHealth;
            currentTexture = goonIdleSpritesheet;
            hitbox = new Rectangle((int)position.X, (int)position.Y, EnemyWidth, EnemyHeight);
            animRect = new Rectangle(0, 0, EnemyWidth, EnemyHeight);
            if (Main.random.Next(0, 1 + 1) == 0)
                direction = -1;
        }

        public override void Update()
        {
            Vector2 velocity = Vector2.Zero;
            if (!targetFound)
            {
                if (Vector2.Distance(Main.currentPlayer.playerCenter, center) <= DetectionDistance)
                {
                    targetFound = true;
                    if (Main.currentPlayer.playerCenter.X > position.X)
                        direction = 1;
                    else
                        direction = -1;
                }
                else
                {
                    animState = AnimState.Idle;
                }
            }
            else
            {
                targetInRange = Vector2.Distance(Main.currentPlayer.playerCenter, center) <= ShootRange;
                if (Main.currentPlayer.playerCenter.X > position.X)
                    direction = 1;
                else
                    direction = -1;

                if (!targetInRange)
                {
                    shootTimer = 0;
                    velocity.X = direction * MoveSpeed;
                    if (DetectTileCollisionsByCollisionStyle(center + (velocity * 18f)))
                        currentYVelocity = -JumpStrength;
                    animState = AnimState.Walk;
                    playedChargeSound = false;
                }
                else
                {
                    shootTimer++;
                    animState = AnimState.Shooting;
                    if (!playedChargeSound)
                    {
                        playedChargeSound = true;
                        SoundPlayer.PlaySoundFromOtherSource(Sounds.GoonShootCharge, center, 16, 0.6f);
                    }
                    if (shootTimer >= 60)
                    {
                        shootTimer = 0;
                        SoundPlayer.PlaySoundFromOtherSource(Sounds.PlayerShoot, center, 12, soundPitch: Main.random.Next(4, 6 + 1) / 10f);
                        EnemyLaser.NewLaser(center + new Vector2(6f * direction, -1f), new Vector2(ShootSpeed * direction, 0f));
                        playedChargeSound = false;
                    }
                }
            }

            if (!tileCollisionDirection[CollisionDirection_Bottom])
            {
                if (currentYVelocity < MaxFallSpeed)
                    currentYVelocity += GravityStrength;
            }
            else
            {
                if (currentYVelocity > 0)
                    currentYVelocity = 0f;
            }

            velocity.Y = currentYVelocity;
            velocity = DetectTileCollisionsWithVelocity(velocity);
            position += velocity;
            hitbox.X = (int)(position.X + hitboxOffset.X);
            hitbox.Y = (int)(position.Y + hitboxOffset.Y);
            center = hitbox.Center.ToVector2();
            AnimateGoon();
        }

        private void AnimateGoon()
        {
            if (animState == AnimState.Idle)
            {
                if (currentTexture != goonIdleSpritesheet)
                    currentTexture = goonIdleSpritesheet;

                frameCounter++;
                if (frameCounter >= 15)
                {
                    frame += 1;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;

                    animRect.Y = frame * EnemyHeight;
                }
            }
            else if (animState == AnimState.Walk)
            {
                if (currentTexture != goonWalkSpritesheet)
                    currentTexture = goonWalkSpritesheet;

                frameCounter++;
                if (frameCounter >= 7)
                {
                    frame += 1;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;

                    animRect.Y = frame * EnemyHeight;
                    if (frame == 1 || frame == 3)
                        SoundPlayer.PlaySoundFromOtherSource(Main.random.Next(Sounds.Step_1, Sounds.Step_3 + 1), center, 12, soundPitch: Main.random.Next(-4, 4 + 1) / 10f);
                }
            }
            else if (animState == AnimState.Shooting)
            {
                if (currentTexture != goonIdleSpritesheet)
                    currentTexture = goonIdleSpritesheet;

                frame = 0;
                frameCounter = 0;
                animRect.Y = 0;
            }
        }

        public override void HurtEffects()
        {
            deathVelocity = new Vector2(4f * -direction, currentYVelocity);
            deathVelocity.X *= 0.1f;
            SoundPlayer.PlaySoundFromOtherSource(Main.random.Next(Sounds.Goon_Hurt1, Sounds.Goon_Hurt2 + 1), center, 12, soundPitch: Main.random.Next(-4, 4 + 1) / 10f);
        }

        public override void DeathEffects()
        {
            Gore.NewGore(Gore.Goon, center + new Vector2(0f, -16f), deathVelocity);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (direction == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(currentTexture, position, animRect, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
        }
    }
}
