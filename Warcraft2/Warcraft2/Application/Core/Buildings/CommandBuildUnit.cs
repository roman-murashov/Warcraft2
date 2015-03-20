using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warcraft2.Application.Core.Units;
using Microsoft.Xna.Framework;

namespace Warcraft2.Application.Core.Buildings
{
    public class CommandBuildUnit : BuildingCommand
    {
        public CommandBuildUnit(Building b, String unitTypeName)
        {
            this.gameObjectType = Engine.GetWorld().GetGameObjectType(unitTypeName);
            this.buildTime = gameObjectType.baseProperties.buildTime;
            this.myBuilding = b;
        }

        public override bool DoFrame()
        {
            this.buildProgress++;
            if(buildProgress > buildTime)
            {
                Complete();
            }

            return complete;
        }

        public override void Complete()
        {
            UnitHelper.CreateNewGameObject(gameObjectType, myBuilding.GetPosition(), myBuilding.GetOwnerId());
            
            complete = true;
        }
    }
}
