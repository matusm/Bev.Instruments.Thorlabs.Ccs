using At.Matus.StatisticPod;
using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class MeasuredDataPoint : IDataPoint
    {
        public double Wavelength { get; }
        public double Signal => sp.AverageValue;
        public double Sem => StdDev / Math.Sqrt(Dof + 1);
        public double StdDev => sp.StandardDeviation;
        public double MaxSignal => sp.MaximumValue;
        public double MinSignal => sp.MinimumValue;
        public int Dof => (int)sp.SampleSize - 1;

        public MeasuredDataPoint(double wavelength) => Wavelength = wavelength;

        public void UpdateSignal(double signal) => sp.Update(signal);

        public void Clear() => sp.Restart();

        public string ToCsvLine() => $"{Wavelength:F2},{Signal:F6},{MinSignal:F6},{MaxSignal:F6},{Sem:F6},{StdDev:F6},{Dof}";

        public string GetCsvHeader() => "Wavelength,Signal,MinSignal,MaxSignal,SEM,StdDev,DoF";

        private readonly StatisticPod sp = new StatisticPod();

    }
}
