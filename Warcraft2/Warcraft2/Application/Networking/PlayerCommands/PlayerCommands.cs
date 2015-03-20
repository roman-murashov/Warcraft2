using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Networking.PlayerCommands
{
    public class PlayerCommands
    {
        private Dictionary<int, List<Command>> commands = new Dictionary<int, List<Command>>();

        public List<Command> GetCommandsForFrame(int frame)
        {
            if(commands.ContainsKey(frame))
            {
                return commands[frame];
            }

            return new List<Command>();
        }

        public void AddCommands(int frame, List<Command> newCommands){
            if(!commands.ContainsKey(frame))
            {
                commands[frame] = new List<Command>();
            }

            commands[frame].AddRange(newCommands);
        }
    }
}
