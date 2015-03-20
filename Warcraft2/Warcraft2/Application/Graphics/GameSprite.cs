using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Graphics
{
    public static class UnitState
    {
        public const int UnitStand = 0;
        public const int UnitMove = 1;
        public const int UnitAttack = 2;
        public const int UnitCarryWood = 3;
        public const int UnitCarryGold = 4;
        public const int UnitCarryOil = 5;
        public const int BuildingComplete = 6;
        public const int BuildingIncomplete = 7;
        public const int BuildingOccupied = 8;
        public const int Icon = 9;
    }

    public class GameSprite
    {
        public String name;
        public String assetName;
        public List<List<GameSpriteFrame>> gameSpriteFrames = new List<List<GameSpriteFrame>>();

        public GameSprite()
        {
        }

        public GameSprite(String name, String assetName)
        {
            this.name = name;
            this.assetName = assetName;
            CreateLists();
        }

        private void CreateLists()
        {
            for(int x = 0; x < 10; x++)
            {
                gameSpriteFrames.Add(new List<GameSpriteFrame>());
            }
        }

        public GameSpriteFrame GetSpriteFrame(int unitState, int frame, int direction)
        {
            List<GameSpriteFrame> frames = gameSpriteFrames[unitState];
            if(frames.Count > 0)
            {
                if(direction >= 0)
                {
                    int framesPerDirection = frames.Count() / 5;
                    return frames[framesPerDirection * direction + frame];//direction * framesPerDirection + frame;
                }
                else
                {
                    if(frame >= 0)
                        return frames[frame];
                    return frames[0];
                }
            }

            return null;
        }
    }
}
