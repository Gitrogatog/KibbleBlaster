namespace CustomTilemap;
using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
// using MyECS.Prefabs;
// using Utils;

public class TilemapReader
{
    TileMap levelData;
    World world;
    const int EntityLayerID = 1;
    // Dictionary<string, int> nameToID;
    public TilemapReader(TileMap levelData, World world) // , Dictionary<string, int> nameToID
    {
        this.levelData = levelData;
        this.world = world;
        // this.nameToID = nameToID;
    }
    public void ReadEntityLayer()
    {
        Rect2I usedRect = levelData.GetUsedRect();
        Vector2I startCorner = usedRect.Position;
        Vector2I endCorner = usedRect.End;
        int xTotalTiles = endCorner.X - startCorner.X;
        int yTotalTiles = endCorner.Y - startCorner.Y;
        // GD.Print($"start: {startCorner}, end: {endCorner}");
        // Tilemap tilemap = new Tilemap(xTotalTiles, yTotalTiles);
        int tileIndex = 0;
        for (int y = startCorner.Y; y < endCorner.Y; y++)
        {
            for (int x = startCorner.X; x < endCorner.X; x++)
            {
                // ReadTile(tileIndex, new Vector2I(x, y), tilemap);
                // Vector2I tilemapPos = tilemap.id_to_xy(tileIndex);
                // ReadTile(tileIndex, new Vector2I(x, y), tilemap);
                ReadEntityTile(EntityLayerID, tileIndex, new Vector2I(x, y));

                tileIndex++;
            }
        }
        levelData.SetLayerEnabled(EntityLayerID, false);
    }

    void ReadEntityTile(int layerIndex, int tileIndex, Vector2I levelPos)
    {
        TileData tileData = levelData.GetCellTileData(layerIndex, levelPos);
        if (tileData != null)
        {
            string tileTypeMeta = (string)tileData.GetCustomDataByLayerId(0);
            Vector2 localPos = levelData.MapToLocal(levelPos);
            Entity entity;
            // GD.Print($"tile type: {tileTypeMeta}");
            // GD.Print($"contains: {nameToID.ContainsKey(tileTypeMeta)}");
            if (tileTypeMeta == "player")
            {
                entity = EntityPrefabs.CreatePlayer(world, localPos);
            }
            else if (tileTypeMeta == "enemy0")
            {
                GD.Print("created enemy!");
                entity = EntityPrefabs.CreateJumpingEnemy(world, localPos);
            }
            // else if (nameToID.ContainsKey(tileTypeMeta))
            // {
            //     int npcID = nameToID[tileTypeMeta];
            //     int dialogID = (int)tileData.GetCustomData(TileStringName.dialog);
            //     entity = TileEntityPrefabs.CreateNPC(world, npcID, dialogID, tilemapPos);
            //     int dialogMeta = (int)tileData.GetCustomData(TileStringName.dialog);
            //     if (dialogMeta >= 0)
            //     {
            //         world.Set(entity, new HasDialog(dialogMeta));
            //     }
            // }

        }
    }
}