using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using CustomTilemap;
using Godot;
using ldtk;
using MoonTools.ECS;
using MyECS.Components;
public class LoadLevelJSON
{
    MoonTools.ECS.World World;
    // LDTKJsonObject jsonObject;
    LdtkJson jsonObject;
    Vector2I tileSize;
    TileMap gdTilemap;
    Tilemap tilemap;
    string[] tileIDToSlopeName;
    Vector2I[] tileIDToSlopeTile;
    Dictionary<string, Vector2I> slopeNameToTile = new Dictionary<string, Vector2I>();

    int maxTileCount;
    int slopeTileSourceID;
    public LoadLevelJSON(MoonTools.ECS.World world, Vector2I tileSize, TileMap gdTilemap)
    {
        World = world;
        this.tileSize = tileSize;
        this.gdTilemap = gdTilemap;
    }
    public void ReadFile(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            GD.Print("No such file: " + filePath);
            return;
        }
        string contents = File.ReadAllText(filePath);

        jsonObject = LdtkJson.FromJson(contents);
        GD.Print($" levels count: {jsonObject.Levels.Length}");
        InitTilemap();

        var atlasSource = GetAtlasByName("collision");
        ReadCollisionTiles(atlasSource);

        InitTileIDToSlope();
    }
    TileSetAtlasSource GetAtlasByName(string name)
    {
        var tileset = gdTilemap.TileSet;
        int numSources = tileset.GetSourceCount();
        for (int i = 0; i < numSources; i++)
        {
            int sourceID = tileset.GetSourceId(i);
            TileSetSource source = tileset.GetSource(sourceID);
            if (source is TileSetAtlasSource atlasSource && atlasSource.ResourceName == name)
            {
                slopeTileSourceID = sourceID;
                return atlasSource;
            }
        }
        return null;
    }
    void ReadCollisionTiles(TileSetAtlasSource atlasSource)
    {
        Vector2I size = atlasSource.GetAtlasGridSize();
        for (int x = 0; x < size.X; x++)
        {
            for (int y = 0; y < size.Y; y++)
            {
                var tilePos = new Vector2I(x, y);
                if (!atlasSource.HasTile(tilePos))
                {
                    continue;
                }
                TileData tileData = atlasSource.GetTileData(tilePos, 0);
                if (tileData != null)
                {
                    string collisionName = (string)tileData.GetCustomData("slope");
                    slopeNameToTile[collisionName] = tilePos;
                }
            }
        }
    }
    public void InitTilemap()
    {
        maxTileCount = 0;
        foreach (Level level in jsonObject.Levels)
        {
            foreach (LayerInstance layerInstance in level.LayerInstances)
            {
                maxTileCount = Mathf.Max(maxTileCount, (int)(layerInstance.CHei * layerInstance.CWid));
            }
        }
        tilemap = new Tilemap(maxTileCount, tileSize);
        LevelInfo.tilemap = tilemap;
    }
    void InitTileIDToSlope()
    {
        foreach (TilesetDefinition definition in jsonObject.Defs.Tilesets)
        {
            if (definition.Identifier == "Industry8")
            {
                int size = (int)(definition.CWid * definition.CHei);
                tileIDToSlopeName = new string[size];
                tileIDToSlopeTile = new Vector2I[size];
                foreach (var enumTag in definition.EnumTags)
                {
                    string name = enumTag.EnumValueId;
                    foreach (int tileID in enumTag.TileIds)
                    {
                        if (tileIDToSlopeName[tileID] != "")
                        {
                            GD.Print($"overwriting existing name with {name}");
                        }
                        tileIDToSlopeName[tileID] = name;
                        tileIDToSlopeTile[tileID] = slopeNameToTile[name];
                    }
                }
            }
        }
    }
    public void ReadLevel(int id)
    {
        if (jsonObject.Levels.Length <= id)
        {
            GD.Print("Level out of bounds!");
        }
        else
        {
            ReadLevel(jsonObject.Levels[id]);
        }
    }
    void ReadLevel(Level level)
    {
        foreach (FieldInstance fieldInstance in level.FieldInstances)
        {

        }
        gdTilemap.Clear();

        LayerInstance layerInstance = GetLayerInstanceByName(level, "Ground");

        tilemap.Resize((int)layerInstance.CWid, (int)layerInstance.CHei);
        tilemap.ClearSprites();
        LoadTileSpritesFromTileLayer(layerInstance);

        layerInstance = GetLayerInstanceByName(level, "Entities");
        foreach (EntityInstance entityInstance in layerInstance.EntityInstances)
        {
            ReadEntity(entityInstance);
        }

    }
    LayerInstance GetLayerInstanceByName(Level level, string name)
    {
        foreach (LayerInstance layerInstance in level.LayerInstances)
        {
            if (layerInstance.Identifier == name)
            {
                return layerInstance;
            }
        }
        return null;
    }
    void LoadTileSpritesFromTileLayer(LayerInstance layerInstance)
    {
        foreach (var tile in layerInstance.GridTiles)
        {
            Vector2I spritePosOnSheet = new Vector2I((int)tile.Src[0], (int)tile.Src[1]);
            Vector2I tilePos = new Vector2I((int)tile.Px[0] / tileSize.X, (int)tile.Px[1] / tileSize.Y);
            tilemap.SetSprite((int)tile.Px[0] / tileSize.X, (int)tile.Px[1] / tileSize.Y, spritePosOnSheet);

            // load tile colliders from tile layer
            int id = (int)tile.T;
            Vector2I atlasTilePos = tileIDToSlopeTile[id];
            gdTilemap.SetCell(0, tilePos, slopeTileSourceID, atlasTilePos);
        }
    }

    void ReadEntity(EntityInstance entityInstance)
    {
        Vector2I position = new Vector2I((int)entityInstance.Px[0], (int)entityInstance.Px[1]) + tileSize / 2;
        GD.Print($"read entity at {position}");
        switch (entityInstance.Identifier)
        {
            case "Player":
                {
                    EntityPrefabs.CreatePlayer(World, position);
                    break;
                }
            case "JumpEnemy":
                {
                    EntityPrefabs.CreateJumpingEnemy(World, position);
                    break;
                }
            case "WalkEnemy":
                {
                    EntityPrefabs.CreateWalkingEnemy(World, position);
                    break;
                }
            case "Door":
                {
                    EntityPrefabs.CreateDoor(World, position, (int)entityInstance.FieldInstances[0].Value);
                    break;
                }
            case "Thing":
                {

                    ThingType t = NameToThing((string)entityInstance.FieldInstances[0].Value);
                    SpawnFacing spawnFacing = NameToFacing((string)entityInstance.FieldInstances[4].Value);
                    GD.Print($"Creating thing of type {t}");
                    Vector2I ambushPoint = default;
                    bool hasAmbushPoint = false;

                    if (entityInstance.FieldInstances[1].Value != null)
                    {
                        ambushPoint = new Vector2I((int)entityInstance.FieldInstances[1].Value.cx, (int)entityInstance.FieldInstances[1].Value.cy);
                        hasAmbushPoint = true;
                    }
                    string ambushTrigger = entityInstance.FieldInstances[2].Value;
                    string spawnType = entityInstance.FieldInstances[3].Value;
                    if (!hasAmbushPoint)
                    {
                        EntityPrefabs.CreateThing(World, t, position, spawnFacing);
                        return;
                    }
                    if (spawnType == "HideInPlainSight")
                    {
                        var entity = EntityPrefabs.CreateThing(World, t, position, spawnFacing);
                        switch (ambushTrigger)
                        {
                            case "X":
                                World.Set(entity, new AmbushOnPosition(position.X, false));
                                break;
                            case "Y":
                                World.Set(entity, new AmbushOnPosition(position.Y, true));
                                break;
                        }
                        return;
                    }
                    //Vector2I ambushPoint = entityInstance.FieldInstances[1].Value; // ambush point
                    // Entity ambushEntity = AmbushPrefabs.CreateVertical(World, t, point, position);
                    GD.Print("creating ambush!");
                    switch (ambushTrigger)
                    { //ambush type

                        case "X":
                            AmbushPrefabs.CreateX(World, t, ambushPoint * tileSize, position, spawnFacing);
                            break;
                        case "Y":
                            AmbushPrefabs.CreateY(World, t, ambushPoint * tileSize, position, spawnFacing);
                            break;
                    }

                    break;
                }

        }
    }
    static ThingType NameToThing(string name)
    {
        return name switch
        {
            "Bird" => ThingType.Bird,
            "Bomb" => ThingType.Bomb,
            "Radish" => ThingType.Radish,
            "Bush" => ThingType.Bush,
            "Ghost" => ThingType.Ghost,
            _ => ThingType.Bomb
        };
    }
    static SpawnFacing NameToFacing(string name)
    {
        return name switch
        {
            "Left" => SpawnFacing.Left,
            "Right" => SpawnFacing.Right,
            "FacePlayer" => SpawnFacing.FacePlayer,
            _ => SpawnFacing.FacePlayer
        };
    }
}
