using System.Linq;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class Spectrum : ISpectrum
    {
        public string Name { get; set; } = "Spectrum";
        public double[] Wavelengths => dataPoints.Select(dp => dp.Wavelength).ToArray();
        public double[] AverageValues => dataPoints.Select(dp => dp.Signal).ToArray();
        public double[] SemValues => dataPoints.Select(dp => dp.Sem).ToArray();
        public double[] StdDevValues => dataPoints.Select(dp => dp.StdDev).ToArray();
        public int[] Dof => dataPoints.Select(dp => dp.Dof).ToArray();
        public IDataPoint[] DataPoints => dataPoints.Cast<IDataPoint>().ToArray();
        public int NumberOfPoints => dataPoints.Length;

        // constructor that copies data from another Spectrum or MeasuredSpectrum
        public Spectrum(ISpectrum spectrum)
        {
            Name = spectrum.Name;
            dataPoints = new DataPoint[spectrum.NumberOfPoints];
            for (int i = 0; i < spectrum.NumberOfPoints; i++)
            {
                var dp = spectrum.DataPoints[i];
                dataPoints[i] = new DataPoint(dp.Wavelength, dp.Signal, dp.Sem, dp.StdDev, dp.Dof);
            }
        }

        public Spectrum(DataPoint[] dataPoints) => this.dataPoints = dataPoints;

        public Spectrum(double[] wavelength, double[] averageValues, double[] semValues, double[] stdDevValues)
        {
            dataPoints = new DataPoint[wavelength.Length];
            for (int i = 0; i < wavelength.Length; i++)
            {
                dataPoints[i] = new DataPoint(wavelength[i], averageValues[i], semValues[i], stdDevValues[i]);
            }
        }

        public Spectrum(double[] wavelength, double[] averageValues, double[] semValues, double[] stdDevValues, int[] dof)
        {
            dataPoints = new DataPoint[wavelength.Length];
            for (int i = 0; i < wavelength.Length; i++)
            {
                dataPoints[i] = new DataPoint(wavelength[i], averageValues[i], semValues[i], stdDevValues[i], dof[i]);
            }
        }

        private readonly DataPoint[] dataPoints;
    }
}
