using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Core.Buildings
{
    public abstract class BuildingCommand
    {
        protected int buildProgress = 0;
        protected int buildTime = 60;
        protected Boolean complete = false;
        protected GameObjectType gameObjectType;
        protected Building myBuilding;
        public abstract Boolean DoFrame();
        public virtual void Complete() { complete = true; }
        public Boolean IsComplete() { return complete; }
    }
}
