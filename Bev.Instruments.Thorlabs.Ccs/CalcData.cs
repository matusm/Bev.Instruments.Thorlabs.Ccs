using System.Linq;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class CalcData : ISpectrum
    {
        public double[] Wavelengths => dataPoints.Select(dp => dp.Wavelength).ToArray();
        public double[] AverageValues => dataPoints.Select(dp => dp.Signal).ToArray();
        public double[] NoiseValues => dataPoints.Select(dp => dp.Noise).ToArray();
        public double[] StdDevValues => dataPoints.Select(dp => dp.StdDev).ToArray();
        public int[] Dof => dataPoints.Select(dp => dp.Dof).ToArray();
        public IDataPoint[] DataPoints => dataPoints.Cast<IDataPoint>().ToArray();
        public int NumberOfPoints => dataPoints.Length;

        public CalcData(SpectralData measuredSpectrum)
        {
            dataPoints = new CalcDataPoint[measuredSpectrum.Wavelengths.Length];
            for (int i = 0; i < measuredSpectrum.Wavelengths.Length; i++)
            {
                dataPoints[i] = new CalcDataPoint(measuredSpectrum.Wavelengths[i], measuredSpectrum.AverageValues[i], measuredSpectrum.NoiseValues[i], measuredSpectrum.StdDevValues[i], measuredSpectrum.Dof[i]);
            }
        }

        public CalcData(CalcData other)
        {
            dataPoints = new CalcDataPoint[other.Wavelengths.Length];
            for (int i = 0; i < other.Wavelengths.Length; i++)
            {
                dataPoints[i] = new CalcDataPoint(other.Wavelengths[i], other.AverageValues[i], other.NoiseValues[i], other.StdDevValues[i], other.Dof[i]);
            }
        }

        public CalcData(double[] wavelength, double[] averageValues, double[] noiseValues, double[] stdDevValues)
        {
            dataPoints = new CalcDataPoint[wavelength.Length];
            for (int i = 0; i < wavelength.Length; i++)
            {
                dataPoints[i] = new CalcDataPoint(wavelength[i], averageValues[i], noiseValues[i], stdDevValues[i]);
            }
        }

        public CalcData(double[] wavelength, double[] averageValues, double[] noiseValues, double[] stdDevValues, int[] dof)
        {
            dataPoints = new CalcDataPoint[wavelength.Length];
            for (int i = 0; i < wavelength.Length; i++)
            {
                dataPoints[i] = new CalcDataPoint(wavelength[i], averageValues[i], noiseValues[i], stdDevValues[i], dof[i]);
            }
        }

        private readonly CalcDataPoint[] dataPoints;
    }
}
