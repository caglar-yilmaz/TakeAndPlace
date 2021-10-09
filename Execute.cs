using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    public class Execute
    {
        private List<Aggregate> _AggList;
        
        public Execute(List<double> SegmentList,
                       double Dmax,
                       double Dmin,
                       double N,
                       double R,
                       Aggregate shape
                       )
        {
            Take take;
            double totArea;
            double aggArea;
            double width;
            Aggregate Agg;
            int j = 0;
   
            Place place = new Place(shape);
            Random r = new Random();
            FullerCurve curve = new FullerCurve(Dmax, N, Dmin);

            _AggList = new List<Aggregate>();

            //List<Aggregate> templist = new List<Aggregate>();
            int count = 0;
            for (int i = SegmentList.Count-1; i > 0; i--)
            {
                totArea = curve.SegmentArea(SegmentList[i-1], SegmentList[i], R, shape.MaxX*shape.MaxY);
                aggArea = totArea - 1;
                take = new Take(SegmentList[i], SegmentList[i-1]);

                while (totArea > aggArea)
                {
                    count = 0;
                    Thread.Sleep(10);
                    Aggregate agg = new Aggregate();
                    _AggList.Add(agg);
                    //_AggList[j].Width = take.AggregateSize();
                    width=take.AggregateSize(r);
                    agg.GenerateShape(0.5, (SegmentList[i - 1]+ SegmentList[i] )/ 2, (SegmentList[i]- SegmentList[i-1]) / 2,width,r);
                    bool flag = true;
                    
                    place.GenerateLocation(ref agg, r);

                    while (flag && totArea > aggArea)
                    {
                        aggArea = agg.GetArea();
                        if (place.IsPlaceable(agg, _AggList))
                        {
                            //_AggList.Add(templist[j]);
                            flag = false;
                            aggArea = agg.GetArea();
                            totArea -= agg.GetArea(agg.ExpEdgeList);
                            j += 1;

                            //_AggList[0].AdjustTheSize(20);
                        }
                        else if (count == 500)
                        { 
                            width = take.AggregateSize(r);
                            agg.GenerateShape(0.5, (SegmentList[i - 1] + SegmentList[i]) / 2, (SegmentList[i] - SegmentList[i - 1]) / 2, width, r);
                            count = 0;
                        }
                        else
                        {
                            agg.Reset();
                            place.GenerateLocation(ref agg,r);
                            count += 1;
                        }
                    }
                }

            }
        }

        public List<Aggregate> AggList
        {
            get => _AggList;
        }
    }
}
