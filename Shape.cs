using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    public abstract class Shape
    {
        private List<Node> _NodeList;
        private List<Edge> _EdgeList;
        private double _Alfa;
        private double _MinX;
        private double _MaxX;
        private double _MinY;
        private double _MaxY;
        private Node _Center;
        public Shape()
        {
            _NodeList = new List<Node>();
            _EdgeList = new List<Edge>();
            _Center = new Node();
        }

        /// <summary>
        /// nodelist is
        /// </summary>
        public List<Node> NodeList { get => _NodeList; set => _NodeList = value; }
        public double Alfa { get => _Alfa; set => _Alfa = value; }

        public double MinX { get => _MinX; set => _MinX = value; }
        public double MinY { get => _MinY; set => _MinY = value; }
        public double MaxX { get => _MaxX; set => _MaxX = value; }
        public double MaxY { get => _MaxY; set => _MaxY = value; }
        public double NVertex { get => _NodeList.Count; }
        public Node Center { get => _Center; }

        public List<Edge> EdgeList { get => _EdgeList; }

        public double GetArea(List<Edge> elist = null)
        {
            double area = 0;
            if (elist == null)
            {
                for (int i = 0; i < _EdgeList.Count; i++)
                {
                    area += _EdgeList[i].Node1.X * _EdgeList[i].Node2.Y - _EdgeList[i].Node1.Y * _EdgeList[i].Node2.X;
                }
            }
            else
            {
                for (int i = 0; i < elist.Count; i++)
                {
                    area += elist[i].Node1.X * elist[i].Node2.Y - elist[i].Node1.Y * elist[i].Node2.X;
                }
            }
            area = Math.Abs(area) / 2;
            return area;
        }
        public Node GetCentroid(List<Edge> elist = null)
        {
            Node center = new Node();
            double x = 0;
            double y = 0;
            double area;
            if (elist == null)
            {
                area = GetArea();
    
                for (int i = 0; i < _EdgeList.Count; i++)
                {
                    x += (_EdgeList[i].Node1.X + _EdgeList[i].Node2.X) * (_EdgeList[i].Node1.X * _EdgeList[i].Node2.Y - _EdgeList[i].Node2.X * _EdgeList[i].Node1.Y);
                    y += (_EdgeList[i].Node1.Y + _EdgeList[i].Node2.Y) * (_EdgeList[i].Node1.X * _EdgeList[i].Node2.Y - _EdgeList[i].Node2.X * _EdgeList[i].Node1.Y);
    
                }

            }
            else
            {
                area = GetArea(elist);

                for (int i = 0; i < elist.Count; i++)
                {
                    x += (elist[i].Node1.X + elist[i].Node2.X) * (elist[i].Node1.X * elist[i].Node2.Y - elist[i].Node2.X * elist[i].Node1.Y);
                    y += (elist[i].Node1.Y + elist[i].Node2.Y) * (elist[i].Node1.X * elist[i].Node2.Y - elist[i].Node2.X * elist[i].Node1.Y);

                }
            }
            x = x / (6 * area);
            y = y / (6 * area);

            center.X = x;
            center.Y = y;

            _Center.X = x;
            _Center.Y = y;
            return center;
        }

    }
}
