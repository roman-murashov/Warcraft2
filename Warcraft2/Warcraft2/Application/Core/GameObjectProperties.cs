using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Warcraft2.Application.Core
{
    public class GameObjectProperties
    {
        public String name = "GameObjectProperties";
        public int maxHealth = 500;
        public int currentHealth = 0;
        public int maxMana = 0;
        public int currentMana = 0;
        public int attackDamage = 5;
        public int piercingDamage = 2;
        public int attackRange = 1;
        public int sightRange = 5;
        public int armor = 1;
        public int foodCost = 0;
        public int foodProduction = 0;
        public int goldCost = 0;
        public int woodCost = 0;
        public int width = 1;
        public int height = 1;
        public int buildTime = 60;
        public int movementSpeed = 0;

        public Boolean buildOnGrass = true;
        public Boolean buildOnShore = false;
        public Boolean buildOnOil = false;
        public Boolean buildOnWater = false;
        public Boolean canWalk = false;
        public Boolean canSwim = false;
        public Boolean canFly = false;
        public void SetSize(int newWidth, int newHeight) { this.width = newWidth; this.height = newHeight; }
        public GameObjectProperties Clone()
        {
            GameObjectProperties newProperties = new GameObjectProperties();
            var fieldValues = this.GetType()
                     .GetFields();
            foreach(FieldInfo field in fieldValues)
            {
                field.SetValue(newProperties, field.GetValue(this));
            }

            return newProperties;
        }

    }
}
