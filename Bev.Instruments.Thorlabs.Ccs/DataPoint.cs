namespace Bev.Instruments.Thorlabs.Ccs
{
    public class DataPoint : IDataPoint
    {
        public double Wavelength { get; }
        public double Signal { get; }
        public double Sem { get; }
        public double StdDev { get; }
        public int Dof { get; }

        public DataPoint(double wavelength, double signal) : this(wavelength, signal, 0, 0, int.MaxValue) { }

        public DataPoint(double wavelength, double signal, double sem, double stdDev) : this(wavelength, signal, sem, stdDev, int.MaxValue) { }

        public DataPoint(double wavelength, double signal, double sem, double stdDev, int dof)
        {
            Wavelength = wavelength;
            Signal = signal;
            Sem = sem;
            StdDev = stdDev;
            Dof = dof;
        }

        public string ToCsvLine() => $"{Wavelength:F2},{Signal:F6},{Sem:F6},{StdDev:F6},{Dof}";
        public string GetCsvHeader() => "Wavelength,Signal,SEM,StdDev,DoF";
    }
}
