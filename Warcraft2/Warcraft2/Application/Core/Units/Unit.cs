using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warcraft2.Application.Graphics;
using Warcraft2.Application.Core.Units.Commands;

namespace Warcraft2.Application.Core.Units
{


    // Something a unit can do these take up turns until complete
    public enum UnitActionLock
    {
        None,
        Moving,
        Attacking,
    }

    public class Unit : GameObject
    {
        UnitActionLock unitActionLock = UnitActionLock.None;
        GameObjectType gameObjectType;
        UnitCommand unitCommand;
        int unitActionLockCounter = 0;
        int unitActionLockTime = 30;
        public Unit(GameObjectType unitType, Point position, int ownerId)
        {
            this.gameObjectType = unitType;
            this.position = new Rectangle(position.X, position.Y, unitType.GetBaseProperties().width, unitType.GetBaseProperties().height);
            this.myProperties = unitType.GetBaseProperties().Clone();
            this.unitState = UnitState.UnitStand;
            this.myProperties.currentHealth = this.myProperties.maxHealth;
        }

        public override void DoFrame()
        {
            if(unitActionLock == UnitActionLock.None && GameEngine.random.Next(0, 100) > 98)
            {
                direction = GameEngine.random.Next(0, 8);
            }
            if(unitActionLock == UnitActionLock.Attacking)
            {
                unitState = UnitState.UnitAttack;
                unitActionLockCounter ++;
                UpdateSpriteFrame();

                if(unitActionLockCounter >= unitActionLockTime)
                {
                    unitActionLockCounter -= unitActionLockTime;
                    unitActionLock = UnitActionLock.None;
                    unitCommand.FinishedLock();
                }
            }

            if(unitActionLock == UnitActionLock.Moving)
            {
                unitState = UnitState.UnitMove;
                unitActionLockCounter += myProperties.movementSpeed;
                UpdateSpriteFrame();

                if(unitActionLockCounter >= unitActionLockTime)
                {
                    unitActionLockCounter -= unitActionLockTime;
                    unitActionLock = UnitActionLock.None;
                }
            }

            if(unitActionLock == UnitActionLock.None)
            {
                if(unitCommand != null)
                {
                    Boolean complete = unitCommand.DoFrame();
                    if(complete)
                    {
                        unitCommand = null;
                        Stand();
                    }
                }
            }
        }

        public void SetUnitActionLock(UnitActionLock actionLock, int length)
        {
            this.unitActionLock = actionLock;
            this.unitActionLockTime = length;
            this.unitActionLockCounter = 0;
        }

        public void IssueNewCommand(UnitCommand command)
        {
            this.unitCommand = command;
        }

        public void Stand()
        {
            unitActionLock = UnitActionLock.None;
            unitState = UnitState.UnitStand;
            frame = 0;
            unitActionLockCounter = 0;
        }

        public void UpdateSpriteFrame()
        {
            GameSprite gameSprite = Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(gameObjectType.spriteName);
            List<GameSpriteFrame> frames = gameSprite.gameSpriteFrames[unitState];
            int countFrames = frames.Count;

            if(unitState == UnitState.UnitStand ||
                unitState == UnitState.UnitMove ||
                unitState == UnitState.UnitCarryWood ||
                unitState == UnitState.UnitCarryOil ||
                unitState == UnitState.UnitCarryGold ||
                unitState == UnitState.UnitAttack)
            {
                countFrames /= 5;
            }
            double movePercent = GetActionPercent();

            frame = (int)(Math.Round(movePercent * countFrames));
            if(frame >= countFrames)
                frame = 0;
        }

        public double GetActionPercent()
        {
            return (double)unitActionLockCounter / unitActionLockTime;
        }
        public void FacePoint(Point p)
        {
            int x = 0;
            int y = 0;
            x = position.X > p.X ? -1 : position.X < p.X ? 1 : 0;
            y = position.Y > p.Y ? -1 : position.Y < p.Y ? 1 : 0;
            FacePointDirection(new Point(x, y));
        }

        public void FacePointDirection(Point p)
        {
            if(p.X == 0 && p.Y == -1)
            {
                direction = 0;
            }
            else if(p.X == 1 && p.Y == -1)
            {
                direction = 1;
            }
            else if(p.X == 1 && p.Y == 0)
            {
                direction = 2;
            }
            else if(p.X == 1 && p.Y == 1)
            {
                direction = 3;
            }
            else if(p.X == 0 && p.Y == 1)
            {
                direction = 4;
            }
            else if(p.X == -1 && p.Y == 1)
            {
                direction = 5;
            }
            else if(p.X == -1 && p.Y == 0)
            {
                direction = 6;
            }
            else if(p.X == -1 && p.Y == -1)
            {
                direction = 7;
            }
        }

        public override GameObjectType GetGameObjectType()
        {
            return gameObjectType;
        }

        public override GameObjectProperties GetMyProperties()
        {
            return myProperties;
        }
        public override String GetAssetName()
        {
            GameSprite gameSprite = Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(gameObjectType.spriteName);
            if(gameSprite != null)
            {
                return gameSprite.assetName;
            }

            return "error";
        }

        public override Graphics.GameSpriteFrame GetGameSpriteFrame()
        {

            GameSprite gameSprite = Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(gameObjectType.spriteName);
            if(gameSprite != null)
            {
                if(direction > 4)
                {
                    return gameSprite.GetSpriteFrame(unitState, frame, 8 - direction);
                }
                else
                {
                    return gameSprite.GetSpriteFrame(unitState, frame, direction);
                }
            }
            return null;
        }

        public override GameSprite GetGameSprite()
        {
            return Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(gameObjectType.spriteName);
        }

        public UnitActionLock GetUnitAction()
        {
            return unitActionLock;
        }
    }
}
