using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Core.Buildings
{
    public class BuildingType : GameObjectType
    {
        public List<String> unitTypes = new List<String>();


        public void AddUnitProduction(params String[] unitNames)
        {
            foreach(String unitName in unitNames)
            {
                unitTypes.Add(unitName);
            }
        }
    }
}
