using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RailorLibrary.Data;
using Warcraft2.Application.Graphics;
using Warcraft2.Application.Core.Buildings;
using Microsoft.Xna.Framework;
using System.Xml;
using Warcraft2.Application.Networking;
using System.Xml.Serialization;
using System.IO;
using RailorLibrary.Input;
using Warcraft2.Application.Core.Units;
using Warcraft2.Application.Core.Units.Commands;
using Warcraft2.Application.Core.Players;
namespace Warcraft2.Application.Core
{
    public class World : Frameable
    {

        public GameSettings gameSettings = new GameSettings();
        public Dictionary<int, List<Building>> buildings = new Dictionary<int, List<Building>>();
        public Dictionary<int, List<Unit>> units = new Dictionary<int, List<Unit>>();
        public Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();
        public Dictionary<int, Player> players = new Dictionary<int, Player>();
        public DataGrid terrainGrid;
        public DataGrid fogGrid;
        public DataGrid groundCollisionGrid;
        public DataGrid AirCollisionGrid;
        public World(int width, int height)
        {
            terrainGrid = new DataGrid(width, height);
            groundCollisionGrid = new DataGrid(width, height);
            AirCollisionGrid = new DataGrid(width, height);

            int counter = 0;
            for(int y = 0; y < terrainGrid.Height; y++)
            {
                for(int x = 0; x < terrainGrid.Width; x++)
                {
                    if(GameEngine.random.NextDouble() > .99)
                    {
                        terrainGrid.SetDataAt(x, y - 1, TileMapper.GetRandomValue(TileType.Tree));
                        terrainGrid.SetDataAt(x, y, TileMapper.GetRandomValue(TileType.Tree));
                    }
                    else
                        terrainGrid.SetDataAt(x, y, TileMapper.GetRandomValue(TileType.Grass));

                    counter++;
                }
            }

            for(int y = 0; y < terrainGrid.Height; y++)
            {
                for(int x = 0; x < terrainGrid.Width; x++)
                {
                    if(TileMapper.GetValueOf(terrainGrid.GetDataAt(x, y)) == TileType.Tree)
                    {
                        if(y - 1 >= 0 && TileMapper.GetValueOf(terrainGrid.GetDataAt(x, y - 1)) == TileType.Grass)
                        {
                            terrainGrid.SetDataAt(x, y, 121);
                        }
                        if(y + 1 >= 0 && TileMapper.GetValueOf(terrainGrid.GetDataAt(x, y + 1)) == TileType.Grass)
                        {
                            terrainGrid.SetDataAt(x, y, 123);
                        }
                    }
                }
            }


            Player p = new Player();
            p.color = Color.Red;
            p.id = 0;
            AddPlayer(p);

            p = new Player();
            p.color = Color.Blue;
            p.id = 1;
            AddPlayer(p);

            p = new Player();
            p.color = Color.Yellow;
            p.id = 2;
            AddPlayer(p);

            p = new Player();
            p.color = Color.Green;
            p.id = 3;
            AddPlayer(p);

            p = new Player();
            p.color = Color.Purple;
            p.id = 4;
            AddPlayer(p);

            fogGrid = new DataGrid(width, height);
            gameSettings.LoadSettings();

            foreach(int key in players.Keys)
            {
                Player player = players[key];
                buildings[player.id] = new List<Building>();
                units[player.id] = new List<Unit>();
            }

            //Unit peas = new Unit(GetGameObjectType("Peasant"), new Point(5,5), 0);
            //GetUnits()[0].Add(peas);
            //CommandChop commandMove = new CommandChop(peas, new Point(25, 25));
            //peas.IssueNewCommand(commandMove);

        }

        public Dictionary<int, List<Building>> GetBuildings()
        {
            return buildings;
        }

        public Dictionary<int, List<Unit>> GetUnits()
        {
            return units;
        }

        public void DoFrame()
        {
            foreach(int playerId in buildings.Keys)
            {
                foreach(Building b in buildings[playerId])
                {
                    b.DoFrame();
                }
                foreach(Unit u in units[playerId])
                {
                    u.DoFrame();
                }
            }
        }

        public DataGrid GetTerrainGrid()
        {
            return terrainGrid;
        }

        public BuildingType GetBuildingType(String buildingName)
        {
            return gameSettings.GetBuildingType(buildingName);
        }

        public GameObjectType GetGameObjectType(String unitName)
        {
            return gameSettings.GetGameObjectType(unitName);
        }

        public void AddPlayer(Player p)
        {
            players[p.id] = p;
        }
    }
}
