namespace Bev.Instruments.Thorlabs.Ccs
{
    public interface IDataPoint
    {
        double Wavelength { get; }
        double Value { get; }
        double Sem { get; } // standard error of the mean (SEM)
        double StdDev { get; }
        string ToCsvLine();
        string GetCsvHeader();
    }
}
