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
            var diff = new Spectrum(newDataPoints);
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
            var diff = new Spectrum(newDataPoints);
            diff.Name = $"[{first.Name}] + [{second.Name}]";
            return diff;
        }


        public static Spectrum RelXXX(ISpectrum signal, ISpectrum reference, ISpectrum bckgnd)
        {
            double[] wl = new double[signal.Wavelengths.Length];
            double[] newSignal = new double[signal.Wavelengths.Length];
            double[] newNoise = new double[signal.Wavelengths.Length];
            double[] newStdDev = new double[signal.Wavelengths.Length];
            for (int i = 0; i < signal.Wavelengths.Length; i++)
            {
                wl[i] = signal.Wavelengths[i];
                newSignal[i] = (signal.AverageValues[i] - bckgnd.AverageValues[i])/(reference.AverageValues[i] - bckgnd.AverageValues[i]);
                newNoise[i] = RelUncXXX(signal.AverageValues[i], reference.AverageValues[i], bckgnd.AverageValues[i], signal.SemValues[i], reference.SemValues[i], bckgnd.SemValues[i]);
                newStdDev[i] = RelUncXXX(signal.AverageValues[i], reference.AverageValues[i], bckgnd.AverageValues[i], signal.StdDevValues[i], reference.StdDevValues[i], bckgnd.StdDevValues[i]); ;
            }
            return new Spectrum(wl, newSignal, newNoise, newStdDev);
        }

        private static DataPoint Subtract(IDataPoint minuend, IDataPoint subtrahend)
        {
            double newSignal = minuend.Signal - subtrahend.Signal;
            double newSem = SqSum(minuend.Sem, subtrahend.Sem);
            double newStdDev = SqSum(minuend.StdDev, subtrahend.StdDev);
            int newDof = WelchSatterthwaiteDoF(minuend.Sem, subtrahend.Sem, minuend.Dof, subtrahend.Dof);
            return new DataPoint(minuend.Wavelength, newSignal, newSem, newStdDev, newDof);
        }

        private static DataPoint Add(IDataPoint first, IDataPoint second)
        {
            double newSignal = first.Signal + second.Signal;
            double newSem = SqSum(first.Sem, second.Sem);
            double newStdDev = SqSum(first.StdDev, second.StdDev);
            int newDof = WelchSatterthwaiteDoF(first.Sem, second.Sem, first.Dof, second.Dof);
            return new DataPoint(first.Wavelength, newSignal, newSem, newStdDev, newDof);
        }

        private static double SqSum(double u1, double u2) => Math.Sqrt(u1 * u1 + u2 * u2);

        // Function to compute Welch–Satterthwaite degrees of freedom
        static int WelchSatterthwaiteDoF(double s1, double s2, int dof1, int dof2)
        {
            // Convert standard deviations to variances 
            double var1 = s1 * s1;
            double var2 = s2 * s2;

            // Numerator: (var1 + var2)^2
            double numerator = Math.Pow(var1 + var2, 2);

            // Denominator: (var1^2 / (n1 - 1)) + (var2^2 / (n2 - 1))
            double denominator = (Math.Pow(var1, 2) / (dof1)) + (Math.Pow(var2, 2) / (dof2));

            // Degrees of freedom
            return (int)(numerator / denominator);
        }



        private static double RelUncXXX(double x, double xr, double xb, double ux, double uxr, double uxb)
        {
            double v1 = 1.0 / (xr - xb);
            double v2 = v1 * v1;
            double u1 = v1 * ux;
            double u2 = v2 * (x - xr) * uxb;
            double u3 = v2 * (xb - x) * uxr;
            return Math.Sqrt((u1 * u1) + (u2 * u2) + (u3 * u3) );
        }
    }
}
