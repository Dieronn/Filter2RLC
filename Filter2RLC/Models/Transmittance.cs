using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Numerics;

namespace Filter2RLC.Models
{
    public class Transmittance
    {
        public double[,] GetTransmittance(FilterParams filter)
        {
            double u1 = filter.U1;
            double fmin = filter.Fmin;
            double fmax = filter.Fmax;
            double R1 = filter.Resistance1;
            double R2 = filter.Resistance2;
            double L1 = filter.Inductance;
            double C1 = filter.Capacitance;
            int size = filter.NumOfRows;
            //---
            double[,] Results = new double[size, 5];
            Complex Z1;
            Complex Z2;
            Complex Z3;
            //Current 
            Complex Z4;
            Complex Z5;
            Complex Z6;
            double f = fmin;
            double df = (fmax - fmin) / (size - 1);
            double omega = 0;
            //---
            for (int i = 0; i < size; i++)
            {
                omega = 2 * Math.PI * f;
                Z1 = new Complex(0, omega*L1);
                Z2 = new Complex(R1 + R2 - omega * omega * R2 * L1 * C1, omega * (L1 + C1 * R2 * R1));
                Z3 = Z1 / Z2;
                //Current 
                Z4 = new Complex(R1+R2-omega*omega*R2*L1*C1, omega*(L1+R1*R2*C1));
                Z5 = new Complex(1 - omega * omega * L1 * C1, omega * C1 * R1);
                Z6 = u1 * Z5 / Z4;

                //Results
                Results[i, 0] = f;
                Results[i, 1] = Z3.Magnitude;
                Results[i, 2] = Z3.Phase;
                Results[i, 3] = Z6.Magnitude;
                Results[i, 4] = -Z6.Magnitude;
                f += df;
            }
            return Results;
        }
    }
}