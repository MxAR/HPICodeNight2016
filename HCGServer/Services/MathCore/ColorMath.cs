using MathNet.Numerics.LinearAlgebra;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using System;

namespace HCGServer.Services.ColorMath
{
    using static System.Math;
    public class ColorMath : IColorMath 
    {
        protected ILogger _logger { get; }
        private static RandomNumberGenerator CSP = RandomNumberGenerator.Create();
        private const double CAMedian = 0.7824274226624149; 
        private const double ICASigma = 0.1652309091634334 / 255.0;
        public ColorMath(ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public Vector<double> GetKombo(Vector<double> CV, int PN = 255)
        {
            double Precision = Convert.ToDouble(Min(Max(0, PN), 255)) * ICASigma;
            Vector<double> KV = Vector<double>.Build.Dense(CV.Count);
            do {
                byte[] num = new byte[1];
                for (int i = 0; i < CV.Count; i++) {
                    CSP.GetBytes(num);
                    KV.At(i, Convert.ToDouble(num[0]));
                }
            } while (Abs(CosAngleofVectors(CV, KV) - CAMedian) <= Precision);
            return KV;
        }

        private double CosAngleofVectors(Vector<double> V1, Vector<double> V2)
        {
            return V1.DotProduct(V2) / (V1.L2Norm() * V2.L2Norm()); 
        }
    }
}