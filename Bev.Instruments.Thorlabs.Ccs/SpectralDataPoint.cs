using At.Matus.StatisticPod;
using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class SpectralDataPoint
    {
        public double Wavelength;
        public double Signal => sp.AverageValue;
        public double Noise => StdDev/Math.Sqrt(Count);
        public double StdDev => sp.StandardDeviation;
        public double MaxSignal => sp.MaximumValue;
        public double MinSignal => sp.MinimumValue;
        public int Count => (int)sp.SampleSize;

        public SpectralDataPoint(double wavelength) => Wavelength = wavelength;

        public void UpdateSignal(double signal) => sp.Update(signal);

        public void Clear() => sp.Restart();

        private readonly StatisticPod sp = new StatisticPod();

    }
}
