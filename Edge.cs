using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    public class Edge
    {
        private Node _Node1;
        private Node _Node2;
        private double _Length;
        public Edge(Node Node1, Node Node2)
        {
            _Node1 = Node1;
            _Node2 = Node2;
            _Length = Math.Sqrt(Math.Pow(Node2.Y - Node1.Y, 2) + Math.Pow(Node2.X - Node1.X, 2));
        }
        public Node Node1
        {
            get => _Node1;
            set
            {
                _Node1.X = value.X;
                _Node1.Y = value.Y;
            }
        }
        public Node Node2
        {
            get => _Node2;
            set
            {
                _Node2.X = value.X;
                _Node2.Y = value.Y;
            }
        }
        public double Length { get => _Length;}
    }
}
