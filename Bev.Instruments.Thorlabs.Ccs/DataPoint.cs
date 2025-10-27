namespace Bev.Instruments.Thorlabs.Ccs
{
    public class DataPoint : IDataPoint
    {
        public double Wavelength { get; }
        public double Value { get; }
        public double Sem { get; }
        public double StdDev { get; }
        public int Dof { get; }

        public DataPoint(double wavelength, double value) : this(wavelength, value, 0, 0, int.MaxValue) { }

        public DataPoint(double wavelength, double value, double sem, double stdDev) : this(wavelength, value, sem, stdDev, int.MaxValue) { }

        public DataPoint(double wavelength, double value, double sem, double stdDev, int dof)
        {
            Wavelength = wavelength;
            Value = value;
            Sem = sem;
            StdDev = stdDev;
            Dof = dof;
        }

        public string ToCsvLine() => $"{Wavelength:F2},{Value:F6},{Sem:F6},{StdDev:F6},{Dof}";
        public string GetCsvHeader() => "Wavelength,Value,SEM,StdDev,DoF";
    }
}
