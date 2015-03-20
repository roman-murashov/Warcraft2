using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warcraft2.Application.Networking.PlayerCommands
{
    public abstract class Command : CommandInterface
    {
        public void DoFrame()
        {
            throw new NotImplementedException();
        }
    }
}
