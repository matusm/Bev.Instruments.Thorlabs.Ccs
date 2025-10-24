namespace Bev.Instruments.Thorlabs.Ccs
{
    public interface ISpectrum
    {
        double[] Wavelengths { get; }
        double[] AverageValues { get; }
        double[] NoiseValues { get; }
        double[] StdDevValues { get; }
        int[] Dof { get; }
        IDataPoint[] DataPoints { get; }

        int NumberOfPoints { get; }
    }
}
