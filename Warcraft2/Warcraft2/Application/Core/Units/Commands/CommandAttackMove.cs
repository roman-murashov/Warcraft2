using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Warcraft2.Application.Core.Units.Commands
{
    public class CommandAttackMove : UnitCommand
    {
        protected Point endLocation;
        protected UnitPath unitPath;
        CommandMove commandMove;
        GameObject attackTarget;
        public CommandAttackMove(Unit newUnit, Point p)
        {
            this.unit = newUnit;
            endLocation = p;
            commandMove = new CommandMove(newUnit, p);
        }

        public override Boolean DoFrame()
        {
            if(GetAttackTarget())
            {
                if(AttackTargetInRange())
                {
                    AttackTarget();
                }
                else
                {
                    attackTarget = null;
                }
            }

            if(attackTarget == null)
            {
                if(!AttackNearby())
                {
                    if(commandMove == null)
                    {
                        commandMove = new CommandMove(unit, endLocation);
                    }

                    commandMove.DoFrame();
                    if(commandMove.IsComplete())
                    {
                        Complete();
                    }
                }
            }

            return complete;
        }

        private Boolean AttackNearby()
        {
            Rectangle sightRangeRectangle = unit.GetSightRangeRectangle();
            Rectangle AttackRangeRectangle = unit.GetAttackRangeRectangle();

            List<GameObject> gameObjects = UnitHelper.GetGameObjectsInArea(sightRangeRectangle);

            foreach(GameObject gameObject in gameObjects)
            {
                if(gameObject != unit && gameObject.GetOwnerId() != unit.GetOwnerId())
                {
                    if(gameObject.GetPosition().Intersects(AttackRangeRectangle))
                    {
                        attackTarget = gameObject;
                        AttackTarget();
                        return true;
                    }
                    else
                    {
                        commandMove = new CommandMove(unit, new Point(gameObject.GetPosition().X, gameObject.GetPosition().Y));
                    }
                }
            }

            return false;
        }

        private void AttackTarget()
        {
            if(attackTarget != null)
            {
                unit.FacePoint(attackTarget.GetPositionPoint());
                unit.SetUnitActionLock(UnitActionLock.Attacking, 8);
            }
        }

        private Boolean AttackTargetInRange()
        {
            if(GetAttackTarget())
            {
                if(attackTarget.GetPosition().Intersects(unit.GetAttackRangeRectangle()))
                {
                    return true;
                }
            }

            return false;
        }

        public override void FinishedLock()
        {
            if(GetAttackTarget())
            {
                unit.DamageTarget(attackTarget);
            }
        }

        public Boolean GetAttackTarget()
        {
            if(attackTarget != null && !attackTarget.IsDead())
            {
                return true;
            }

            attackTarget = null;
            return false;
        }
    }
}
