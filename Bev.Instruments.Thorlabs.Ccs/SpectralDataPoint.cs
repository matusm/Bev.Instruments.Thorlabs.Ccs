using At.Matus.StatisticPod;
using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class SpectralDataPoint : IDataPoint
    {
        public double Wavelength { get; }
        public double Signal => sp.AverageValue;
        public double Noise => StdDev / Math.Sqrt(Dof + 1);
        public double StdDev => sp.StandardDeviation;
        public double MaxSignal => sp.MaximumValue;
        public double MinSignal => sp.MinimumValue;
        public int Dof => (int)sp.SampleSize - 1;

        public SpectralDataPoint(double wavelength) => Wavelength = wavelength;

        public void UpdateSignal(double signal) => sp.Update(signal);

        public void Clear() => sp.Restart();

        public string ToCsvLine() => $"{Wavelength},{Signal},{Noise},{StdDev},{Dof}";

        private readonly StatisticPod sp = new StatisticPod();

    }
}
