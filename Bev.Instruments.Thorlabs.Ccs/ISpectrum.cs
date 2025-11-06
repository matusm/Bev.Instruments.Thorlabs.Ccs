namespace Bev.Instruments.Thorlabs.Ccs
{
    public interface ISpectrum
    {
        string Name { get; set; }
        double[] Wavelengths { get; }
        double[] Values { get; }
        double[] SemValues { get; }
        double[] StdDevValues { get; }
        IDataPoint[] DataPoints { get; }
        int NumberOfPoints { get; }
    }
}
