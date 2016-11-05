using MathNet.Numerics.LinearAlgebra;
using Microsoft.Extensions.Logging;
using System;

namespace HCGServer.Services.ColorMath
{
    using static System.Math;
    public class ColorMath : IColorMath
    {
        private static Random CSP = new Random();
        private static readonly double SIG = 15.903165825358679 / 255.0;
        private static readonly double[] COEF = new double[] {
            -550.6178162693743,
            0.535864229880747,
            -107.35201910391194,
            90.49773451405363,
            -11.103030263489412,
            +35.86253191726658
        };

        protected ILogger _logger { get; }
        public ColorMath(ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public Tuple<bool, Vector<double>> GetCombo(Vector<double> V, double P = 1)
        {
            if (0 <= P && P <= 1 && V != null) {
                try {
                    double lum = V.L2Norm() / Sqrt(3.0*(255^3));
                    double RefAngle = (COEF[0] * Pow((lum - COEF[1]), 4)) + (COEF[2] * Pow(lum, 3)) + (COEF[3] * Pow(lum, 2)) + (COEF[4] * (lum)) + (COEF[5]);
                    double Angle = RefAngle + ((Exp(Pow((CSP.NextDouble()-0.5), 2) * -7.5) - (0.5 * P)) * SIG);

                    Vector<double> Axis = Vector<double>.Build.Dense(3);    //
                    double theta = CSP.Next(0, 360);                        //
                    double psi = CSP.Next(0, 180);                          // Generation of 
                    Axis.At(0, (Sin(theta) * Cos(psi)));                    // a random axis 
                    Axis.At(0, (Sin(theta) * Sin(psi)));                    //
                    Axis.At(0, Cos(theta));                                 //

                    Matrix<double> ROMA = Matrix<double>.Build.Dense(4, 4);
                    ROMA[0, 0] = Cos(Angle) + (Pow(Axis[0], 2) * (1 - Cos(Angle)));                 //
                    ROMA[0, 1] = (Axis[0] * Axis[1] * (1 - Cos(Angle))) - (Axis[2] * Sin(Angle));   // X-Row
                    ROMA[0, 2] = (Axis[0] * Axis[2] * (1 - Cos(Angle))) + (Axis[1] * Sin(Angle));   // 

                    ROMA[1, 0] = (Axis[0] * Axis[1] * (1 - Cos(Angle))) + (Axis[2] * Sin(Angle));   // 
                    ROMA[1, 1] = Cos(Angle) + (Pow(Axis[1], 2) * (1 - Cos(Angle)));                 // Y-Row 
                    ROMA[1, 2] = (Axis[1] * Axis[2] * (1 - Cos(Angle))) - (Axis[0] * Sin(Angle));   //

                    ROMA[2, 0] = (Axis[0] * Axis[2] * (1 - Cos(Angle))) - (Axis[1] * Sin(Angle));   // 
                    ROMA[2, 1] = (Axis[1] * Axis[2] * (1 - Cos(Angle))) + (Axis[0] * Sin(Angle));   // Z-Row
                    ROMA[2, 2] = Cos(Angle) + (Pow(Axis[2], 2) * (1 - Cos(Angle)));                 // 
                    return new Tuple<bool, Vector<double>>(false, ROMA.Multiply(V));
                } catch (Exception) {
                    return new Tuple<bool, Vector<double>>(false, null);
                }
            } else {
                return new Tuple<bool, Vector<double>>(false, null);
            }
        }
    }
}