using System;
using MathNet.Numerics.LinearAlgebra;
using System.Globalization;

namespace HCGServer.Services.HexConverter
{
    using Microsoft.Extensions.Logging;
    using static System.Math;
    public class HexConverter : IHexConverter
    {
        protected ILogger _logger { get; }
        public HexConverter(ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public string Vector2Hex(Vector<double> CV)
        {
            if (CV != null) {
                string CC = string.Empty;
                for (int i = 0; i < CV.Count; i++) { CC = string.Concat(CC, CV[i].ToString("X")); }
                return CC;
            } else {
                return string.Empty;
            }
        }

        public Vector<double> Hex2Vector(string CC)
        {
            if (string.IsNullOrEmpty(CC)) {
                return null;
            } else {
                CC = CC.Trim(new char[]{ '#', '"' });
                if (CC.Length == 3 || CC.Length == 4) { 
                    CC = String.Concat(CC, CC); 
                }

                if (CC.Length == 6 || CC.Length == 8) {
                    int VecSize = Convert.ToInt32(Round(CC.Length*0.5));
                    Vector<double> OV = Vector<double>.Build.Dense(VecSize);
                    for (int i = 0; i < VecSize; i++) { OV.At(i, Convert.ToDouble(Int32.Parse(CC.Substring(i*2, i*2+1), NumberStyles.HexNumber))); }
                    return OV;
                } else {
                    return null;
                }
            }
        }
    }
}