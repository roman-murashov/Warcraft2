using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Warcraft2.Application.Core.Units.Commands
{
    public class CommandChop : UnitCommand
    {
        protected enum ChopState
        {
            MovingToTree,
            ChoppingTree,
            DroppingOff
        }

        protected Point endLocation;
        protected UnitPath unitPath;
        protected ChopState chopState;
        CommandMove commandMove;
        int chopsTillTreeDead = 10;
        int chopCount = 0;
        public CommandChop(Unit newUnit, Point p)
        {
            this.unit = newUnit;
            endLocation = p;
            chopState = ChopState.MovingToTree;
            commandMove = new CommandMove(newUnit, p);
        }

        public override Boolean DoFrame()
        {
            if(chopState == ChopState.MovingToTree)
            {
                if(commandMove != null)
                {
                    if(commandMove.DoFrame())
                    {
                        if(!FindNearestTree())
                        {
                            Complete();
                        }
                    }
                }
                else
                {
                    if(!FindNearestTree())
                    {
                        Complete();
                    }
                }
            }
            else if(chopState == ChopState.ChoppingTree)
            {
                if(!ChopTree())
                {
                    if(!FindNearestTree())
                    {
                        Complete();
                    }
                }
            }
            

            return complete;
        }

        private Boolean FindNearestTree()
        {

            double xd = endLocation.X - unit.GetPositionPoint().X;
            double yd = endLocation.Y - unit.GetPositionPoint().Y;
            double distance = (int)Math.Sqrt(xd * xd + yd * yd);

            if(distance <= 1 && UnitHelper.GetTileAtLocation(endLocation.X, endLocation.Y).isTree)
            {
                chopState = ChopState.ChoppingTree;
                return true;
            }

            Rectangle sightRangeRectangle = unit.GetSightRangeRectangle();
            List<Point> treeList = UnitHelper.GetTreesInArea(sightRangeRectangle);
            Point closest = new Point(-50, -50);
            double closestDistance = -50;
            foreach(Point p in treeList)
            {
                if(closest.X == -50)
                    closest = p;

                xd = p.X - unit.GetPositionPoint().X;
                yd = p.Y - unit.GetPositionPoint().Y;
                distance = (int)Math.Sqrt(xd * xd + yd * yd);

                if(closestDistance == -50)
                {
                    closestDistance = distance;
                }

                if(distance < closestDistance)
                {
                    closest = p;
                    closestDistance = distance;
                }
            }

            if(closest.X == -50)
            {
                return false;
            }

            if(closestDistance > 1)
            {
                endLocation = closest;
                chopState = ChopState.MovingToTree;
                commandMove = new CommandMove(unit, closest);
            }
            else if(closestDistance == 1)
            {
                chopState = ChopState.ChoppingTree;
                endLocation = closest;
            }

            return true;
        }

        private Boolean ChopTree()
        {
            int xd = endLocation.X - unit.GetPositionPoint().X;
            int yd = endLocation.Y - unit.GetPositionPoint().Y;
            int distance = (int)Math.Sqrt(xd * xd + yd * yd);
            if(UnitHelper.GetTileAtLocation(endLocation.X, endLocation.Y).isTree && distance == 1)
            {
                unit.FacePoint(endLocation);
                unit.SetUnitActionLock(UnitActionLock.Attacking, 6);
                commandMove = null;
                AddSwing();
                return true;
            }
            else
            {
                chopCount = 0;
                return false;
            }
        }

        public Boolean AddSwing()
        {
            chopCount++;

            if(chopCount > chopsTillTreeDead)
            {
                UnitHelper.ChopDownTreeAt(endLocation);
                chopCount = 0;
                return true;
            }

            return false;
        }

        public override void FinishedLock()
        {
            throw new NotImplementedException();
        }
    }
}
