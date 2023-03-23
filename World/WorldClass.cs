﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Moment.World.WorldObjects;
using AnotherLib.Utilities;
using System;
using Moment.Effects;
using AnotherLib;
using AnotherLib.Drawing;
using Moment.Utilities;

namespace Moment.World
{
    public class WorldClass
    {
        public static List<WorldObject> activeWorldObjects;
        public static WorldData activeWorldData;
        public static Tile[,] activeWorldChunk;
        public static WorldDetails worldDetails;

        public static int CurrentWorldWidth;
        public static int CurrentWorldHeight;

        public static void GenerateWorld(int width, int height, int baseCoordinate, int heightVariance, int heightThreshold)
        {
            Tile[,] worldTiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    worldTiles[x, y] = Tile.GetTileInfo(Tile.TileType.None, new Vector2(x, y) * 16f);
                }
            }
            Main.camera.bounds = new Vector2(width, height) * 16;

            int baseYCoordinate = height - baseCoordinate + Main.random.Next(-heightVariance, heightVariance + 1);
            int currentYCoordinate = baseYCoordinate;
            for (int x = 0; x < width; x++)
            {
                int segmentLength = Main.random.Next(4, 12);
                if (x + segmentLength > width)
                    segmentLength = width - x;

                for (int x2 = 0; x2 < segmentLength; x2++)
                {
                    Point tileCoordinates = new Point(x + x2, currentYCoordinate);
                    worldTiles[tileCoordinates.X, tileCoordinates.Y] = Tile.GetTileInfo((Tile.TileType)Main.random.Next((int)Tile.TileType.Floor_1, (int)Tile.TileType.Floor_2 + 1), tileCoordinates.ToVector2() * 16f);
                    for (int y = 1; y < height - currentYCoordinate; y++)
                    {
                        tileCoordinates.Y = height - y;
                        worldTiles[tileCoordinates.X, tileCoordinates.Y] = Tile.GetTileInfo(Tile.TileType.Ground, tileCoordinates.ToVector2() * 16f);
                    }
                }
                x += segmentLength - 1;
                currentYCoordinate += Main.random.Next(-heightVariance, heightVariance + 1);
                while (Math.Abs(baseYCoordinate - currentYCoordinate) >= heightThreshold)
                {
                    currentYCoordinate += Main.random.Next(-heightVariance, heightVariance + 1);
                }
            }

            WorldData worldData = new WorldData(0, width, height);
            worldData.tiles = worldTiles;
            activeWorldData = worldData;
            activeWorldObjects = new List<WorldObject>();
            CurrentWorldWidth = width;
            CurrentWorldHeight = height;
            GenerateExtraWorldFeatures(baseYCoordinate);
            ChunkLoader.ResetChunkDimensions();
            activeWorldChunk = new Tile[ChunkLoader.ChunkSizeWidth, ChunkLoader.ChunkSizeHeight];
            Main.currentPlayer.position = new Vector2(5 * 16, (baseYCoordinate - 4) * 16f);
            ChunkLoader.ForceUpdateActiveWorldChunk(Main.currentPlayer.position);
            worldDetails = new WorldDetails();
            worldDetails.Initialize();
        }

        public static void GenerateExtraWorldFeatures(int baseYCoordinate)
        {
            int amountOfAlarmPosts = Main.random.Next(128, 256 + 1);
            for (int i = 0; i < amountOfAlarmPosts; i++)
            {
                Point spawnPoint = new Point(Main.random.Next(0, CurrentWorldWidth), baseYCoordinate);
                while (ChunkLoader.CheckForSafeTileCoordinates(spawnPoint) && !(activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable && !activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable))
                {
                    if (activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable && activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable)
                        spawnPoint.Y -= 1;
                    else if (!activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable && !activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable)
                        spawnPoint.Y += 1;
                }

                activeWorldData.staticWorldObjects.Add(activeWorldData.staticWorldObjects.Count, AlarmTower.NewAlarmTower((spawnPoint.ToVector2() * 16f) + new Vector2(8f, 17f)));
            }

            int amountOfLampPosts = Main.random.Next(64, 128 + 1);
            for (int i = 0; i < amountOfLampPosts; i++)
            {
                Point spawnPoint = new Point(Main.random.Next(0, CurrentWorldWidth), baseYCoordinate);
                while (ChunkLoader.CheckForSafeTileCoordinates(spawnPoint) && !(activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable && !activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable))
                {
                    if (activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable && activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable)
                        spawnPoint.Y -= 1;
                    else if (!activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable && !activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable)
                        spawnPoint.Y += 1;
                }

                activeWorldData.staticWorldObjects.Add(activeWorldData.staticWorldObjects.Count, LampPost.NewLampPost((spawnPoint.ToVector2() * 16f) + new Vector2(8f, 17f)));
            }

            int amountOfBoomBoxes = Main.random.Next(12, 24 + 1);
            for (int i = 0; i < amountOfBoomBoxes; i++)
            {
                Point spawnPoint = new Point(Main.random.Next(0, CurrentWorldWidth), baseYCoordinate);
                while (ChunkLoader.CheckForSafeTileCoordinates(spawnPoint) && !(activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable && !activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable))
                {
                    if (activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable && activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable)
                        spawnPoint.Y -= 1;
                    else if (!activeWorldData.tiles[spawnPoint.X, spawnPoint.Y].isCollideable && !activeWorldData.tiles[spawnPoint.X, spawnPoint.Y + 1].isCollideable)
                        spawnPoint.Y += 1;
                }

                activeWorldData.staticWorldObjects.Add(activeWorldData.staticWorldObjects.Count, BoomBox.NewBoomBox((spawnPoint.ToVector2() * 16f) + new Vector2(0f, -16f)));
            }
        }

        public static void Update()
        {
            worldDetails.Update();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (activeWorldChunk == null)
                return;

            foreach (Tile tile in activeWorldChunk)
            {
                tile.Draw(spriteBatch);
            }
            foreach (WorldObject obj in activeWorldObjects)
                obj.Draw(spriteBatch);
        }

        public static void DrawDetails(SpriteBatch spriteBatch)
        {
            worldDetails.Draw(spriteBatch);
        }
    }
}
