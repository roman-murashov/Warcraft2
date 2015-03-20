using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Warcraft2.Application.Core.Units.Commands
{
    public class CommandMove : UnitCommand
    {
        protected Point endLocation;
        protected UnitPath unitPath;
        public CommandMove(Unit newUnit, Point p)
        {
            this.unit = newUnit;
            endLocation = p;
            PathToPoint(UnitHelper.getNearestOpenPoint(newUnit.GetMyProperties().canFly, new Rectangle(p.X, p.Y, 1, 1), newUnit.GetPositionPoint()));
        }

        public override Boolean DoFrame()
        {
            if(unitPath.HasNext())
            {
                Rectangle position = unit.GetPosition();
                Point p = unitPath.GetNext();
                Point newLocation = new Point(position.X + p.X, position.Y + p.Y);
                Tile t = UnitHelper.GetTileAtLocation(position.X + p.X, position.Y + p.Y);
                if(UnitHelper.CanMoveToPoint(newLocation, unit.GetMyProperties()))
                {
                    unit.FacePointDirection(p);
                    UnitHelper.MoveGameObject(unit,new Point(position.X + p.X, position.Y + p.Y));
                    unit.SetUnitActionLock(UnitActionLock.Moving, 30);
                }
                else
                {
                    Complete();
                }
            }
            else
            {
                Complete();
            }

            return complete;
        }

        public override void Complete()
        {
            complete = true;
            unit.Stand();
        }

        public void PathToPoint(Point p)
        {
            PathToPoint(p.X, p.Y);
        }

        // Split the increase over the course of the entire journey
        public void PathToPoint(int x, int y)
        {
            unitPath = new UnitPath();

            int currentX = unit.GetPosition().X;
            int currentY = unit.GetPosition().Y;
            int startingDistanceX = Math.Abs(currentX - x); ;
            int distanceX = Math.Abs(currentX - x);
            int distanceY = Math.Abs(currentY - y);


            int maxDistance = Math.Max(distanceX, distanceY);
            int runs = 0;
            while((currentX != x || currentY != y) && runs < 15)
            {
                int xDirection = 0;
                if(currentX < x)
                {
                    xDirection = 1;
                }
                else if(currentX > x)
                {
                    xDirection = -1;
                }
                currentX += xDirection;
                int yDirection = 0;
                if(currentY < y)
                    yDirection = 1;
                else if(currentY > y)
                    yDirection = -1;
                currentY += yDirection;
                unitPath.AddPoint(xDirection, yDirection);
            }
        }

        public override void FinishedLock()
        {
            //throw new NotImplementedException();
        }
    }
}
