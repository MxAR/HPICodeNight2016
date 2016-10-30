using MathNet.Numerics.LinearAlgebra;

namespace HCGServer.Services.HexConverter
{
    public interface IHexConverter
    {
        string Vector2Hex(Vector<double> cv);

        Vector<double> Hex2Vector(string cc);
    }
}