using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    public class Place
    {
        Shape _Shape;

        public Place(Shape Shape)
        {
            _Shape = Shape;
        }

        public void GenerateLocation(ref List<Aggregate> agg,Random r, int j)
        {
            double Eta1 = r.NextDouble();
            double Eta2 = r.NextDouble();
            
            double X0 = _Shape.MinX + Eta1 * (_Shape.MaxX - _Shape.MinX);
            double Y0 = _Shape.MinY + Eta2 * (_Shape.MaxY - _Shape.MinY);

            //double alfa = 2 * Math.PI * r.NextDouble();

            double qweqwe = r.NextDouble();

            double alfa = 2 * Math.PI * qweqwe;


            agg[j].Alfa = alfa;
            agg[j].R = qweqwe;
            agg[j].Location = new Node(X0, Y0);
        }

        public bool IsPlaceable(Aggregate Agg, List<Aggregate> AggList)
        {
            // is it within the shape
            if (Agg.MinX<_Shape.MinX || Agg.MaxX>_Shape.MaxX || Agg.MinY < _Shape.MinY || Agg.MaxY > _Shape.MaxY)
            {
                return false;
            }

            // is it overlapping with already placed aggregates
            for (int i = 0; i < AggList.Count-1; i++)
            {
                //Agg.R + AggList[i].R > Distance
                double Distance = CalculateDistance(Agg.Location, AggList[i].Location);
                if (true)
                {
                if (IsOverlapping(Agg,AggList[i]))
                {
                return false;
                }
                else if (IsInside(Agg,AggList[i]))
                {
                return false;
                }
                //calculate two polygon is overlapping or not
                }
            }
            return true;
        }

        public bool IsOverlapping(Aggregate Agg1, Aggregate Agg2)
        {
            double x;
            double y;
            
            for (int i = 0; i < Agg1.ExpEdgeList.Count; i++)
            {
                for (int j = 0; j < Agg2.ExpEdgeList.Count; j++)
                {
                    double x11 = Agg1.ExpEdgeList[i].Node1.X;
                    double x12 = Agg1.ExpEdgeList[i].Node2.X;
                    double x21 = Agg2.ExpEdgeList[j].Node1.X;
                    double x22 = Agg2.ExpEdgeList[j].Node2.X;
                                      
                    double y11 = Agg1.ExpEdgeList[i].Node1.Y;
                    double y12 = Agg1.ExpEdgeList[i].Node2.Y;
                    double y21 = Agg2.ExpEdgeList[j].Node1.Y;
                    double y22 = Agg2.ExpEdgeList[j].Node2.Y;

                    x = ((x11 * y12 - y11 * x12) * (x21 - x22) - (x11 - x12) * (x21 * y22 - y21 * x22)) / ((x11 - x12) * (y21 - y22) - (y11 - y12) * (x21 - x22));
                    y = ((x11 * y12 - y11 * x12) * (y21 - y22) - (y11 - y12) * (x21 * y22 - y21 * x22)) / ((x11 - x12) * (y21 - y22) - (y11 - y12) * (x21 - x22));

                    if ((x>=Math.Min(x11,x12) && x>=Math.Min(x21,x22) && x<=Math.Max(x11,x12) &&
                        x<=Math.Max(x21,x22) && y >= Math.Min(y11, y12) && y >= Math.Min(y21, y22) && 
                        y <= Math.Max(y11, y12) && y <= Math.Max(y21, y22)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsInside(Aggregate Agg1, Aggregate Agg2)
        {
            bool? bPrev;
            bool? b=true;
            if (Agg1.GetArea() < Agg2.GetArea())
            {
                //Agg1.GetConvexShapeNodeList
                for (int i = 0; i < Agg2.ConvexShapeEdgeList.Count; i++)
                {
                    bPrev = b;

                    double v1x = Agg2.ConvexShapeEdgeList[i].Node2.X - Agg2.ConvexShapeEdgeList[i].Node1.X;
                    double v1y = Agg2.ConvexShapeEdgeList[i].Node2.Y - Agg2.ConvexShapeEdgeList[i].Node1.Y;
                    double v2x = Agg1.Location.X - Agg2.ConvexShapeEdgeList[i].Node1.X;
                    double v2y = Agg1.Location.Y - Agg2.ConvexShapeEdgeList[i].Node1.Y;

                    if (v1x * v2y - v2x * v1y < 0)
                    {
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }

                    if (b != bPrev && i !=0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Agg1.ConvexShapeEdgeList.Count; i++)
                {
                    bPrev = b;

                    double v1x = Agg1.ConvexShapeEdgeList[i].Node2.X - Agg1.ConvexShapeEdgeList[i].Node1.X;
                    double v1y = Agg1.ConvexShapeEdgeList[i].Node2.Y - Agg1.ConvexShapeEdgeList[i].Node1.Y;
                    double v2x = Agg2.Location.X - Agg1.ConvexShapeEdgeList[i].Node1.X;
                    double v2y = Agg2.Location.Y - Agg1.ConvexShapeEdgeList[i].Node1.Y;

                    if (v1x * v2y - v2x * v1y < 0)
                    {
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }

                    if (b != bPrev && i != 0)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public double CalculateDistance(Node Node1, Node Node2)
        {
            double Distance = Math.Sqrt((Node1.X - Node2.X) * (Node1.X - Node2.X) + (Node1.Y - Node2.Y) * (Node1.Y - Node2.Y));
            return Distance;
        }
    }
}
