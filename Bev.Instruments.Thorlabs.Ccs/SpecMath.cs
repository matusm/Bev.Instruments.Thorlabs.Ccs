using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public static class SpecMath
    {
        public static Spectrum Subtract(ISpectrum signal, ISpectrum bckgnd)
        {
            double[] wl = new double[signal.Wavelengths.Length];
            double[] newSignal = new double[signal.Wavelengths.Length];
            double[] newNoise = new double[signal.Wavelengths.Length];
            double[] newStdDev = new double[signal.Wavelengths.Length];
            for (int i = 0; i < signal.Wavelengths.Length; i++)
            {
                wl[i] = signal.Wavelengths[i];
                newSignal[i] = signal.AverageValues[i] - bckgnd.AverageValues[i];
                newNoise[i] = SqSum(signal.NoiseValues[i], bckgnd.NoiseValues[i]);
                newStdDev[i] = SqSum(signal.StdDevValues[i], bckgnd.StdDevValues[i]);
            }
            return new Spectrum(wl,newSignal, newNoise, newStdDev);
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
                newNoise[i] = RelUncXXX(signal.AverageValues[i], reference.AverageValues[i], bckgnd.AverageValues[i], signal.NoiseValues[i], reference.NoiseValues[i], bckgnd.NoiseValues[i]);
                newStdDev[i] = RelUncXXX(signal.AverageValues[i], reference.AverageValues[i], bckgnd.AverageValues[i], signal.StdDevValues[i], reference.StdDevValues[i], bckgnd.StdDevValues[i]); ;
            }
            return new Spectrum(wl, newSignal, newNoise, newStdDev);
        }

        private static double SqSum(double u1, double u2) => Math.Sqrt(u1 * u1 + u2 * u2);
        
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
