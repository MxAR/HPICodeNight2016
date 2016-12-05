using MathNet.Numerics.LinearAlgebra;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;

namespace HCGServer.Services.HexConverter
{
    using static System.Math;
    public class HexConverter : IHexConverter
    {
        protected ILogger _logger { get; }
        public HexConverter(ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }
        
        /// <summary>
        /// converts Vector to an even hex string
        /// </summary>
        /// <param name="CV"> double vector </param>
        /// <returns></returns>
        public string Vector2Hex(Vector<double> CV)
        {
            if (CV != null) {
                string CC = string.Empty;
                string sw = string.Empty;
                for (int i = 0; i < CV.Count; i++) { 
                    sw = Convert.ToInt32(Floor(CV[i])).ToString("X"); 
                    sw = sw.Length == 1 ? string.Concat(sw, sw) : sw;
                    CC = string.Concat(CC, sw); 
                }
                return CC;
            } else {
                _logger.LogWarning("Couldn't convert vector to hex-string, due to missingness of a vector!");
                return string.Empty;
            }
        }

        /// <summary>
        /// Convert 
        /// </summary>
        /// <param name="CC"> hex string </param>
        /// <returns></returns>
        public Vector<double> Hex2Vector(string CC)
        {
            if (string.IsNullOrEmpty(CC)) { return null; } else {
                if (CC.Length < 6){ CC = string.Concat(CC, CC); } else {
                    if (CC.Length % 2 == 1) {
                        CC = string.Concat(CC, CC); 
                    }
                }
                int VecSize = Convert.ToInt32(Round(CC.Length*0.5));
                Vector<double> OV = Vector<double>.Build.Dense(VecSize);
                List<string> HEX = Split(CC, 2);
                for (int i = 0; i < HEX.Count; i++) { OV[i] = Convert.ToDouble(int.Parse(HEX[i], NumberStyles.HexNumber)); }
                return OV;
            }
        }

        /// <summary>
        /// Generates a random color vector
        /// </summary>
        /// <returns></returns>
        public Vector<double> RRGBVector()
        { 
            Vector<double> V = Vector<double>.Build.Dense(3);
            Random R = new Random(); 
            V[0] = R.Next(0, 255);
            V[1] = R.Next(0, 255);
            V[2] = R.Next(0, 255);
            return V;
        }

        private static List<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }
    }
}