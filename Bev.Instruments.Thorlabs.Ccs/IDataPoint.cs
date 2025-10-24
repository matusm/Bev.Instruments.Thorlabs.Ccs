namespace Bev.Instruments.Thorlabs.Ccs
{
    public interface IDataPoint
    {
        double Wavelength { get; }
        double Signal { get; }
        double Noise { get; }
        double StdDev { get; }
        int Dof { get; }

        string ToCsvLine();
        string GetCsvHeader();
    }
}
