using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public static class SpecMath
    {

        public static CalcData Subtract(CalcData signal, CalcData bckgnd)
        {
            double[] wl = new double[signal.Wavelengths.Length];
            double[] newSignal = new double[signal.Wavelengths.Length];
            double[] newNoise = new double[signal.Wavelengths.Length];
            double[] newStdDev = new double[signal.Wavelengths.Length];
            for (int i = 0; i < signal.Wavelengths.Length; i++)
            {
                wl[i] = signal.Wavelengths[i];
                newSignal[i] = signal.AverageValues[i] - bckgnd.AverageValues[i];
                newNoise[i] = Math.Sqrt(Math.Pow(signal.NoiseValues[i], 2) + Math.Pow(bckgnd.NoiseValues[i], 2));
                newStdDev[i] = Math.Sqrt(Math.Pow(signal.StdDevValues[i], 2) + Math.Pow(bckgnd.StdDevValues[i], 2));
            }
            return new CalcData(wl,newSignal, newNoise, newStdDev);
        }

        public static CalcData RelXXX(CalcData signal, CalcData reference, CalcData bckgnd)
        {
            double[] wl = new double[signal.Wavelengths.Length];
            double[] newSignal = new double[signal.Wavelengths.Length];
            double[] newNoise = new double[signal.Wavelengths.Length];
            double[] newStdDev = new double[signal.Wavelengths.Length];
            for (int i = 0; i < signal.Wavelengths.Length; i++)
            {
                wl[i] = signal.Wavelengths[i];
                newSignal[i] = (signal.AverageValues[i] - bckgnd.AverageValues[i])/(reference.AverageValues[i] - bckgnd.AverageValues[i]);
                newNoise[i] = Math.Sqrt(Math.Pow(signal.NoiseValues[i], 2) + Math.Pow(bckgnd.NoiseValues[i], 2));
                newStdDev[i] = Math.Sqrt(Math.Pow(signal.StdDevValues[i], 2) + Math.Pow(bckgnd.StdDevValues[i], 2));
            }
            return new CalcData(wl, newSignal, newNoise, newStdDev);
        }

    }
}
