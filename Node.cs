using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    public class Node
    {

        private double _X;
        private double _Y;

        public Node(double X = 0, double Y = 0)
        {
            _X = X;
            _Y = Y;
        }

        public double X { get => _X; set => _X = value; }
        public double Y { get => _Y; set => _Y = value; }

    }
}
