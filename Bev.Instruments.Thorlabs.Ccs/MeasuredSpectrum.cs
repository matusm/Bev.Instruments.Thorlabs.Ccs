using System;
using System.Linq;
using At.Matus.StatisticPod;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class MeasuredSpectrum : ISpectrum
    {
        public string Name { get; set; } = "Measured Spectrum";
        public double[] Wavelengths => dataPoints.Select(dp => dp.Wavelength).ToArray();
        public double[] AverageValues => dataPoints.Select(dp => dp.Signal).ToArray();
        public double[] SemValues => dataPoints.Select(dp => dp.Sem).ToArray();
        public double[] StdDevValues => dataPoints.Select(dp => dp.StdDev).ToArray();
        public int[] Dof => dataPoints.Select(dp => dp.Dof).ToArray();
        public double[] MaxValues => dataPoints.Select(dp => dp.MaxSignal).ToArray();
        public double[] MinValues => dataPoints.Select(dp => dp.MinSignal).ToArray();
        public IDataPoint[] DataPoints => dataPoints.Cast<IDataPoint>().ToArray();

        public double MaximumSignal => GetMaximumSignal();
        public int NumberOfSpectra => dataPoints[0].Dof + 1;
        public int NumberOfPoints => dataPoints.Length;
        public bool IsOverexposed => MaximumSignal >= 1;
        public bool IsEmpty => NumberOfSpectra == 0;

        public MeasuredSpectrum(double[] wavelength)
        {
            dataPoints = wavelength.Select(w => new MeasuredDataPoint(w)).ToArray();
        }

        public void UpdateSignal(double[] signal)
        {
            if (signal.Length != dataPoints.Length) throw new ArgumentException("Signal array length does not match data points length.");
            for (int i = 0; i < signal.Length; i++)
            {
                dataPoints[i].UpdateSignal(signal[i]);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < dataPoints.Length; i++)
            {
                dataPoints[i].Clear();
            }
        }

        private double GetMaximumSignal()
        {
            StatisticPod sp = new StatisticPod();
            foreach (var dp in dataPoints)
            {
                sp.Update(dp.MaxSignal);
            }
            return sp.MaximumValue;
        }

        private readonly MeasuredDataPoint[] dataPoints;
    }
}
