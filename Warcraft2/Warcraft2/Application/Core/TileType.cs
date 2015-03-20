using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Core
{
    public static class TileType
    {
        private static List<Tile> tiles = new List<Tile>();
        public const int Nothing = 0;
        public const int Grass = 1;
        public const int Dirt = 2;
        public const int Tree = 3;
        public const int Water = 4;
        public const int Wall = 5;
        public const int Stone = 6;
        public const int Shore = 7;
        public static void Setup()
        {
            tiles.Add(new Tile(false, false, false, false, false, false, false));// Nothing, NO AIR EITHER DAWG
            tiles.Add(new Tile(true,false,false,false,true,false,true));// Grass
            tiles.Add(new Tile(false, false, false, false, true, false, true));// Dirt
            tiles.Add(new Tile(false, false, false,true, false, false, true));// Tree
            tiles.Add(new Tile(false, true, false, false, false, true, true));// Water
            tiles.Add(new Tile(false, false, false, false, false, false, true));// Wall
            tiles.Add(new Tile(false, false, true, false, false, false, true));// Stone
            tiles.Add(new Tile(false, false, false, false, true, true, true));// Shore
        }

        public static Tile GetTile(int tile)
        {
            if(tile < 8)
                return tiles[tile];
            return tiles[0];
        }
    }
}
