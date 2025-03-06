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
        // using FileStream fileStream = File.OpenRead(filePath);
        // jsonObject = JsonSerializer.Deserialize<LDTKJsonObject>(fileStream);
        string contents = File.ReadAllText(filePath);

        jsonObject = LdtkJson.FromJson(contents);
        GD.Print($" levels count: {jsonObject.Levels.Length}");
        InitTilemap();

        var atlasSource = GetAtlasByName("collision");
        ReadCollisionTiles(atlasSource);

        InitTileIDToSlope();


        // Level level0 = jsonObject.Levels[0];
        // ReadLevel(level0);
        // GD.Print($"number of levels: {result.levels.Length}");
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
                    // GD.Print($"{collisionName} at pos {tilePos}");
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
        // tileID = new string[maxTileCount];
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
            // switch(fieldInstance.Identifier){
            //     case "Path0":
            //         break;
            //     case "Path1":
            //         break;
            //     case "Path2":
            //         break;
            //     case 
            // }
        }
        // bool tilemapResized = false;
        // foreach (ldtk.LayerInstance layerinstance in level.LayerInstances)
        // {
        //     if (!tilemapResized && layerinstance.Identifier != "BackgroundGrid")
        //     {

        //         tilemapResized = true;
        //     }

        // }
        gdTilemap.Clear();
        // foreach (LayerInstance layerInstance in level.LayerInstances)
        // {
        //     switch (layerInstance.Identifier)
        //     {

        //     }
        // }
        LayerInstance layerInstance = GetLayerInstanceByName(level, "Ground");

        tilemap.Resize((int)layerInstance.CWid, (int)layerInstance.CHei);
        tilemap.ClearSprites();
        LoadTileSpritesFromTileLayer(layerInstance);
        // LoadTileCollidersFromTileLayer(layerInstance);

        layerInstance = GetLayerInstanceByName(level, "Entities");
        foreach (EntityInstance entityInstance in layerInstance.EntityInstances)
        {
            ReadEntity(entityInstance);
        }

        // LayerInstance layerInstance = GetLayerInstanceByName(level, "Ground");
        // // tilemap.Resize((int)layerInstance.CWid, (int)layerInstance.CHei);
        // LoadTilemapLayerFromTileLayer(layerInstance);
        // layerInstance = GetLayerInstanceByName(level, "Entities");
        // foreach (ldtk.EntityInstance entityinstance in layerInstance.EntityInstances)
        // {
        //     GD.Print("Reading one entity");
        //     ReadEntity(entityinstance);
        // }
        // layerInstance = GetLayerInstanceByName(level, "ChunkyTiles");
        // SetUniqueTileSprites(layerInstance);
        // LogPoints();
        // LogPaths();
        // PathData.LogPaths();

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
    // void LoadTileCollidersFromTileLayer(LayerInstance layerInstance)
    // {
    //     foreach (var tile in layerInstance.GridTiles)
    //     {
    //         int id = (int)tile.T;
    //         string slopeName = tileIDToSlope[id];

    //     }
    // }
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
                { //the casual revolution

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
                // case "House":
                //     {
                //         int level = (int)entityInstance.FieldInstances[0].Value;
                //         EntityPrefabs.CreateHouse(World, position, level);
                //         break;
                //     }
                // case "ExitTile":
                //     {
                //         EntityPrefabs.CreateExitTile(World, position);
                //         break;
                //     }

        }
        // EntityType entityType = entityInstance.Identifier switch
        // {
        //     "Player" => EntityType.Player,
        //     "Coin" => EntityType.Coin,
        //     "Goomba" => EntityType.Goomba,
        //     "Door" => EntityType.Door,
        //     "WalkingGoomba" => EntityType.WalkingGoomba,
        //     "PathGoomba" => EntityType.PathGoomba,
        //     "Thwomp" => EntityType.Thwomp,
        //     "Box" => EntityType.Box,

        //     _ => EntityType.Blank

        // };
        // var origin = EntityPrefabs.CreateOrigin(World, position, entityType);
        // foreach (FieldInstance fieldInstance in entityInstance.FieldInstances)
        // {
        //     switch (fieldInstance.Identifier)
        //     {
        //         case "Path":
        //             int path = (int)fieldInstance.Value;
        //             World.Set(origin, new FollowPath(path));
        //             break;
        //         case "InitialTargetPoint":
        //             int pointID = (int)fieldInstance.Value;
        //             World.Set(origin, new TargetPathPoint(pointID));
        //             break;
        //         case "TileDir":
        //             World.Set(origin, new MovementDirection(TileUtils.StringToTileDir((string)fieldInstance.Value)));
        //             break;
        //         case "SmashDir":
        //             World.Set(origin, new SmashDirection());
        //             break;
        //         case "MoveSpeed":
        //             float speed = (float)fieldInstance.Value;
        //             World.Set(origin, new HopSpeed(speed));
        //             break;
        //     }
        // }
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
    // void ReadPathPoint(EntityInstance entityinstance)
    // {
    //     Vector2I position = new Vector2I((int)entityinstance.Grid[0], (int)entityinstance.Grid[1]);
    //     int id = (int)entityinstance.FieldInstances[0].Value;
    //     pathPoints[id] = position;
    // }
    // void ReadPath(EntityInstance entityInstance)
    // {
    //     int pathID = (int)entityInstance.FieldInstances[0].Value;
    //     GD.Print($"path id beaing read: {pathID}");
    //     // int i = (int)v1.ToObject(typeof(int));
    //     int[] pointIDs = entityInstance.FieldInstances[1].Value.ToObject(typeof(int[]));
    //     SimpleList<int> path = paths[pathID];
    //     path.SetLength(pointIDs.Length);
    //     for (int i = 0; i < pointIDs.Length; i++)
    //     {
    //         path[i] = pointIDs[i];
    //     }
    // }
    // void SetUniqueTileSprites(LayerInstance layerInstance)
    // {
    //     int numTiles = tilemap.totalTiles;
    //     foreach (var tile in layerInstance.GridTiles)
    //     {
    //         int id = tilemap.xy_id((int)tile.Px[0] / tileSize.X, (int)tile.Px[1] / tileSize.Y);
    //         // if (tile.A < 0.5)
    //         // {
    //         //     tilemap.tiles[id] = TileType.Empty;
    //         //     continue;
    //         // }
    //         Vector2I startPos = new Vector2I((int)tile.Src[0], (int)tile.Src[1]);
    //         tilemap.tileSprites[id] = new Rect2I(startPos, tileSize);
    //     }
    // }
    // void CreateTilemapFromIntGrid(LayerInstance layerInstance)
    // {
    //     // tilemap.Resize((int)layerinstance.CWid, (int)layerinstance.CHei);
    //     int numTiles = tilemap.tiles.Length;
    //     GD.Print($"tile array length {numTiles}, totalTiles {tilemap.totalTiles}");
    //     GD.Print($"intgridlength: {layerInstance.IntGridCsv.Length}");
    //     GD.Print($"autolayer length: {layerInstance.AutoLayerTiles.Length}");
    //     for (int i = 0; i < numTiles; i++)
    //     {
    //         TileType tileType = IntGridToTileType((int)layerInstance.IntGridCsv[i]);
    //         tilemap.tiles[i] = tileType;
    //         tilemap.colors[i] = tileType switch
    //         {
    //             TileType.Field => StaticSprite.colors[2],
    //             TileType.Forest => StaticSprite.colors[2],
    //             TileType.Road => StaticSprite.colors[4],
    //             TileType.Mountain => StaticSprite.colors[4],
    //             TileType.Water => StaticSprite.colors[1],
    //             TileType.Brick => StaticSprite.colors[6],
    //             TileType.FloorBrick => StaticSprite.colors[7],
    //             TileType.Swamp => StaticSprite.colors[2],
    //             _ => Colors.White
    //         };
    //         tilemap.tileSprites[i] = default;
    //         // var AutoLayerTiles
    //         // Vector2I startPos = new Vector2I((int)layerinstance.AutoLayerTiles[i].Src[0], (int)layerinstance.AutoLayerTiles[i].Src[1]);
    //         // tilemap.tileSprites[i] = new Rect2I(startPos, tileSize);
    //     }
    //     foreach (var tile in layerInstance.AutoLayerTiles)
    //     {
    //         int id = tilemap.xy_id((int)tile.Px[0] / tileSize.X, (int)tile.Px[1] / tileSize.Y);
    //         if (tile.A < 0.5)
    //         {
    //             tilemap.tiles[id] = TileType.Empty;
    //             continue;
    //         }

    //         Vector2I startPos = new Vector2I((int)tile.Src[0], (int)tile.Src[1]);
    //         tilemap.tileSprites[id] = new Rect2I(startPos, tileSize);
    //     }
    //     tilemap.PopulateBlocked();
    //     tilemap.LogTiles();
    //     // return tilemap;
    // }
    // static TileType IntGridToTileType(int id)
    // {
    //     return id switch
    //     {
    //         0 => TileType.Empty,
    //         1 => TileType.Field,
    //         2 => TileType.Forest,
    //         3 => TileType.Road,
    //         4 => TileType.Mountain,
    //         5 => TileType.Water,
    //         6 => TileType.Water,
    //         7 => TileType.Brick,
    //         8 => TileType.FloorBrick,
    //         10 => TileType.Swamp,
    //         _ => TileType.Empty
    //     };
    // }
}
