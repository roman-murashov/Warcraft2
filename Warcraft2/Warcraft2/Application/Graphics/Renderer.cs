using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RailorLibrary.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RailorLibrary.Input;
using Warcraft2.Application.Core;
using RailorLibrary.Data;
using Warcraft2.Application.Core.Players;
using Warcraft2.Application.Core.Buildings;
using System.IO;
using Warcraft2.Application.Core.Units;
using Warcraft2.Application.Core.Units.Commands;

namespace Warcraft2.Application.Graphics
{
    public class Renderer
    {
        public static Random random = new Random(5000);
        public static AssetManager assetManager;
        SpriteBatch spriteBatch;
        public float screenScale = 1;
        Matrix scaleMatrix;
        public static Texture2D whiteRectangle;
        GraphicsDevice graphics;
        ScreenText screenText = new ScreenText();
        public static Color cellColor = Color.Black;
        static int unitState = 6;
        static int frame = 0;
        static int direction = 0;
        static int spriteNameKey = 0;
        static GameSpriteFrame copiedFrame = null;
        static Player p = new Player();
        static Rectangle draggingRectangle;
        static int playerId = 0;
        public Renderer(AssetManager assetManager, GraphicsDevice graphics)
        {
            Renderer.assetManager = assetManager;
            this.graphics = graphics;
            scaleMatrix = Matrix.CreateScale(1, 1, 1);
            whiteRectangle = new Texture2D(graphics, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            TileMapper.Setup(assetManager.GetAsset("tileset"));
            TileMapper.AddRange(0, 15, TileType.Nothing);//White
            TileMapper.AddRange(16, 101, TileType.Wall);//wall
            TileMapper.AddRange(102, 125, TileType.Tree);//tree
            TileMapper.AddRange(126, 126, TileType.Grass);//White
            TileMapper.AddRange(127, 141, TileType.Tree);//tree
            TileMapper.AddRange(142, 179, TileType.Stone);//stone
            TileMapper.AddRange(180, 186, TileType.Dirt);//dirt
            TileMapper.AddRange(187, 187, TileType.Nothing);//White
            TileMapper.AddRange(188, 237, TileType.Dirt);//dirt
            TileMapper.AddRange(238, 269, TileType.Grass);//grass
            TileMapper.AddRange(270, 299, TileType.Dirt);//dirt
            TileMapper.AddRange(300, 333, TileType.Water);//water
            TileMapper.AddRange(334, 355, TileType.Dirt);//dirt
            TileMapper.AddRange(356, 371, TileType.Grass);//grass
            Engine.CreateGameEngine();

        }

        public void ConvertLotsOfTextures()
        {
            String pathName = "textures/orc/";
            List<String> assetNames = new List<String>();
            assetNames.Add("axethrower");
            assetNames.Add("catapult");
            assetNames.Add("deathknight");
            assetNames.Add("destroyer");
            assetNames.Add("dragon");
            assetNames.Add("giantturtle");
            assetNames.Add("goblinsapper");
            assetNames.Add("goblinzepplin");
            assetNames.Add("juggernaught");
            assetNames.Add("ogre");
            assetNames.Add("oiltanker");
            assetNames.Add("peon");
            assetNames.Add("skeleton");
            assetNames.Add("summerbuildings");
            assetNames.Add("winterbuildings");
            assetNames.Add("transport");

            foreach(String assetName in assetNames)
            {
                Texture2D texture = assetManager.GetAsset(pathName + assetName);

                ConvertTexture(texture);

                Stream stream = File.Create(assetName + ".png");
                texture.SaveAsPng(stream, texture.Width, texture.Height);
                stream.Dispose();
                texture.Dispose();
            }
        }
        public Texture2D ConvertTexture(Texture2D texture)
        {
            Color Darkest = new Color(0, 4, 76);
            Color Darkest2 = new Color(0, 20, 116);
            Color Darkest3 = new Color(4, 40, 160);
            Color Darkest4 = new Color(12, 72, 204);

            Color orcColor = new Color(68, 4, 0);
            Color orcColor2 = new Color(92, 4, 0);
            Color orcColor3 = new Color(124, 0, 0);
            Color orcColor4 = new Color(164, 0, 0);

            Color newColor = Darkest;
            Color newColor1 = Darkest2;
            Color newColor2 = Darkest3;
            Color newColor3 = Darkest4;

            Color[] tcolor = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(tcolor);
            for(int x = 0; x < texture.Width; x++) //Convert!
            {
                for(int y = 0; y < texture.Height; y++)
                {
                    if(tcolor[x + y * texture.Width].B == orcColor.B)
                        if(tcolor[x + y * texture.Width].R == orcColor.R)
                            if(tcolor[x + y * texture.Width].G == orcColor.G)
                                tcolor[x + y * texture.Width] = newColor;
                    if(tcolor[x + y * texture.Width].B == orcColor2.B)
                        if(tcolor[x + y * texture.Width].R == orcColor2.R)
                            if(tcolor[x + y * texture.Width].G == orcColor2.G)
                                tcolor[x + y * texture.Width] = newColor1;
                    if(tcolor[x + y * texture.Width].B == orcColor3.B)
                        if(tcolor[x + y * texture.Width].R == orcColor3.R)
                            if(tcolor[x + y * texture.Width].G == orcColor3.G)
                                tcolor[x + y * texture.Width] = newColor2;
                    if(tcolor[x + y * texture.Width].B == orcColor4.B)
                        if(tcolor[x + y * texture.Width].R == orcColor4.R)
                            if(tcolor[x + y * texture.Width].G == orcColor4.G)
                                tcolor[x + y * texture.Width] = newColor3;
                }
            }

            texture.SetData(tcolor);
            return texture;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Boolean changeTexture = false;

            if(GameScreen.GetPlayableRectangle().Contains(PlayerControllerCommands.GetMousePoint()))
            {
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.B) && !changeTexture)
                {
                    Building b = new Building(Engine.gameEngine.GetWorld().GetBuildingType("Town Hall"), new Point(PlayerControllerCommands.GetGameMouseX() / Engine.CELL_SIZE, PlayerControllerCommands.GetGameMouseY() / Engine.CELL_SIZE), playerId);
                    UnitHelper.AddGameObject(playerId, b);
                    //Unit peas = new Unit(Engine.gameEngine.GetWorld().GetGameObjectType("Peasant"), new Point(PlayerControllerCommands.GetGameMouseX() / Engine.CELL_SIZE, PlayerControllerCommands.GetGameMouseY() / Engine.CELL_SIZE), 0);
                    //UnitHelper.AddGameObject(0, peas);
                }

                if(InputManager.IsPressed(MouseButtons.LeftClick))
                {
                    draggingRectangle = new Rectangle(PlayerControllerCommands.GetGameMouseX() / Engine.CELL_SIZE, PlayerControllerCommands.GetGameMouseY() / Engine.CELL_SIZE, 1, 1);
                }

                if(InputManager.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && !InputManager.IsDown(MouseButtons.LeftClick))
                {
                    Point releasePoint = new Point(PlayerControllerCommands.GetGameMouseX() / Engine.CELL_SIZE, PlayerControllerCommands.GetGameMouseY() / Engine.CELL_SIZE);
                    int xDistance = releasePoint.X - draggingRectangle.X;
                    int yDistance = releasePoint.Y - draggingRectangle.Y;

                    draggingRectangle.Width = xDistance;
                    draggingRectangle.Height = yDistance;
                    if(xDistance < 0)
                    {
                        draggingRectangle.Width = -xDistance;
                        draggingRectangle.X = releasePoint.X;
                    }
                    if(yDistance < 0)
                    {
                        draggingRectangle.Height = -yDistance;
                        draggingRectangle.Y = releasePoint.Y;
                    }

                    p.SelectUnitRectangle(draggingRectangle);
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.T) && !changeTexture)
                {
                    playerId++;
                    if(playerId >= Engine.GetWorld().players.Count)
                    {
                        playerId = 0;
                    }
                    
                    
                    
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.E) && !changeTexture)
                {
                    foreach(int key in Engine.GetWorld().players.Keys)
                    {
                        foreach(Building a in Engine.gameEngine.GetWorld().GetBuildings()[key])
                        {
                            CommandBuildUnit buildUnit = new CommandBuildUnit(a, "Footman");
                            a.IssueNewCommand(buildUnit);

                        }
                    }
                }

