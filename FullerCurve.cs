using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    
    public class FullerCurve
    {
        private double _Dmax;
        private double _Dmin;
        private double _N;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Dmax"></param>
        /// <param name="N"></param>
        /// <param name="Dmin"></param>
        public FullerCurve(double Dmax,  
                           double N,
                           double Dmin)
        {
            _Dmax = Dmax;
            _Dmin = Dmin;
            _N = N;
        }
            
            public double CumPassing(double D)
            {
                double P = 100*Math.Pow((D / _Dmax),_N);
            return P;
            }

        /// <summary>
        /// Calculates the area of the curve segment S1-S2
        /// </summary>
        /// <param name="S1">Lower sieving size</param>
        /// <param name="S2">Upper sieving size</param>
        /// <param name="R">Area ratio of coarse aggregate (around 0.4-0.5)</param>
        /// <param name="ConcreteArea">Area of the concrete section</param>
        /// <returns></returns>
        public double SegmentArea(double S1, double S2, double R, double ConcreteArea)
            {
                double SegmentArea = ((CumPassing(S2) - CumPassing(S1)) / (CumPassing(_Dmax) - CumPassing(_Dmin))) * R * ConcreteArea;
                return SegmentArea;
            }
    }
}
