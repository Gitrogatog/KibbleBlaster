using System;
using System.Collections.Generic;


static class CollisionTileStorage
{
    // we have a list of enums (strings) each of which corresponds to a set of tiles
    // we want: enum -> id, id -> enum, and tile id -> id
    static int[] TileToID = new int[1024 * 1024]; // making it extra big just in case
    static string[] IDToString = new string[64];
    static Dictionary<string, int> StringToID = new Dictionary<string, int>();
    static int NextID = 0;
    public static string GetStringFromID(int id)
    {
        return IDToString[id];
    }
    public static int GetID(string text)
    {
        if (!StringToID.ContainsKey(text))
        {
            RegisterString(text);
        }
        return StringToID[text];
    }
    static void RegisterString(string text)
    {
        if (NextID >= IDToString.Length)
        {
            Array.Resize(ref IDToString, IDToString.Length * 2);
        }
        IDToString[NextID] = text;
        StringToID[text] = NextID;
        NextID++;
    }
}