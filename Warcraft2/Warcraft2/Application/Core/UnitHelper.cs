using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warcraft2.Application.Graphics;
using Microsoft.Xna.Framework;
using RailorLibrary.Data;
using Warcraft2.Application.Core.Units;
using Warcraft2.Application.Core.Buildings;
using Warcraft2.Application.Core.Players;

namespace Warcraft2.Application.Core
{
    public static class UnitHelper
    {
        public static Boolean CanMoveToPoint(Point location, GameObjectProperties properties)
        {
            DataGrid grid = GetIsAirGrid(properties.canFly);
            for(int x = location.X; x < location.X + properties.width; x++)
            {
                for(int y = location.Y; y < location.Y + properties.height;y++)
                {
                    if(grid.GetDataAt(x, y) != 0)
                        return false;
                    Tile t = UnitHelper.GetTileAtLocation(x, y);
                    if(!((t.canWalk && properties.canWalk) ||
                        (t.canSwim && properties.canSwim) ||
                        (t.canFly && properties.canFly)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static Boolean CanBuildOnTile(Point location, GameObjectProperties properties)
        {
            int tileData = Engine.GetWorld().terrainGrid.GetDataAt(location.X, location.Y);
            Tile tile = TileType.GetTile(TileMapper.GetValueOf(tileData));

            if((properties.buildOnGrass && tile.isGrass) || (properties.buildOnShore && (tile.canSwim && tile.canWalk)))
            {
                return true;
            }

            return false;
        }

        public static Tile GetTileAtLocation(int x, int y)
        {
            return TileType.GetTile(TileMapper.GetValueOf(Engine.GetWorld().terrainGrid.GetDataAt(x, y)));
        }

        public static List<GameObject> GetGameObjectsInArea(Rectangle rect)
        {
            World world = Engine.GetWorld();
            List<GameObject> gameObjectList = new List<GameObject>();

            DataGrid airGrid = GetIsAirGrid(true);
            DataGrid groundGrid = GetIsAirGrid(false);

            for(int x = rect.X; x <= rect.X + rect.Width; x++)
            {
                for(int y = rect.Y; y <= rect.Y + rect.Height; y++)
                {
                    int id = airGrid.GetDataAt(x, y);
                    if(id > 0)
                        gameObjectList.Add(GetGameObjectById(id));

                    id = groundGrid.GetDataAt(x, y);
                    if(id > 0)
                        gameObjectList.Add(GetGameObjectById(id));
                }
            }

            return gameObjectList;
        }

        public static List<Point> GetTreesInArea(Rectangle rect)
        {
            List<Point> treeList = new List<Point>();
            DataGrid terrain = Engine.GetWorld().terrainGrid;
            for(int x = rect.X; x < rect.Width + rect.X; x++)
            {
                for(int y = rect.Y; y < rect.Height + rect.Y; y++)
                {
                    if(TileMapper.GetValueOf(terrain.GetDataAt(x, y)) == TileType.Tree)
                    {
                        treeList.Add(new Point(x, y));
                    }
                }
            }

            return treeList;
        }

        public static void ChopDownTreeAt(Point p)
        {
            DataGrid terrain = Engine.GetWorld().terrainGrid;
            if(TileMapper.GetValueOf(terrain.GetDataAt(p.X, p.Y)) == TileType.Tree)
            {
                if(TileMapper.GetValueOf(terrain.GetDataAt(p.X, p.Y - 1)) == TileType.Tree && TileMapper.GetValueOf(terrain.GetDataAt(p.X, p.Y - 2)) != TileType.Tree)
                {
                    terrain.SetDataAt(p.X, p.Y - 1, TileMapper.TILE_TREE_CUTDOWN);
                }
                if(TileMapper.GetValueOf(terrain.GetDataAt(p.X, p.Y + 1)) == TileType.Tree && TileMapper.GetValueOf(terrain.GetDataAt(p.X, p.Y + 2)) != TileType.Tree)
                {
                    terrain.SetDataAt(p.X, p.Y + 1, TileMapper.TILE_TREE_CUTDOWN);
                }
                terrain.SetDataAt(p.X, p.Y, TileMapper.TILE_TREE_CUTDOWN);
            }
        }

        public static Point getFirstOpenPoint(Boolean isAir, Rectangle position, int checkRange = 4)
        {
            World world = Engine.GetWorld();
            DataGrid grid = GetIsAirGrid(isAir);
            if(checkRange == 0)
                checkRange = Engine.checkRange / 2;
            for(int count = 0; count < checkRange; count++)
            {
                // top line
                for(int x = position.X - 1; x < position.X + position.Width + 1; x++)
                {
                    if(grid.GetDataAt(x, position.Y - 1) == 0 && (isAir == false && GetTileAtLocation(x, position.Y - 1).canWalk || isAir))
                    {
                        return new Point(x, position.Y - 1);
                    }
                }

                // Right line
                for(int y = position.Y; y < position.Y + position.Height; y++)
                {
                    if(grid.GetDataAt(position.X + position.Width, y) == 0 && (isAir == false && GetTileAtLocation(position.X + position.Width, y).canWalk || isAir))
                    {
                        return new Point(position.X + position.Width, y);
                    }
                }

                // bottom line
                for(int x = position.X + position.Width; x >= position.X - 1; x--)
                {
                    if(grid.GetDataAt(x, position.Y + position.Height) == 0 && (isAir == false && GetTileAtLocation(x, position.Y + position.Height).canWalk || isAir))
                    {
                        return new Point(x, position.Y + position.Height);
                    }
                }

                // Left line
                for(int y = position.Y + position.Height; y > position.Y - 1; y--)
                {
                    if(grid.GetDataAt(position.X - 1, y) == 0 && (isAir == false && GetTileAtLocation(position.X - 1, y).canWalk || isAir))
                    {
                        return new Point(position.X - 1, y);
                    }
                }

                position.Width += 2;
                position.Height += 2;
                position.X--;
                position.Y--;
            }

            return new Point(-50, 0);
        }

        public static Point getNearestOpenPoint(Boolean isAir, Rectangle newPosition, Point myPoint, int checkRange = 1)
        {
            World world = Engine.GetWorld();
            DataGrid grid = GetIsAirGrid(isAir);
            if(checkRange == 0)
                checkRange = Engine.checkRange / 2;
            Rectangle position = new Rectangle(newPosition.X, newPosition.Y, newPosition.Width, newPosition.Height);
            for(int count = 0; count <= checkRange; count++)
            {
                Point closest = new Point(-50, -50);
                int closestDistance = -50;
                // top line
                for(int x = position.X; x < position.X + position.Width; x++)
                {
                    for(int y = position.Y; y < position.Y + position.Height; y++)
                    {
                        Tile t = TileType.GetTile(TileMapper.GetValueOf(world.GetTerrainGrid().GetDataAt(x,y)));
                        if((grid.GetDataAt(x, y) == 0 || grid.GetDataAt(x,y) == grid.GetDataAt(myPoint.X,myPoint.Y)) && ((!isAir && t.canWalk) || (isAir && t.canFly)))
                        {
                            if(closest.X == -50)
                                closest = new Point(x, y);

                            int xd = x - myPoint.X;
                            int yd = y - myPoint.Y;
                            int distance = (int)Math.Sqrt(xd * xd + yd * yd);

                            if(closestDistance == -50)
                            {
                                closestDistance = distance;
                            }

                            if(distance < closestDistance)
                            {
                                closest = new Point(x,y);
                                closestDistance = distance;
                            }
                        }
                    }
                }

                if(closest.X != -50)
                    return closest;

                position.Width += 2;
                position.Height += 2;
                position.X--;
                position.Y--;
            }

            return new Point(newPosition.X, newPosition.Y);
        }

        public static DataGrid GetIsAirGrid(Boolean isAir)
        {
            World world = Engine.GetWorld();

            if(isAir)
            {
                return world.AirCollisionGrid;
            }

            return world.groundCollisionGrid;
        }

        public static void MoveGameObject(GameObject gameObject, Point p)
        {
            World world = Engine.GetWorld();
            DataGrid grid = UnitHelper.GetIsAirGrid(gameObject.GetMyProperties().canFly);
            SetGridDataForRectangle(grid, gameObject.GetPosition(), 0);
            gameObject.SetLastPosition(gameObject.GetPositionPoint());
            gameObject.SetPosition(p);
            SetGridDataForRectangle(grid, gameObject.GetPosition(), gameObject.GetGameObjectId());
        }

        public static void SetGridDataForRectangle(DataGrid grid, Rectangle position, int value)
        {
            for(int x = position.X; x < position.X + position.Width; x++)
            {
                for(int y = position.Y; y < position.Y + position.Height; y++)
                {
                    grid.SetDataAt(x, y, value);
                }
            }
        }

        public static void CreateNewGameObject(GameObjectType gameObjectType, Rectangle position, int playerId)
        {
            Point p = UnitHelper.getFirstOpenPoint(gameObjectType.baseProperties.canFly, position);
            Unit unit = new Unit(gameObjectType, p, playerId);
            AddGameObject(playerId, unit);
        }

        public static void AddGameObject(int playerId, GameObject gameObject)
        {
            World world = Engine.GetWorld();
            GameObject.gameObjectIdCounter++;
            gameObject.SetGameObjectId(GameObject.gameObjectIdCounter);
            world.gameObjects.Add(gameObject.GetGameObjectId(), gameObject);
            gameObject.SetOwnerId(playerId);
            if(gameObject.GetType() == typeof(Unit))
            {
                Unit n = gameObject as Unit;
                world.units[playerId].Add(n);
            }
            else if(gameObject.GetType() == typeof(Building))
            {
                Building n = gameObject as Building;
                world.buildings[playerId].Add(n);
            }

            DataGrid grid;
            if(gameObject.GetMyProperties().canFly)
            {
                grid = world.AirCollisionGrid;
            }
            else
            {
                grid = world.groundCollisionGrid;
            }

            SetGridDataForRectangle(grid, gameObject.GetPosition(), gameObject.GetGameObjectId());
        }

        public static void RemoveGameObject(GameObject gameObject)
        {
            World world = Engine.GetWorld();
            world.gameObjects.Remove(gameObject.GetGameObjectId());
            if(gameObject.GetType() == typeof(Unit))
            {
                Unit n = gameObject as Unit;
                world.units[gameObject.GetOwnerId()].Remove(n);
            }
            else if(gameObject.GetType() == typeof(Building))
            {
                Building n = gameObject as Building;
                world.buildings[gameObject.GetOwnerId()].Remove(n);
            }

            DataGrid grid;
            if(gameObject.GetMyProperties().canFly)
            {
                grid = world.AirCollisionGrid;
            }
            else
            {
                grid = world.groundCollisionGrid;
            }

            SetGridDataForRectangle(grid, gameObject.GetPosition(), 0);
        }

        public static GameObject GetGameObjectById(int id)
        {
            World world = Engine.GetWorld();
            GameObject gameObject;
            world.gameObjects.TryGetValue(id,out gameObject);

            return gameObject;
        }

        public static Player GetPlayerById(int id)
        {
            return Engine.GetWorld().players[id];
        }
    }
}
