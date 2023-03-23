using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Moment.Effects;
using Moment.Entities.Players;
using Moment.World;
using Moment.Entities.Projectiles;
using Moment.UI;
using Moment.Entities.Enemies;
using Microsoft.Xna.Framework.Audio;
using Moment.World.WorldObjects;

namespace Moment.Utilities
{
    public class AssetLoader : ContentLoader
    {
        private static ContentLoader contentLoader;
        private static AssetLoader assetLoader;

        public AssetLoader(ContentManager content) : base(content)
        {
            contentManager = content;
        }

        public static void LoadAssets(ContentManager content)
        {
            assetLoader = new AssetLoader(content);
            Main.gameFont = assetLoader.LoadFont("MainFont");
            Shaders.greyscaleEffect = content.Load<Effect>("Effects/Greyscale");
            Shaders.gradientEffect = content.Load<Effect>("Effects/Gradient");
            assetLoader.LoadTextures();
            assetLoader.LoadSounds();
        }

        private void LoadTextures()
        {
            Tile.tileTextures = new Dictionary<Tile.TileType, Texture2D>();
            Tile.tileTextures.Add(Tile.TileType.Ground, LoadTex("Tiles/Ground"));
            Tile.tileTextures.Add(Tile.TileType.Floor_1, LoadTex("Tiles/Floor_1"));
            Tile.tileTextures.Add(Tile.TileType.Floor_2, LoadTex("Tiles/Floor_2"));

            WorldDetails.moonTexture = LoadTex("Environment/Moon");

            Gore.goreTextures = new Texture2D[Gore.AmountOfGoreTextures];
            Gore.goreTextures[Gore.Turret_1] = LoadTex("Gore/Turret_1");
            Gore.goreTextures[Gore.Turret_2] = LoadTex("Gore/Turret_2");
            Gore.goreTextures[Gore.Turret_3] = LoadTex("Gore/Turret_3");
            Gore.goreTextures[Gore.Goon] = LoadTex("Gore/Goon");

            Player.playerIdleSpritesheet = LoadTex("Player/Player_Idle");
            Player.playerWalkSpritesheet = LoadTex("Player/Player_Walk");
            Player.playerJumpFrame = LoadTex("Player/Player_Jumping");
            Player.playerFallFrame = LoadTex("Player/Player_Falling");

            Goon.goonIdleSpritesheet = LoadTex("Enemies/Goon_Idle");
            Goon.goonWalkSpritesheet = LoadTex("Enemies/Goon_Walk");
            Turret.turretTexture = LoadTex("Enemies/Turret");

            PlayerBullet.bulletTexture = LoadTex("Projectiles/FriendlyBullet");
            EnemyLaser.laserTexture = LoadTex("Projectiles/EnemyBullet");
            TurretLaser.laserTexture = LoadTex("Projectiles/TurretBullet");

            DirectionalArrow.arrowTexture = LoadTex("UI/DirectionalArrow");

            AlarmTower.alarmTowerTexture = LoadTex("Environment/AlarmTower");
            LampPost.lampPostTexture = LoadTex("Environment/LampPost");
            BoomBox.boomBoxTexture = LoadTex("Environment/BoomBox");

            PlayerUI.playerIndicatorTexture = LoadTex("UI/PlayerIndicator");

            Smoke.smokePixelTextures = new Texture2D[1];
            Smoke.smokePixelTextures[Smoke.WhitePixelTexture] = TextureGenerator.CreatePanelTexture(2, 2, 1, Color.White, Color.White, false);
        }

        private void LoadSounds()
        {
            SoundPlayer.sounds = new SoundEffect[20];
            SoundPlayer.sounds[Sounds.Step_1] = LoadSFX("SFX/Step_1");
            SoundPlayer.sounds[Sounds.Step_2] = LoadSFX("SFX/Step_2");
            SoundPlayer.sounds[Sounds.Step_3] = LoadSFX("SFX/Step_3");
            SoundPlayer.sounds[Sounds.PlayerJump] = LoadSFX("SFX/PlayerJump");
            SoundPlayer.sounds[Sounds.PlayerShoot] = LoadSFX("SFX/PlayerShoot");
            SoundPlayer.sounds[Sounds.Goon_Hurt1] = LoadSFX("SFX/Goon_Hurt_1");
            SoundPlayer.sounds[Sounds.Goon_Hurt2] = LoadSFX("SFX/Goon_Hurt_2");
            SoundPlayer.sounds[Sounds.GameEnd] = LoadSFX("SFX/DeathSound");
            SoundPlayer.sounds[Sounds.GoonShootCharge] = LoadSFX("SFX/ShotCharge_1");
            SoundPlayer.sounds[Sounds.TurretShootCharge] = LoadSFX("SFX/ShotCharge_2");
            SoundPlayer.sounds[Sounds.TurretHurt] = LoadSFX("SFX/Turret_Hit");
            SoundPlayer.sounds[Sounds.WrongDirectionSound] = LoadSFX("SFX/WrongDirectionSound");
            SoundPlayer.sounds[Sounds.TimeOutSound] = LoadSFX("SFX/TimeOutSound");
            SoundPlayer.sounds[Sounds.TurretExplosionSound] = LoadSFX("SFX/TurretExplosion");
            SoundPlayer.sounds[Sounds.PlayerDash] = LoadSFX("SFX/DashSound");
            SoundPlayer.sounds[Sounds.AttemptHit] = LoadSFX("SFX/AttemptHitSound");
            SoundPlayer.sounds[Sounds.DirectionalSound] = LoadSFX("SFX/DirectionalAppear");
            SoundPlayer.sounds[Sounds.One] = LoadSFX("SFX/One");
            SoundPlayer.sounds[Sounds.Three] = LoadSFX("SFX/Three");
            SoundPlayer.sounds[Sounds.Four] = LoadSFX("SFX/Four");

            GameMusicPlayer.theme1 = LoadSFX("Music/Theme_1").CreateInstance();
            GameMusicPlayer.theme2 = LoadSFX("Music/Theme_2").CreateInstance();
            BoomBox.boomBoxSong = LoadSFX("Music/RadioTheme");
        }
    }
}
