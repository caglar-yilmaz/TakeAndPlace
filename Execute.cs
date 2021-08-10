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
                    Thread.Sleep(10);
                    _AggList.Add(new Aggregate());
                    //_AggList[j].Width = take.AggregateSize();
                    _AggList[j].GenerateShape(0.5, (SegmentList[i - 1]+ SegmentList[i] )/ 2, (SegmentList[i]- SegmentList[i-1]) / 2,r);

                    bool flag = true;
                    
                    place.GenerateLocation(ref _AggList,r,j);

                    while (flag)
                    {
                        if (place.IsPlaceable(_AggList[j], _AggList))
                        {
                            //_AggList.Add(templist[j]);
                            flag = false;
                            aggArea = _AggList[j].GetArea();
                            totArea -= aggArea;
                            j += 1;
                            count += 1;
                            //_AggList[0].AdjustTheSize(20);
                        }
                        else
                        {
                          
                            _AggList[j].Reset();
                            place.GenerateLocation(ref _AggList,r, j);
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
