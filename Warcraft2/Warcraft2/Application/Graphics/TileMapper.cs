
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Warcraft2.Application.Core;
using Microsoft.Xna.Framework;

namespace Warcraft2.Application.Graphics
{
    public static class TileMapper
    {
        public const int TILE_TREE_CUTDOWN = 126;

        static int[] map;
        static Texture2D texture;
        static int width, height;
        static Dictionary<int, List<int>> texturesByKey = new Dictionary<int, List<int>>();
        public static void Setup(Texture2D newTileset)
        {
            texture = newTileset;
            width = 19;
            height = 20;
            map = new int[width * height];
        }

        public static void AddRange(int starting, int ending, int value)
        {
            for(int x = starting; x <= ending; x++)
            {
                map[x] = value;
            }
        }

        public static int GetValueOf(int value)
        {
            if(value < map.Length)
                return map[value];
            return 0;
        }

        public static int GetRandomValue(int value)
        {
            if(!texturesByKey.ContainsKey(value))
            {
                List<int> list = new List<int>();
                for(int x = 0; x < map.Length; x++)
                {
                    if(map[x] == value)
                        list.Add(x);
                }
                texturesByKey[value] = list;
            }

            if(texturesByKey[value].Count > 0)
                return texturesByKey[value][Renderer.random.Next(0, texturesByKey[value].Count)];

            return 0;
        }

        public static Rectangle GetSourceRectangle(int value)
        {
            int x = value % width;
            int y = value / width;
            return new Rectangle(x * (Engine.CELL_SIZE + 1), y * (Engine.CELL_SIZE + 1), Engine.CELL_SIZE, Engine.CELL_SIZE);
        }
    }
}
