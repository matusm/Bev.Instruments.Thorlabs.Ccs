namespace Bev.Instruments.Thorlabs.Ccs
{
    public class DataPoint : IDataPoint
    {
        public double Wavelength { get; }
        public double Signal { get; }
        public double Noise { get; }
        public double StdDev { get; }
        public int Dof { get; }

        public DataPoint(double wavelength, double signal) : this(wavelength, signal, 0, 0, int.MaxValue) { }

        public DataPoint(double wavelength, double signal, double noise, double stdDev) : this(wavelength, signal, noise, stdDev, int.MaxValue) { }

        public DataPoint(double wavelength, double signal, double noise, double stdDev, int dof)
        {
            Wavelength = wavelength;
            Signal = signal;
            Noise = noise;
            StdDev = stdDev;
            Dof = dof;
        }

        public string ToCsvLine() => $"{Wavelength:F2},{Signal:F6},{Noise:F6},{StdDev:F6},{Dof}";
        public string GetCsvHeader() => "Wavelength,Signal,Noise,StdDev,DoF";
    }
}
