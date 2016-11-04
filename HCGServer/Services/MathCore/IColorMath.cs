using MathNet.Numerics.LinearAlgebra;
using System;

namespace HCGServer.Services.ColorMath
{
    public interface IColorMath
    {
        Tuple<bool, Vector<double>> GetCombo(Vector<double> V, double P = 1);
    }
}