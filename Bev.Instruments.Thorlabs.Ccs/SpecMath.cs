using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public static class SpecMath
    {

        public static Spectrum Subtract(ISpectrum minuend, ISpectrum subtrahend)
        {
            DataPoint[] newDataPoints = new DataPoint[minuend.NumberOfPoints];
            for (int i = 0; i < newDataPoints.Length; i++)
            {
                IDataPoint pointMin = minuend.DataPoints[i];
                IDataPoint pointSub = subtrahend.DataPoints[i];
                newDataPoints[i] = Subtract(pointMin, pointSub);
            }
            Spectrum diff = new Spectrum(newDataPoints);
            diff.Name = $"[{minuend.Name}] - [{subtrahend.Name}]";
            return diff;
        }

        public static Spectrum Add(ISpectrum first, ISpectrum second)
        {
            DataPoint[] newDataPoints = new DataPoint[first.NumberOfPoints];
            for (int i = 0; i < newDataPoints.Length; i++)
            {
                IDataPoint firstPoint = first.DataPoints[i];
                IDataPoint secondPoint = second.DataPoints[i];
                newDataPoints[i] = Add(firstPoint, secondPoint);
            }
            Spectrum diff = new Spectrum(newDataPoints);
            diff.Name = $"[{first.Name}] + [{second.Name}]";
            return diff;
        }

        public static Spectrum ComputeBiasCorrectedRatio(ISpectrum signal, ISpectrum reference, ISpectrum bckgnd)
        {
            DataPoint[] newDataPoints = new DataPoint[signal.NumberOfPoints];
            for (int i = 0; i < newDataPoints.Length; i++)
            {
                IDataPoint signalPoint = signal.DataPoints[i];
                IDataPoint referencePoint = reference.DataPoints[i];
                IDataPoint bckgndPoint = bckgnd.DataPoints[i];
                newDataPoints[i] = ComputeBiasCorrectedRatio(signalPoint, referencePoint, bckgndPoint);
            }
            Spectrum ratio = new Spectrum(newDataPoints);
            ratio.Name = $"RelBC[{signal.Name}] / [{reference.Name}]";
            return ratio;   
        }

        #region Private Methods
        private static DataPoint Subtract(IDataPoint minuend, IDataPoint subtrahend)
        {
            double newSignal = minuend.Value - subtrahend.Value;
            double newSem = SqSum(minuend.Sem, subtrahend.Sem);
            double newStdDev = SqSum(minuend.StdDev, subtrahend.StdDev);
            return new DataPoint(minuend.Wavelength, newSignal, newSem, newStdDev);
        }

        private static DataPoint Add(IDataPoint first, IDataPoint second)
        {
            double newSignal = first.Value + second.Value;
            double newSem = SqSum(first.Sem, second.Sem);
            double newStdDev = SqSum(first.StdDev, second.StdDev);
            return new DataPoint(first.Wavelength, newSignal, newSem, newStdDev);
        }

        private static DataPoint ComputeBiasCorrectedRatio(IDataPoint signal, IDataPoint reference, IDataPoint bckgnd)
        {
            double correctedSignal = signal.Value - bckgnd.Value;
            double correctedReference = reference.Value - bckgnd.Value;
            double ratio = correctedSignal / correctedReference;
            double newSem = BiasCorrectedRatioUncertainty(signal.Value, reference.Value, bckgnd.Value, signal.Sem, reference.Sem, bckgnd.Sem);
            double newStdDev = BiasCorrectedRatioUncertainty(signal.Value, reference.Value, bckgnd.Value, signal.StdDev, reference.StdDev, bckgnd.StdDev);
            return new DataPoint(signal.Wavelength, ratio, newSem, newStdDev);
        }

        private static double SqSum(double u1, double u2) => Math.Sqrt(u1 * u1 + u2 * u2);

        private static double BiasCorrectedRatioUncertainty(double x, double xr, double xb, double ux, double uxr, double uxb)
        {
            double v1 = 1.0 / (xr - xb);
            double v2 = v1 * v1;
            double u1 = v1 * ux;
            double u2 = v2 * (x - xr) * uxb;
            double u3 = v2 * (xb - x) * uxr;
            return Math.Sqrt((u1 * u1) + (u2 * u2) + (u3 * u3) );
        }
        
        #endregion

    }
}
