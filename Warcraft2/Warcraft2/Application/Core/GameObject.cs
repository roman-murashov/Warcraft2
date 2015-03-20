using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warcraft2.Application.Graphics;
using Warcraft2.Application.Core.Players;

namespace Warcraft2.Application.Core
{
    public static class Direction
    {
        public const int Up = 0;
        public const int UpRight = 1;
        public const int Right = 2;
        public const int DownRight = 3;
        public const int Down = 4;
        public const int DownLeft = 5;
        public const int Left = 6;
        public const int UpLeft = 7;
    }

    public abstract class GameObject
    {
        public static int gameObjectIdCounter = 0;
        protected int gameObjectId;
        protected int unitState = 0;
        protected Rectangle position;
        protected int ownerId = 0;
        protected int frame = 0;
        protected int direction = Direction.Up;
        protected GameObjectProperties myProperties;
        protected Point lastPosition;

        public void SetOwnerId(int value) { this.ownerId = value; }
        public int GetOwnerId() { return ownerId; }
        public Player GetOwner() { return UnitHelper.GetPlayerById(ownerId); }
        public abstract void DoFrame();
        public Point GetPositionPoint() { return new Point(position.X, position.Y); }
        public Rectangle GetPosition() { return position; }
        public int GetDirection() { return direction; }
        public int GetGameObjectId() { return gameObjectId; }
        public Point GetLastPosition() { return lastPosition; }

        public void SetLastPosition(Point p) { this.lastPosition = p; }
        public void SetPosition(Point p) { SetPosition(p.X, p.Y); }
        public void SetPosition(int x, int y) { position.X = x; position.Y = y; }
        public void SetPositionRectangle(Rectangle value) { position = value; }
        public void SetGameObjectId(int x) { gameObjectId = x; }
        public Boolean IsDead() { return myProperties.currentHealth <= 0; }
        public abstract String GetAssetName();
        public abstract GameObjectType GetGameObjectType();
        public abstract GameObjectProperties GetMyProperties();
        public abstract GameSprite GetGameSprite();
        public abstract GameSpriteFrame GetGameSpriteFrame();

        public Rectangle GetSightRangeRectangle()
        {
            Rectangle area = new Rectangle();
            Rectangle unitPosition = GetPosition();
            GameObjectProperties properties = GetMyProperties();
            area.X = unitPosition.X - properties.sightRange;
            area.Y = unitPosition.Y - properties.sightRange;
            area.Width = unitPosition.Width + properties.sightRange * 2;
            area.Height = unitPosition.Height + properties.sightRange * 2;

            return area;
        }

        public Rectangle GetAttackRangeRectangle()
        {
            Rectangle area = new Rectangle();
            Rectangle unitPosition = GetPosition();
            GameObjectProperties properties = GetMyProperties();
            area.X = unitPosition.X - properties.attackRange;
            area.Y = unitPosition.Y - properties.attackRange;
            area.Width = unitPosition.Width + properties.attackRange * 2;
            area.Height = unitPosition.Height + properties.attackRange * 2;

            return area;
        }

        public void DamageTarget(GameObject target)
        {
            target.TakeDamage(myProperties.attackDamage, myProperties.piercingDamage);
        }

        public void TakeDamage(int attackDamage, int piercingDamage)
        {
            int damageTaken = attackDamage - myProperties.armor;
            this.myProperties.currentHealth -= (damageTaken + piercingDamage);

            if(this.myProperties.currentHealth <= 0)
            {
                UnitHelper.RemoveGameObject(this);
            }
        }
    }
}
