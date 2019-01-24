using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vector2Int p = (Vector2Int)obj;
                return (x == p.x) && (y == p.y);
            }
        }

        public override string ToString()
        {
            return x.ToString() + "," + y.ToString();
        }
    }
}
