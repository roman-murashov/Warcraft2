using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Core.Units.Commands
{
    public abstract class UnitCommand
    {
        public const int UnitCommandMove = 0;
        public const int UnitCommandAttackMove = 1;
        public const int UnitCommandChop = 2;

        protected Boolean complete = false;
        public Unit unit;
        public abstract Boolean DoFrame();
        public virtual void Complete() { complete = true; }
        public Boolean IsComplete() { return complete; }
        public abstract void FinishedLock();
    }
}
