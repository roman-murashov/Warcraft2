using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warcraft2.Application.Graphics;

namespace Warcraft2.Application.Core.Buildings
{
    public class Building : GameObject
    {
        protected int buildProgress = 0;
        BuildingType buildingType;
        protected BuildingCommand buildingCommand;
        public Building(BuildingType buildingType, Point position, int ownerId)
        {
            this.buildingType = buildingType;
            this.position = new Rectangle(position.X, position.Y, buildingType.GetBaseProperties().width, buildingType.GetBaseProperties().height);
            this.unitState = UnitState.BuildingIncomplete;
            this.myProperties = buildingType.GetBaseProperties().Clone();
            this.myProperties.currentHealth = this.myProperties.maxHealth;
        }

        public override void DoFrame()
        {
            if(buildProgress < buildingType.GetBaseProperties().buildTime)
            {
                buildProgress++;
                if(buildProgress >= myProperties.buildTime)
                    BuildingComplete();
            }
            else
            {
                if(buildingCommand != null)
                    if(buildingCommand.DoFrame())
                        buildingCommand = null;
            }
        }

        public int GetBuildProgress() { return buildProgress; }

        public void BuildingComplete()
        {
            this.unitState = UnitState.BuildingComplete;
        }

        public BuildingType GetBuildingType()
        {
            return buildingType;
        }

        public override GameObjectType GetGameObjectType()
        {
            return buildingType;
        }

        public override GameObjectProperties GetMyProperties()
        {
            return myProperties;
        }
        public override String GetAssetName()
        {
            GameSprite gameSprite = Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(buildingType.spriteName);
            if(gameSprite != null)
            {
                return gameSprite.assetName;
            }

            return "error";
        }

        public override Graphics.GameSpriteFrame GetGameSpriteFrame()
        {
            double buildPerecent = (double)buildProgress / GetMyProperties().buildTime;

            // Building Incomplete, less than 50%, USE GENERIC
            if(buildPerecent < .6)
            {
                int theFrame = 0;
                if(buildPerecent > .3)
                    theFrame = 1;
                if(buildingType.baseProperties.width > 2)
                    theFrame += (buildingType.baseProperties.width - 2) * 2;
                GameSprite sprite = Engine.gameEngine.GetWorld().gameSettings.GetGameSprite("Human Construction");
                return sprite.gameSpriteFrames[UnitState.BuildingIncomplete][theFrame];
            }
            else
            {
                GameSprite gameSprite = Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(buildingType.spriteName);
                if(gameSprite != null)
                {
                    return gameSprite.GetSpriteFrame(unitState, frame, direction);
                }
            }
            return null;
        }

        public override GameSprite GetGameSprite()
        {
            if((double)buildProgress / GetMyProperties().buildTime < .6)
                return Engine.gameEngine.GetWorld().gameSettings.GetGameSprite("Human Construction");
            return Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(buildingType.spriteName);
        }

        public void IssueNewCommand(BuildingCommand command)
        {
            this.buildingCommand = command;
        }
    }
}
