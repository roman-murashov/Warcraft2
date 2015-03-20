using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warcraft2.Application.Networking.PlayerCommands;

namespace Warcraft2.Application.Core
{
    public class GameEngine
    {
        int currentFrame = 0;
        PlayerCommands playerCommands = new PlayerCommands();
        World world;
        GameStatus gameStatus = GameStatus.Running;
        public static Random random = new Random(5000);
        public enum GameStatus
        {
            Running,
            Stopped
        }

        public GameEngine()
        {
            world = new World(64,64);
        }

        public void DoFrame(int frame)
        {
            while(currentFrame < frame)
            {
                currentFrame++;
                List<Command> commands = playerCommands.GetCommandsForFrame(frame);
                if(commands.Count > 0)
                {
                    foreach(CommandInterface command in commands)
                    {
                        command.DoFrame();
                    }
                }

                world.DoFrame();
            }
        }

        public int GetCurrentFrame()
        {
            return currentFrame;
        }

        public World GetWorld()
        {
            return world;
        }
    }
}
