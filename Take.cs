using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeAndPlace
{
    public class Take
    {
        private double _Ds1;
        private double _Ds2;

        public Take(double Ds1, double Ds2)
        {
            _Ds1 = Ds1;
            _Ds2 = Ds2;
        }

        public double AggregateSize(Random r)
        {
            double Eta = r.NextDouble();

            double D = _Ds1+Eta*(_Ds2-_Ds1);

            return D;
        }
    }
}
