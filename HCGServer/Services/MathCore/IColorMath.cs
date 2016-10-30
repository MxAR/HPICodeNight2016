using MathNet.Numerics.LinearAlgebra;

namespace HCGServer.Services.ColorMath
{
    public interface IColorMath
    {
        Vector<double> GetKombo(Vector<double> CV, int PN = 255);
    }
}