using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Warcraft2.Application.Graphics;

namespace Warcraft2.Application.Core
{
    public class GameObjectType
    {
        public GameObjectProperties baseProperties;
        public String spriteName;
        
        public void SetBaseProperties(GameObjectProperties properties) { this.baseProperties = properties; }
        public void SetProperties(Dictionary<String, Object> properties)
        {
            //System.Reflection.PropertyInfo test = this.GetType().GetProperty("name");
            //foreach (String key in properties.Keys)
            //{
            //    System.Reflection.PropertyInfo propertyInfo = typeof(GameObjectType).GetProperty(key);
            //    propertyInfo.SetValue(this,properties[key],null);
            //}
        }

        public GameObjectProperties GetBaseProperties()
        {
            if(baseProperties == null)
                baseProperties = new GameObjectProperties();
            return baseProperties;
        }
    }
}
