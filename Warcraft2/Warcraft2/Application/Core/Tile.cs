using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Core
{
    public class Tile
    {
        public Boolean isGrass = false;
        public Boolean isWater = false;
        public Boolean isStone = false;
        public Boolean isTree = false;
        public Boolean canWalk = false;
        public Boolean canSwim = false;
        public Boolean canFly = true;

        public Tile(Boolean isGrass, Boolean isWater, Boolean isStone, Boolean isTree, Boolean canWalk, Boolean canSwim, Boolean canFly)
        {
            this.isGrass = isGrass;
            this.isWater = isWater;
            this.isStone = isStone;
            this.isTree = isTree;
            this.canWalk = canWalk;
            this.canSwim = canSwim;
            this.canFly = canFly;
        }
    }
}
