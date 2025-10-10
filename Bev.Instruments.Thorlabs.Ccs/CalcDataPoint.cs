namespace Bev.Instruments.Thorlabs.Ccs
{
    public class CalcDataPoint
    {
        public double Wavelength;
        public double Signal { get; }
        public double Noise { get; }
        public double StdDev { get; }

        public CalcDataPoint(double wavelength, double signal) : this(wavelength, signal, 0, 0) { }

        public CalcDataPoint(double wavelength, double signal, double noise, double stdDev)
        {
            Wavelength = wavelength;
            Signal = signal;
            Noise = noise;
            StdDev = stdDev;
        }

    }
}
