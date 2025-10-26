namespace Bev.Instruments.Thorlabs.Ccs
{
    public interface IDataPoint
    {
        double Wavelength { get; }
        double Signal { get; }
        double Sem { get; } // standard error of the mean (SEM)
        double StdDev { get; }
        int Dof { get; }
        string ToCsvLine();
        string GetCsvHeader();
    }
}
