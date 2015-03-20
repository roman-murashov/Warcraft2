using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warcraft2.Application.Core.Buildings;
using Warcraft2.Application.Core.Units;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework;
using Warcraft2.Application.Graphics;

namespace Warcraft2.Application.Core
{
    public class GameSettings
    {
        public List<BuildingType> buildingTypes = new List<BuildingType>();
        public List<GameObjectType> unitTypes = new List<GameObjectType>();
        public List<GameSprite> gameSprites = new List<GameSprite>();

        public BuildingType GetBuildingType(String s)
        {
            var value = buildingTypes.First(item => item.baseProperties.name == s);
            if(value != null)
                return value;
            return null;
        }

        public GameObjectType GetGameObjectType(String s)
        {
            var value = unitTypes.First(item => item.baseProperties.name == s);
            if(value != null)
                return value;
            return null;
        }

        public GameSprite GetGameSprite(String s)
        {
            var value = gameSprites.First(item => item.name == s);
            if(value != null)
                return value;
            return null;
        }

        public void LoadSettings()
        {
            //System.IO.Stream stream = TitleContainer.OpenStream(path);
            FileStream stream = new FileStream("gameSettingsr.xml", FileMode.Open, FileAccess.Read);
            XmlSerializer xs = new XmlSerializer(typeof(GameSettings));
            GameSettings gameSettings = (GameSettings)xs.Deserialize(stream);
            stream.Close();
            this.buildingTypes = gameSettings.buildingTypes;
            this.unitTypes = gameSettings.unitTypes;
            this.gameSprites = gameSettings.gameSprites;
        }

        public void SaveSettings()
        {
            FileStream fs = new FileStream("gameSettingsr.xml", FileMode.Create);
            XmlSerializer xs = new XmlSerializer(this.GetType());
            xs.Serialize(fs, this);
            fs.Close();
        }
    }
}
