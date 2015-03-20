using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warcraft2.Application.Networking;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using Warcraft2.Application.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Warcraft2.Application.Core.Players;

namespace Warcraft2.Application.Core
{
    public static class Engine
    {
        static public GameEngine gameEngine;
        static public Renderer renderer;
        static public PlayerController playerController;
        public const int checkRange = 6;
        public const int framesPerSecond = 20;
        public const int timeBetweenFrames = 1000 / framesPerSecond;
        public const int CELL_SIZE = 32;
        static int miliseconds = 0;

        public static void CreateGameEngine()
        {
            gameEngine = new GameEngine();
            int firstPort = Network.GetFirstFreePort();
            if(firstPort == 5000)
            {
                Network.StartServer();
            }

            Network.StartClient();
            Network.getGameClient().SendDiscoveryRequest();
        }

        public static void Update(GameTime gameTime)
        {
            miliseconds += gameTime.ElapsedGameTime.Milliseconds;

            Network.UpdateNetwork();
            if(miliseconds > timeBetweenFrames)
            {
                if(Network.IsServer())
                {
                    NetworkCommands.SendFrame();
                }
                miliseconds = 0;
            }
        }

        public static void DoFrame(int frame)
        {
            gameEngine.DoFrame(frame);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            renderer.Draw(spriteBatch);
        }

        public static World GetWorld()
        {
            if(gameEngine != null)
                return Engine.gameEngine.GetWorld();
            return null;
        }
    }
}
