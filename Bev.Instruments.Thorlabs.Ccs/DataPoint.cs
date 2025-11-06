namespace Bev.Instruments.Thorlabs.Ccs
{
    public class DataPoint : IDataPoint
    {
        public double Wavelength { get; }
        public double Value { get; }
        public double Sem { get; }
        public double StdDev { get; }

        public DataPoint(double wavelength, double value) : this(wavelength, value, 0, 0) { }

        public DataPoint(double wavelength, double value, double sem, double stdDev)
        {
            Wavelength = wavelength;
            Value = value;
            Sem = sem;
            StdDev = stdDev;
        }

        public string ToCsvLine() => $"{Wavelength:F2},{Value:F6},{Sem:F6},{StdDev:F6}";
        public string GetCsvHeader() => "Wavelength,Value,SEM,StdDev";
    }
}
