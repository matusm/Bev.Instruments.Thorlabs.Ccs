using System;
using System.Linq;
using At.Matus.StatisticPod;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class MeasuredSpectrum : ISpectrum
    {
        public string Name { get; set; } = "Measured Spectrum";
        public double[] Wavelengths => dataPoints.Select(dp => dp.Wavelength).ToArray();
        public double[] Values => dataPoints.Select(dp => dp.Value).ToArray();
        public double[] SemValues => dataPoints.Select(dp => dp.Sem).ToArray();
        public double[] StdDevValues => dataPoints.Select(dp => dp.StdDev).ToArray();
        public int[] Dof => dataPoints.Select(dp => dp.Dof).ToArray();
        public double[] MaxValues => dataPoints.Select(dp => dp.MaxSignal).ToArray();
        public double[] MinValues => dataPoints.Select(dp => dp.MinSignal).ToArray();
        public IDataPoint[] DataPoints => dataPoints.Cast<IDataPoint>().ToArray();

        public double MaximumValue => GetMaximumValue();
        public double MinimumValue => GetMinimumValue();

        public int NumberOfSpectra => dataPoints[0].Dof + 1;
        public int NumberOfPoints => dataPoints.Length;
        public bool IsOverexposed => MaximumValue >= 1;
        public bool IsEmpty => NumberOfSpectra == 0;

        public MeasuredSpectrum(double[] wavelength)
        {
            dataPoints = wavelength.Select(w => new MeasuredDataPoint(w)).ToArray();
        }

        public void UpdateSignal(double[] values)
        {
            if (values.Length != dataPoints.Length) throw new ArgumentException("Signal array length does not match data points length.");
            for (int i = 0; i < values.Length; i++)
            {
                dataPoints[i].UpdateSignal(values[i]);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < dataPoints.Length; i++)
            {
                dataPoints[i].Clear();
            }
        }

        private double GetMaximumValue()
        {
            StatisticPod sp = new StatisticPod();
            foreach (var dp in dataPoints)
            {
                sp.Update(dp.MaxSignal);
            }
            return sp.MaximumValue;
        }

        private double GetMinimumValue()
        {
            StatisticPod sp = new StatisticPod();
            foreach (var dp in dataPoints)
            {
                sp.Update(dp.MinSignal);
            }
            return sp.MinimumValue;
        }

        private readonly MeasuredDataPoint[] dataPoints;
    }
}
