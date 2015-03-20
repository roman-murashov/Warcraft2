using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warcraft2.Application.Core.Units.Commands;
using Warcraft2.Application.Core.Units;
using Microsoft.Xna.Framework;

namespace Warcraft2.Application.Core.Players
{
    public class Player
    {
        public int id = 0;
        public Color color = Color.Blue;
        List<Unit> selectedUnits = new List<Unit>();

        public void IssueGenericCommand(int unitCommandNumber, Point p)
        {
            if(unitCommandNumber == UnitCommand.UnitCommandMove)
            {
                foreach(Unit u in selectedUnits)
                {
                    CommandMove commandMove = new CommandMove(u, p);
                    u.IssueNewCommand(commandMove);
                }
            }
            else if(unitCommandNumber == UnitCommand.UnitCommandAttackMove)
            {
                foreach(Unit u in selectedUnits)
                {
                    CommandAttackMove commandAttackMove = new CommandAttackMove(u, p);
                    u.IssueNewCommand(commandAttackMove);
                }
            }
            else if(unitCommandNumber == UnitCommand.UnitCommandChop)
            {
                foreach(Unit u in selectedUnits)
                {
                    CommandChop commandChop = new CommandChop(u, p);
                    u.IssueNewCommand(commandChop);
                }
            }
        }

        public void SelectUnitRectangle(Rectangle rect)
        {
            selectedUnits.Clear();
            List<GameObject> gameObjects = UnitHelper.GetGameObjectsInArea(rect);
            foreach(GameObject gameObject in gameObjects)
            {
                if(gameObject.GetType() == typeof(Unit))
                {
                    if(gameObject.GetOwnerId() == id)
                    {
                        selectedUnits.Add((Unit)gameObject);
                    }
                }
            }
        }

        public List<Unit> GetSelectedUnits()
        {
            return selectedUnits;
        }
    }
}
