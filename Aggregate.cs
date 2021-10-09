using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    public class Aggregate : Shape
    {
        private double _Width;
        private double _R;
        private Node _Location;
        private List<Node> NodeArray;
        private List<Edge> _ExpEdgeList;
        private List<Edge> _ConvexShapeEdgeList;

        public double R { get => _R; set => _R = value; }
        public List<Edge> ExpEdgeList { get => _ExpEdgeList; }
        public List<Edge> ConvexShapeEdgeList { get => _ConvexShapeEdgeList; }

        public double Width
        {
            get => _Width;
            set
            {
                _Width = value;
            }
        }

        public Node Location
        {
            get => _Location;
            set
            {
                _Location.X = value.X;
                _Location.Y = value.Y;

                UpdateEdgeList(value);

            }
        }

        /// <summary>
        /// if a new location is appended to the aggregate lambda value shouldnt be assigned. If lambda is assigned, size adjustment is done according to the lambda value.
        /// </summary>
        /// <param name="value">Shifting value of the aggregate</param>
        /// <param name="lambda">Ratio of prescribed width value to width of the aggregate, this parameter should be used if one wants to adjust the size of the aggregate</param>
        private void UpdateEdgeList(Node value=null, double lambda = 0)
        {
            if (lambda == 0)
            {

                double x2;
                double y2;
                MinX = double.MaxValue;
                MaxX = double.MinValue;
                MinY = double.MaxValue;
                MaxY = double.MinValue;

                for (int i = 0; i < EdgeList.Count; i++)
                {
                    x2 = Math.Cos(Alfa) * EdgeList[i].Node1.X - Math.Sin(Alfa) * EdgeList[i].Node1.Y;
                    y2 = Math.Sin(Alfa) * EdgeList[i].Node1.X + Math.Cos(Alfa) * EdgeList[i].Node1.Y;

                    EdgeList[i].Node1.X = x2 + value.X;
                    EdgeList[i].Node1.Y = y2 + value.Y;

                    if (MaxX < EdgeList[i].Node1.X)
                    {
                        MaxX = EdgeList[i].Node1.X;
                    }
                    if (MaxY < EdgeList[i].Node1.Y)
                    {
                        MaxY = EdgeList[i].Node1.Y;
                    }
                    if (MinX > EdgeList[i].Node1.X)
                    {
                        MinX = EdgeList[i].Node1.X;
                    }
                    if (MinY > EdgeList[i].Node1.Y)
                    {
                        MinY = EdgeList[i].Node1.Y;
                    }
                }
            }
            else
            {
                for (int i = 0; i < EdgeList.Count; i++)
                {
                    EdgeList[i].Node1.X = lambda * (EdgeList[i].Node1.X - Location.X) + Location.X;
                    EdgeList[i].Node1.Y = lambda * (EdgeList[i].Node1.Y - Location.Y) + Location.Y;
                }
            }
            GenerateExpEdgeList();
            GenerateConvexShapeEdgeList();
        }
        public Aggregate()
        {
            _Location = new Node();
            NodeArray = new List<Node>();
            _ExpEdgeList = new List<Edge>();
            _ConvexShapeEdgeList = new List<Edge>();
        }

        public double GenerateShape(double delta, double A0, double A1,double width, Random r)
        {
            NodeList.Clear();
            NodeArray.Clear();

            double a = 4 + 6 * r.NextDouble();
            int n = Convert.ToInt32(Math.Round(a));
            List<double> PhiList = new List<double>();
            List<double> RList = new List<double>();
            double sum = 0;
            double teta = 0;

            for (int i = 0; i < n; i++)
            {
                double Ni = 2 * Math.PI * r.NextDouble();
                double Nj = r.NextDouble();

                PhiList.Add(2 * Math.PI / n + (2 * Ni - 1) * delta * 2 * Math.PI / n);
                RList.Add(A0 + (2 * Nj - 1) * A1);
                sum += PhiList[i];
            }

            for (int i = 0; i < n; i++)
            {
                PhiList[i] = PhiList[i] * (2 * Math.PI / sum);
                teta += PhiList[i];
                double X = RList[i] * Math.Cos(teta);
                double Y = RList[i] * Math.Sin(teta);

                NodeList.Add(new Node(X, Y));
                NodeArray.Add(new Node(X, Y));
            }
            CalculateR();
            GenerateEdges();

            /// TODO
            AdjustTheSize(width);
            return 0;
        }

        public void Reset()
        {
            _Location.X = 0;
            _Location.Y = 0;
            EdgeList.Clear();
            NodeList.Clear();
            _ExpEdgeList.Clear();

            for (int i = 0; i < NodeArray.Count; i++)
            {
                NodeList.Add(new Node());
                NodeList[i].X = NodeArray[i].X;
                NodeList[i].Y = NodeArray[i].Y;
            }
            GenerateEdges();
        }

        private void GenerateEdges()
        {
            EdgeList.Clear();

            if (NodeList.Count != 0)
            {
                for (int i = 0; i < NodeList.Count - 1; i++)
                {
                    EdgeList.Add(new Edge(NodeList[i], NodeList[i + 1]));
                }
                EdgeList.Add(new Edge(NodeList[NodeList.Count - 1], NodeList[0]));

            }
            GenerateExpEdgeList();
            GenerateConvexShapeEdgeList();
        }

        public void GenerateExpEdgeList()
        {
            double thickness = 1.5;
            List<Node> expNodeList = new List<Node>();
            _ExpEdgeList.Clear();

            expNodeList.Add(GenerateExpNode(EdgeList.Last(), EdgeList.First(), thickness));
            for (int i = 0; i < EdgeList.Count - 1; i++)
            {
                expNodeList.Add(GenerateExpNode(EdgeList[i], EdgeList[i + 1], thickness));
            }

            for (int i = 0; i < expNodeList.Count - 1; i++)
            {
                _ExpEdgeList.Add(new Edge(expNodeList[i], expNodeList[i + 1]));
            }
            _ExpEdgeList.Add(new Edge(expNodeList[NodeList.Count - 1], expNodeList[0]));

        }

        public double Norm(Node node)
        {
            double norm = Math.Sqrt(Math.Pow(node.X, 2) + Math.Pow(node.Y, 2));
            return norm;
        }

        private Node GenerateExpNode(Edge edge1, Edge edge2, double t)
        {
            Node node = new Node();
            Node V1 = new Node(edge1.Node1.X - edge1.Node2.X, edge1.Node1.Y - edge1.Node2.Y);
            Node V2 = new Node(edge2.Node2.X - edge2.Node1.X, edge2.Node2.Y - edge2.Node1.Y);
            double Angle = Math.Acos((V1.X * V2.X + V1.Y * V2.Y) / (Norm(V1) * Norm(V2)));
            Node Bisector = new Node(Norm(V1) * V2.X + Norm(V2) * V1.X, Norm(V1) * V2.Y + Norm(V2) * V1.Y);
            double norm = (-Norm(Bisector));
            Bisector.X /= norm;
            Bisector.Y /= norm;

            if (IsConvex(edge1.Node1,edge2.Node1,edge2.Node2))
            {
                node.X = edge1.Node2.X + t * Bisector.X / Math.Sin(Angle / 2);
                node.Y = edge1.Node2.Y + t * Bisector.Y / Math.Sin(Angle / 2);
            }
            else
            {
                node.X = edge1.Node2.X - t * Bisector.X / Math.Sin(Angle / 2);
                node.Y = edge1.Node2.Y - t * Bisector.Y / Math.Sin(Angle / 2);
            }
            
            return node;
        }

        private void CalculateR()
        {
            double Distance = 0;
            //Node Center = GetCentroid();

            for (int i = 0; i < NodeList.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(NodeList[i].X - _Location.X, 2) + Math.Pow(NodeList[i].Y - _Location.Y, 2)) > Distance)
                {
                    Distance = Math.Sqrt(Math.Pow(NodeList[i].X - _Location.X, 2) + Math.Pow(NodeList[i].Y - _Location.Y, 2));
                }

            }
            _R = Distance;
        }

        private void GenerateConvexShapeEdgeList()
        {
            _ConvexShapeEdgeList.Clear();
            List<Node> ConvNodeList;
            ConvNodeList = GetConvexShapeNodeList();
            
            if (ConvNodeList.Count != 0)
            {
                for (int i = 0; i < ConvNodeList.Count - 1; i++)
                {
                    _ConvexShapeEdgeList.Add(new Edge(ConvNodeList[i], ConvNodeList[i + 1]));
                }
                _ConvexShapeEdgeList.Add(new Edge(ConvNodeList[ConvNodeList.Count - 1], ConvNodeList[0]));
            }
        }
        
        private bool IsConvex(Node node1,Node node2, Node node3)
        {
            //edge listesi ccw, ona göre değerlendiriyor.
            bool flag;
            Node v1 = new Node();
            Node v2 = new Node();

            v1.X = node2.X - node1.X;
            v1.Y = node2.Y - node1.Y;

            v2.X = node3.X - node2.X;
            v2.Y = node3.Y - node2.Y;

            if (v1.X * v2.Y - v1.Y * v2.X > 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private List<Node> CloneNodeList(List<Edge> elist)
        {
            List<Node> copylist = new List<Node>();

            for (int i = 0; i < elist.Count; i++)
            {
                copylist.Add(new Node(elist[i].Node1.X, elist[i].Node1.Y));
            }
            return copylist;
        }

        private List<Node> GetConvexShapeNodeList()
        {
            List<Node> ConvNodeList = new List<Node>();
            List<Node> CopiedNodeList = CloneNodeList(EdgeList);
            
            //TODO edgelistin cloneeunu oluştur, convexliği bozanı listeden sil
            //Node v1 = new Node();
            //Node v2 = new Node();
            int i = 0;
            int iConv = 0;
            
            if (IsConvex(CopiedNodeList[CopiedNodeList.Count-1], CopiedNodeList[0],CopiedNodeList[1]))
            {
                ConvNodeList.Add(new Node());
                ConvNodeList[0].X = CopiedNodeList[0].X;
                ConvNodeList[0].Y = CopiedNodeList[0].Y;
                iConv += 1;
            }
            else
            {
                CopiedNodeList.RemoveAt(0);
            }

            while (i < CopiedNodeList.Count-2)
            {
                if (IsConvex(CopiedNodeList[i], CopiedNodeList[i+1], CopiedNodeList[i+2]))
                {
                    ConvNodeList.Add(new Node());
                    ConvNodeList[iConv].X = CopiedNodeList[i + 1].X;
                    ConvNodeList[iConv].Y = CopiedNodeList[i + 1].Y;
                    i += 1;
                    iConv += 1;
                }
                else
                {
                    CopiedNodeList.RemoveAt(i + 1);
                }
            }

            if (IsConvex(CopiedNodeList[CopiedNodeList.Count - 2], CopiedNodeList[CopiedNodeList.Count-1], CopiedNodeList[0]))
            {
                ConvNodeList.Add(new Node());
                ConvNodeList[iConv].X = CopiedNodeList[CopiedNodeList.Count-1].X;
                ConvNodeList[iConv].Y = CopiedNodeList[CopiedNodeList.Count-1].Y;
            }
            
           
            return ConvNodeList;
        }

        /// <summary>
        /// This method transforms coordinates of "convex shape" about its center by the angle between x axis and given edge. 
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        private List<Node> TransformCoordinates(Edge edge)
        {
            double alfa;
            Node center;
            center = GetCentroid(ConvexShapeEdgeList);
            alfa =- Math.Asin((- (edge.Node2.Y-edge.Node1.Y)) / edge.Length);
            
            //-----------------------------------------------------------------------------------------------------------------------
            //the sign of the angle comes up wrong for some edges, I couldnt figure it out how to overcome this problem.
            //thats why I check whether its angle is 0 or not if not I take the negative of the alfa 
            double edgeNode1Y = (edge.Node1.X - center.X)* Math.Sin(alfa) + (edge.Node1.Y -center.Y)* Math.Cos(alfa) + center.Y;
            double edgeNode2Y= (edge.Node2.X - center.X) * Math.Sin(alfa) + (edge.Node2.Y - center.Y) * Math.Cos(alfa) + center.Y;
            if (Math.Abs(edgeNode2Y-edgeNode1Y)>0.01)
            {
                alfa = -alfa;
            }
            //-----------------------------------------------------------------------------------------------------------------------

            List<Node> Transformed = new List<Node>();
            List<Node> ConvNodeList = GetConvexShapeNodeList();

            for (int i = 0; i < ConvNodeList.Count; i++)
            {
                Transformed.Add(new Node());
                Transformed[i].X = (ConvNodeList[i].X-center.X) * Math.Cos(alfa) - (ConvNodeList[i].Y-center.Y) * Math.Sin(alfa) +center.X;
                Transformed[i].Y = (ConvNodeList[i].X - center.X)* Math.Sin(alfa) + (ConvNodeList[i].Y -center.Y)* Math.Cos(alfa) + center.Y;
            }


            return Transformed;
        }

        private double CalculateWidth(List<Node> VertexList)
        {
            double w;
            Node minx = new Node();
            Node miny = new Node();
            Node maxx = new Node();
            Node maxy = new Node();

            minx.X = double.MaxValue;
            miny.Y = double.MaxValue;
            maxx.X = double.MinValue;
            maxy.Y = double.MinValue;

            for (int i = 0; i < VertexList.Count; i++)
            {
                minx = (VertexList[i].X < minx.X) ? VertexList[i] : minx;
                miny = (VertexList[i].Y < miny.Y) ? VertexList[i] : miny;
                maxx = (VertexList[i].X > maxx.X) ? VertexList[i] : maxx;
                maxy = (VertexList[i].Y > maxy.Y) ? VertexList[i] : maxy;
            }

            w = ((maxx.X - minx.X) < (maxy.Y - miny.Y)) ? (maxx.X - minx.X) : (maxy.Y - miny.Y);
            return w;
        }

        private double GetMinWidth()
        {
            double minw=double.MaxValue;
            double w;
            List<Node> minWidthNodeList;
            List<Node> NList;
            for (int i = 0; i < ConvexShapeEdgeList.Count; i++)
            {
                NList = TransformCoordinates(ConvexShapeEdgeList[i]);
                w = CalculateWidth(NList);
                if (w<minw)
                {
                    minw = w;
                    minWidthNodeList = TransformCoordinates(ConvexShapeEdgeList[i]);
                }
            }

            return minw;
        }

        private void AdjustTheSize(double width)
        {
            double minw;
            double lambda;

            minw = GetMinWidth();
            lambda = width / minw;
            //UpdateEdgeList(lambda: lambda);
            UpdateNodeArray(lambda);
            Reset();
            double ses = GetMinWidth();
        }


        /// <summary>
        /// Nodelist is designed to be contain local coordinates, thats why this method should only be called after the size adjustment at the beginning.
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        private void UpdateNodeArray(double lambda)
        {
            for (int i = 0; i < NodeArray.Count; i++)
            {
                NodeArray[i].X = lambda * (NodeArray[i].X ) ;
                NodeArray[i].Y = lambda * (NodeArray[i].Y ) ;
            }
        }

    }
}
