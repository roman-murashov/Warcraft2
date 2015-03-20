using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Warcraft2.Application.Core.Units
{
    public class UnitPath
    {
        List<Point> pathList = new List<Point>();

        public void Clear()
        {
            pathList.Clear();
        }

        public void AddPoint(Point p)
        {
            pathList.Add(p);
        }

        public void AddPoint(int x, int y)
        {
            pathList.Add(new Point(x, y));
        }

        public void RemovePoint(int index)
        {
            if(pathList.Count > index)
                pathList.RemoveAt(index);
        }

        public Boolean HasNext()
        {
            if(pathList != null)
                return pathList.Count > 0;
            return false;
        }

        public Point GetNext()
        {
            Point p = pathList[0];
            pathList.RemoveAt(0);
            return p;
        }
    }
}