                if(InputManager.IsDown(MouseButtons.RightClick) && (InputManager.IsDown(Microsoft.Xna.Framework.Input.Keys.A) && !changeTexture))
                {
                    foreach(Unit a in Engine.gameEngine.GetWorld().GetUnits()[1])
                    {
                        CommandAttackMove b = new CommandAttackMove(a, new Point(PlayerControllerCommands.GetGameMouseX() / Engine.CELL_SIZE, PlayerControllerCommands.GetGameMouseY() / Engine.CELL_SIZE));
                        a.IssueNewCommand(b);
                    }
                }
                if(InputManager.IsDown(MouseButtons.RightClick) && !changeTexture)
                {
                    p.IssueGenericCommand(UnitCommand.UnitCommandAttackMove, new Point(PlayerControllerCommands.GetGameMouseX() / Engine.CELL_SIZE, PlayerControllerCommands.GetGameMouseY() / Engine.CELL_SIZE));
                }
            }
            //UpdateGuiObjects();
            spritebatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, scaleMatrix);
            if(!changeTexture)
                DrawTiles(spritebatch);
            if(changeTexture)
                DrawGridOutline(spritebatch, Color.Green);
            //DrawGuiObjects(spritebatch);
            if(!changeTexture)
            {
                foreach(Unit n in p.GetSelectedUnits())
                {
                    Rectangle newLocation = new Rectangle();
                    newLocation.X = n.GetPosition().X;
                    newLocation.Y = n.GetPosition().Y;
                    newLocation.Width = n.GetPosition().Width * Engine.CELL_SIZE;
                    newLocation.Height = n.GetPosition().Height * Engine.CELL_SIZE;
                    newLocation.X = newLocation.X * Engine.CELL_SIZE - GameScreen.GetPosition().X;
                    newLocation.Y = newLocation.Y * Engine.CELL_SIZE - GameScreen.GetPosition().Y;
                    newLocation.X += n.GetGameSpriteFrame().xOffset;
                    newLocation.Y += n.GetGameSpriteFrame().yOffset;
                    newLocation.X += GameScreen.GetMarginLeft();
                    newLocation.Y += GameScreen.GetMarginUp();
                    DrawBox(spritebatch, newLocation, Color.Green);
                }

                DrawBuildings(spritebatch);
                DrawUnits(spritebatch);
                DrawBox(spritebatch, new Rectangle(0, 0, GameScreen.GetMarginLeft(), GameScreen.GetHeight()), Color.Black);
                DrawBox(spritebatch, new Rectangle(0, 0, GameScreen.GetWidth(), GameScreen.GetMarginUp()), Color.Black);
                DrawBox(spritebatch, new Rectangle(0, GameScreen.GetHeight() - GameScreen.GetMarginDown(), GameScreen.GetWidth(), GameScreen.GetMarginDown()), Color.Black);
                DrawBox(spritebatch, new Rectangle(GameScreen.GetWidth() - GameScreen.GetMarginRight(), 0, GameScreen.GetMarginRight(), GameScreen.GetHeight()), Color.Black);



            }
            if(changeTexture)
            {
                Renderer.ChangeTexturePosition(spritebatch);
            }


            spritebatch.End();
        }
        public void DrawUnits(SpriteBatch spritebatch)
        {
            World world = Engine.gameEngine.GetWorld();
            Dictionary<int, List<Unit>> units = world.GetUnits();
            foreach(int playerKey in units.Keys)
            {
                foreach(Unit unit in units[playerKey])
                {
                    Color c = Color.White;
                    DrawGameObject(spritebatch, unit, c);
                }
            }
        }
        public void DrawBuildings(SpriteBatch spritebatch)
        {
            World world = Engine.gameEngine.GetWorld();
            Dictionary<int, List<Building>> buildings = world.GetBuildings();
            foreach(int playerKey in buildings.Keys)
            {
                foreach(Building building in buildings[playerKey])
                {
                    //building.SetPositionRectangle(new Rectangle(PlayerControllerCommands.GetGameMouseX() / Engine.CELL_SIZE, PlayerControllerCommands.GetGameMouseY() / Engine.CELL_SIZE, building.GetMyProperties().width, building.GetMyProperties().height));
                    Rectangle rect = building.GetPosition();
                    Boolean success = true;
                    for(int x = rect.X; x < rect.Width + rect.X; x++)
                    {
                        for(int y = rect.Y; y < rect.Height + rect.Y; y++)
                        {
                            if(!UnitHelper.CanBuildOnTile(new Point(x, y), building.GetBuildingType().GetBaseProperties()))
                            {
                                success = false;
                                break;
                            }
                        }
                    }
                    Color c = Color.White;
                    if(!success)
                        c = Color.Red;
                    DrawGameObject(spritebatch, building, c);
                }
            }


        }

        public void DrawTiles(SpriteBatch spritebatch)
        {
            World world = Engine.gameEngine.GetWorld();
            DataGrid terrainGrid = world.GetTerrainGrid();

            Point position = GameScreen.GetPosition();
            int cellSize = Engine.CELL_SIZE;

            int startingX = position.X / cellSize;
            int endingX = ((GameScreen.GetWidth() - GameScreen.GetMarginRight() - GameScreen.GetMarginLeft()) / cellSize) + startingX + 2;
            if(startingX < 0)
                startingX = 0;
            if(endingX > terrainGrid.Width)
                endingX = terrainGrid.Width;

            int startingY = position.Y / cellSize;
            int endingY = (GameScreen.GetHeight() - GameScreen.GetMarginUp() - GameScreen.GetMarginDown()) / cellSize + startingY + 2;
            if(startingY < 0)
                startingY = 0;
            if(endingY > terrainGrid.Height)
                endingY = terrainGrid.Height;
            for(int x = startingX; x < endingX; x++)
            {
                int positionX = x * cellSize;
                positionX -= position.X;
                for(int y = startingY; y < endingY; y++)
                {
                    int positionY = y * cellSize;
                    positionY -= position.Y;
                    DrawTile(spritebatch, terrainGrid.GetDataAt(x, y), new Vector2(positionX, positionY), Color.White);
                }
            }
        }

        public static void DrawGridOutline(SpriteBatch spriteBatch, Color c)
        {
            World world = Engine.gameEngine.GetWorld();
            DataGrid terrainGrid = world.GetTerrainGrid();

            Point position = GameScreen.GetPosition();
            int cellSize = Engine.CELL_SIZE;
            int thickness = 2;
            int startingX = position.X / cellSize;
            if(startingX < 0)
                startingX = -startingX;
            int startingY = position.Y / cellSize;
            if(startingY < 0)
                startingY = -startingY;
            for(int x = 0; x < GameScreen.GetWidth() / cellSize; x++)
            {
                if(!(position.X / cellSize < 0 && x < startingX) && x > 0)
                {
                    DrawBox(spriteBatch, new Rectangle(x * cellSize + GameScreen.GetMarginLeft() - position.X % cellSize, 0 + GameScreen.GetMarginUp(), thickness, GameScreen.GetHeight()), c);
                }
                if(!(position.Y / cellSize < 0 && x < startingY) && x > 0)
                {
                    DrawBox(spriteBatch, new Rectangle(0 + GameScreen.GetMarginLeft(), x * cellSize + GameScreen.GetMarginUp() - position.Y % cellSize, GameScreen.GetWidth(), thickness), c);
                }
            }
        }

        public static void DrawTile(SpriteBatch spriteBatch, int tileNumber, Vector2 location, Color c)
        {
            spriteBatch.Draw(Renderer.assetManager.GetAsset("tileset"), new Rectangle((int)location.X + GameScreen.GetMarginLeft(), (int)location.Y + GameScreen.GetMarginUp(), Engine.CELL_SIZE, Engine.CELL_SIZE), TileMapper.GetSourceRectangle(tileNumber), c);
        }

        public static void DrawIcon(SpriteBatch spriteBatch, String asset, Vector2 location, Color c)
        {
            spriteBatch.Draw(Renderer.assetManager.GetAsset(asset), new Rectangle((int)location.X, (int)location.Y, 25, 25), c);
        }

        public static void DrawImage(SpriteBatch spriteBatch, String asset, Vector2 location, int width, int height, Color c)
        {
            spriteBatch.Draw(Renderer.assetManager.GetAsset(asset), new Rectangle((int)location.X, (int)location.Y, width, height), c);
        }

        public static void DrawBox(SpriteBatch spriteBatch, Rectangle rect, Color c)
        {
            spriteBatch.Draw(whiteRectangle, rect, c);
        }

        public static void DrawString(SpriteBatch spriteBatch, String message, Vector2 vec2, Color c)
        {
            spriteBatch.DrawString(Renderer.assetManager.GetFont("normal"), message, vec2, Color.White, 0, new Vector2(), 1f, SpriteEffects.None, 0);
        }

        public static Vector2 GetStringSize(String message)
        {
            return Renderer.assetManager.GetFont("normal").MeasureString(message);
        }

        public static void DrawGameObject(SpriteBatch spriteBatch, GameObject gameObject, Color c)
        {
            GameSprite gameSprite = gameObject.GetGameSprite();
            GameSpriteFrame gameSpriteFrame = gameObject.GetGameSpriteFrame();
            Vector2 newLocation = new Vector2(gameObject.GetPosition().X, gameObject.GetPosition().Y);
            double actionPercent = 0;

            if(gameObject.GetType() == typeof(Unit))
            {
                Unit unit = gameObject as Unit;
                if(unit.GetUnitAction() == UnitActionLock.Moving)
                {
                    actionPercent = unit.GetActionPercent();
                    Point lastPosition = unit.GetLastPosition();
                    if(lastPosition != null)
                    {
                        newLocation = new Vector2(unit.GetLastPosition().X, unit.GetLastPosition().Y);
                    }
                }
            }

            newLocation.X = newLocation.X * Engine.CELL_SIZE - GameScreen.GetPosition().X;
            newLocation.Y = newLocation.Y * Engine.CELL_SIZE - GameScreen.GetPosition().Y;
            newLocation.X += gameSpriteFrame.xOffset;
            newLocation.Y += gameSpriteFrame.yOffset;
            newLocation.X += GameScreen.GetMarginLeft();
            newLocation.Y += GameScreen.GetMarginUp();
            SpriteEffects effect = SpriteEffects.None;
            if(gameObject.GetDirection() > 4)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            if(gameObject.GetType() == typeof(Unit))
            {
                Unit unit = gameObject as Unit;
                if(unit.GetUnitAction() == UnitActionLock.Moving)
                {
                    actionPercent = unit.GetActionPercent();
                    Point lastPosition = unit.GetLastPosition();
                    if(lastPosition != null)
                    {
                        newLocation.X -= (int)((lastPosition.X - gameObject.GetPosition().X) * (actionPercent * Engine.CELL_SIZE));
                        newLocation.Y += (int)((gameObject.GetPosition().Y - lastPosition.Y) * (actionPercent * Engine.CELL_SIZE));
                    }
                }
            }

            spriteBatch.Draw(GetAsset(gameSprite.assetName, gameObject.GetOwner().color), newLocation, gameSpriteFrame.sourceRectangle, c, 0f, new Vector2(0, 0), 1f, effect, 0);
        }

        public static GameSprite GetGameSprite(GameObject gameObject)
        {
            return Engine.gameEngine.GetWorld().gameSettings.GetGameSprite(gameObject.GetGameObjectType().spriteName);
        }

        public static void ChangeTexturePosition(SpriteBatch spritebatch)
        {
            GameSettings settings = Engine.gameEngine.GetWorld().gameSettings;

            List<String> spriteNames = new List<String>();
            foreach(GameSprite temp in settings.gameSprites)
            {
                spriteNames.Add(temp.name);
            }

            String spriteName = spriteNames[spriteNameKey];
            GameSprite sprite = settings.GetGameSprite(spriteName);
            GameSpriteFrame spriteFrame = sprite.GetSpriteFrame(unitState, frame, direction);

            RailorLibrary.Data.Cout.W("Name: " + spriteName + " Unit State: " + unitState + " Frame: " + frame + " Direction: " + direction);
            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.R))
            {
                frame--;
                if(frame < 0)
                    frame = sprite.gameSpriteFrames[unitState].Count - 1;
                if(frame < 0)
                    frame = 0;
            }
            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.T))
            {
                frame++;
                if(frame >= sprite.gameSpriteFrames[unitState].Count)
                    frame = 0;
            }
            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                spriteNameKey--;
                frame = 0;
                if(spriteNameKey < 0)
                    spriteNameKey = spriteNames.Count - 1;
                if(spriteNameKey < 0)
                    spriteNameKey = 0;
            }
            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.E))
            {
                frame = 0;
                spriteNameKey++;
                if(spriteNameKey > spriteNames.Count - 1)
                    spriteNameKey = 0;
            }


            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.X))
            {
                frame = 0;
                unitState++;
                if(unitState > 7)
                    unitState = 0;
            }
            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Z))
            {
                frame = 0;
                unitState--;
                if(unitState < 0)
                    unitState = 7;
            }
            if(spriteFrame != null)
            {
                // --- OFFSET ----
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    spriteFrame.xOffset--;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    spriteFrame.xOffset++;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    spriteFrame.yOffset--;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    spriteFrame.yOffset++;
                }
                // -- SOURCE RECTANGLE -- 
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Left))
                {
                    spriteFrame.sourceRectangle.X--;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Right))
                {
                    spriteFrame.sourceRectangle.X++;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    spriteFrame.sourceRectangle.Y--;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Down))
                {
                    spriteFrame.sourceRectangle.Y++;
                }
                // -- WIDTH AND HEIGHT OF SOURCE
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4))
                {
                    spriteFrame.sourceRectangle.Width--;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6))
                {
                    spriteFrame.sourceRectangle.Width++;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad8))
                {
                    spriteFrame.sourceRectangle.Height--;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
                {
                    spriteFrame.sourceRectangle.Height++;
                }

                if(GameScreen.GetPlayableRectangle().Contains(PlayerControllerCommands.GetMousePoint()))
                {
                    if(InputManager.IsDown(MouseButtons.LeftClick))
                    {
                        spriteFrame.sourceRectangle.X = InputManager.GetMouseX();
                        spriteFrame.sourceRectangle.Y = InputManager.GetMouseY();
                    }
                }

                Vector2 newLocation = new Vector2(0, 0);
                newLocation.X += spriteFrame.xOffset + Engine.CELL_SIZE;
                newLocation.Y += spriteFrame.yOffset + Engine.CELL_SIZE;
                spritebatch.Draw(Renderer.assetManager.GetAsset(sprite.assetName), newLocation, spriteFrame.sourceRectangle, Color.Wheat, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                newLocation.Y += 50;
                spritebatch.Draw(Renderer.assetManager.GetAsset(sprite.assetName), newLocation, spriteFrame.sourceRectangle, Color.Wheat, 0f, new Vector2(0, 0), 1, SpriteEffects.FlipHorizontally, 0);




                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.P))
                {
                    copiedFrame = new GameSpriteFrame();
                    Rectangle rect = new Rectangle(spriteFrame.sourceRectangle.X, spriteFrame.sourceRectangle.Y, spriteFrame.sourceRectangle.Width, spriteFrame.sourceRectangle.Height);
                    copiedFrame.sourceRectangle = rect;
                    copiedFrame.xOffset = spriteFrame.xOffset;
                    copiedFrame.yOffset = spriteFrame.yOffset;
                }

                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.I))
                {
                    spriteFrame.sourceRectangle.X = copiedFrame.sourceRectangle.X;
                }
                if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.O))
                {
                    spriteFrame.sourceRectangle.Y = copiedFrame.sourceRectangle.Y;
                }
            }

            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                GameSpriteFrame newFrame = new GameSpriteFrame();
                Rectangle rect = new Rectangle(copiedFrame.sourceRectangle.X, copiedFrame.sourceRectangle.Y, copiedFrame.sourceRectangle.Width, copiedFrame.sourceRectangle.Height);
                newFrame.sourceRectangle = rect;
                newFrame.xOffset = copiedFrame.xOffset;
                newFrame.yOffset = copiedFrame.yOffset;
                sprite.gameSpriteFrames[unitState].Add(newFrame);
            }

            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.K))
            {
                for(int x = 0; x < 5; x++)
                {
                    GameSpriteFrame tempFrame = sprite.GetSpriteFrame(unitState, frame - x, direction);
                    GameSpriteFrame newFrame = new GameSpriteFrame();
                    Rectangle rect = new Rectangle(tempFrame.sourceRectangle.X, tempFrame.sourceRectangle.Y, tempFrame.sourceRectangle.Width, tempFrame.sourceRectangle.Height);
                    newFrame.sourceRectangle = rect;
                    newFrame.xOffset = tempFrame.xOffset;
                    newFrame.yOffset = tempFrame.yOffset;
                    sprite.gameSpriteFrames[unitState].Add(newFrame);
                }

            }
            String unitStateString = "";
            switch(unitState)
            {
                case 0: unitStateString = "UnitStand"; break;
                case 1: unitStateString = "Unit Move"; break;
                case 2: unitStateString = "Unit Attack"; break;
                case 3: unitStateString = "Unit Carry Wood"; break;
                case 4: unitStateString = "Unit Carry Gold"; break;
                case 5: unitStateString = "Unit Carry Oil"; break;
                case 6: unitStateString = "Building Complete"; break;
                case 7: unitStateString = "Building Incomplete"; break;
                case 8: unitStateString = "Building Occupied"; break;
                case 9: unitStateString = "Icon"; break;
                default: break;
            }
            List<String> commands = new List<String>();
            commands.Add("Building: Q/E: " + spriteNames[spriteNameKey]);
            commands.Add("UnitState: Z/X: " + unitStateString);
            commands.Add("Frame: R/T: " + frame);
            if(spriteFrame != null)
                commands.Add("Offset: W/A/S/D: X:" + spriteFrame.xOffset + " , Y: " + spriteFrame.yOffset);
            else
                commands.Add("Offset: W/A/S/D: ");
            if(spriteFrame != null)
                commands.Add("Source Position: Arrow Keys: X:" + spriteFrame.sourceRectangle.X + " , Y: " + spriteFrame.sourceRectangle.Y);
            else
                commands.Add("Source Position: Arrow Keys: ");

            if(spriteFrame != null && copiedFrame != null)
            {
                commands.Add("Copied Source Position: Arrow Keys: X:" + copiedFrame.sourceRectangle.X + " , Y: " + copiedFrame.sourceRectangle.Y);
                commands.Add("I Copies source position X, O Copies source position Y");
            }

            if(spriteFrame != null)
                commands.Add("Source Size: Numpad 8/6/2/4: Width:" + spriteFrame.sourceRectangle.Width + " , Height: " + spriteFrame.sourceRectangle.Height);
            else
                commands.Add("Source Size: Numpad 8/6/2/4 ");

            int counter = 0;
            foreach(String str in commands)
            {
                DrawString(spritebatch, str, new Vector2(200, counter * 25 + 50), Color.Green);
                counter++;
            }
            if(InputManager.IsPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Engine.gameEngine.GetWorld().gameSettings.SaveSettings();
            }
        }

        public static Texture2D GetAsset(String assetName, Color playerColor)
        {
            if(playerColor != null)
            {
                if(assetManager.HasAsset(assetName + playerColor.PackedValue))
                {
                    return assetManager.GetAsset(assetName + playerColor.PackedValue);
                }
                else
                {
                    Color Darkest = new Color(0, 4, 76);
                    Color Darkest2 = new Color(0, 20, 116);
                    Color Darkest3 = new Color(4, 40, 160);
                    Color Darkest4 = new Color(12, 72, 204);
                    Color newColor = Color.Lerp(playerColor, Color.Black, .5f);
                    Color newColor1 = Color.Lerp(playerColor, Color.Black, .4f);
                    Color newColor2 = Color.Lerp(playerColor, Color.Black, .3f);
                    Color newColor3 = Color.Lerp(playerColor, Color.Black, .0f);

                    Texture2D baseTexture = assetManager.GetAsset(assetName);
                    Texture2D texture = new Texture2D(baseTexture.GraphicsDevice, baseTexture.Width, baseTexture.Height);
                    Color[] tcolor = new Color[texture.Width * texture.Height];
                    baseTexture.GetData<Color>(tcolor);
                    for(int x = 0; x < texture.Width; x++) //Convert!
                    {
                        for(int y = 0; y < texture.Height; y++)
                        {
                            if(tcolor[x + y * texture.Width].B == Darkest.B)
                                if(tcolor[x + y * texture.Width].R == Darkest.R)
                                    if(tcolor[x + y * texture.Width].G == Darkest.G)
                                        tcolor[x + y * texture.Width] = newColor;
                            if(tcolor[x + y * texture.Width].B == Darkest2.B)
                                if(tcolor[x + y * texture.Width].R == Darkest2.R)
                                    if(tcolor[x + y * texture.Width].G == Darkest2.G)
                                        tcolor[x + y * texture.Width] = newColor1;
                            if(tcolor[x + y * texture.Width].B == Darkest3.B)
                                if(tcolor[x + y * texture.Width].R == Darkest3.R)
                                    if(tcolor[x + y * texture.Width].G == Darkest3.G)
                                        tcolor[x + y * texture.Width] = newColor2;
                            if(tcolor[x + y * texture.Width].B == Darkest4.B)
                                if(tcolor[x + y * texture.Width].R == Darkest4.R)
                                    if(tcolor[x + y * texture.Width].G == Darkest4.G)
                                        tcolor[x + y * texture.Width] = newColor3;
                        }
                    }

                    texture.SetData(tcolor);
                    assetManager.SetAsset(assetName + playerColor.PackedValue, texture);
                }
            }

            return Renderer.assetManager.GetAsset(assetName + playerColor.PackedValue);
        }
    }
}
