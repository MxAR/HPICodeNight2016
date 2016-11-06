using MathNet.Numerics.LinearAlgebra;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace HCGServer.Services.ColorMath
{
    using static System.Math;
    public class ColorMath : IColorMath
    {
        private static Random CSP = new Random();
        private static readonly double SIG = 15.903165825358679 / 255.0;
        private static readonly double[] COEF = new double[] {
            113.32578213409947,
            -0.08078343013936262,
            -36.246805457141605,
            -90.70399420073976,
            57.738600082142526,
            -36.08078343013936
        };
        protected ILogger _logger { get; }
        public ColorMath(ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        /// <summary>
        /// Returns matching color vector for a given color vector 
        /// </summary>
        /// <param name="V"> reference (input) vector </param>
        /// <param name="P"> precision of the calculation </param>
        /// <returns></returns>
        public Tuple<bool, Vector<double>> GetCombo(Vector<double> V, double P = 255)
        {
            if (0 <= P && P <= 255 && V != null) {
                try {
                    double lum = V.L2Norm() / Sqrt(3.0*(Pow(255, 2)));
                    double RefAngle = (COEF[0] * Pow((lum - COEF[1]), 4)) + (COEF[2] * Pow(lum, 3)) + (COEF[3] * Pow(lum, 2)) + (COEF[4] * (lum)) + (COEF[5]);
                    double Angle = RefAngle + (Exp(Pow((CSP.NextDouble()-0.5), 2) * -7.5) * P * SIG);
                    Vector<double> OV = Vector<double>.Build.Dense(3);

                    do {
                        Vector<double> Axis = Vector<double>.Build.Dense(3);                //
                        double theta = (Exp(Pow((CSP.NextDouble()-0.5), 2) * -20) * 360);   //
                        double psi = (Exp(Pow((CSP.NextDouble()-0.5), 2) * -20) * 180);     // Generation of 
                        Axis.At(0, (Sin(theta) * Cos(psi)));                                // a random axis 
                        Axis.At(1, (Sin(theta) * Sin(psi)));                                //
                        Axis.At(2, Cos(theta));                                             //

                        Matrix<double> ROMA = Matrix<double>.Build.Dense(3, 3);
                        ROMA[0, 0] = Cos(Angle) + (Pow(Axis[0], 2) * (1 - Cos(Angle)));                 //
                        ROMA[0, 1] = (Axis[0] * Axis[1] * (1 - Cos(Angle))) - (Axis[2] * Sin(Angle));   // X-Row
                        ROMA[0, 2] = (Axis[0] * Axis[2] * (1 - Cos(Angle))) + (Axis[1] * Sin(Angle));   // 

                        ROMA[1, 0] = (Axis[0] * Axis[1] * (1 - Cos(Angle))) + (Axis[2] * Sin(Angle));   // 
                        ROMA[1, 1] = Cos(Angle) + (Pow(Axis[1], 2) * (1 - Cos(Angle)));                 // Y-Row 
                        ROMA[1, 2] = (Axis[1] * Axis[2] * (1 - Cos(Angle))) - (Axis[0] * Sin(Angle));   //

                        ROMA[2, 0] = (Axis[0] * Axis[2] * (1 - Cos(Angle))) - (Axis[1] * Sin(Angle));   // 
                        ROMA[2, 1] = (Axis[1] * Axis[2] * (1 - Cos(Angle))) + (Axis[0] * Sin(Angle));   // Z-Row
                        ROMA[2, 2] = Cos(Angle) + (Pow(Axis[2], 2) * (1 - Cos(Angle)));                 //
                        OV = ROMA.Multiply(V);
                    } while(!IsCPositive(OV));

                    double MAX = OV.Maximum();
                    if (MAX > 255.0) { OV = OV.Divide(MAX); OV = OV.Multiply(255); }

                    return new Tuple<bool, Vector<double>>(true, OV);
                } catch (Exception ex) {
                    _logger.LogCritical(ex.ToString());
                    return new Tuple<bool, Vector<double>>(false, null);
                }
            } else {
                _logger.LogWarning("Aborted the calculation of a color-combo, due to flawed parameters!");
                return new Tuple<bool, Vector<double>>(false, null);
            }
        }

        /// <summary>
        /// Validation of a vector
        /// </summary>
        /// <param name="V"></param>
        /// <returns></returns>
        private bool IsCPositive(Vector<double> V)
        {
            bool R = true;
            for (int i = 0; i < V.Count; i++) { R = R && V[i] >= 0; }
            return R;
        }
    }
}