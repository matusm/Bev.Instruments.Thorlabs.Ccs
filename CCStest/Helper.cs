using System;
using Bev.Instruments.Thorlabs.Ccs;

namespace CCStest
{
    internal static class Helper
    {
        internal static MeasuredSpectrum GetSpectrumUI(string message, TlCcs tlCcs, int numberSamples)
        {
            MeasuredSpectrum spectrum = new MeasuredSpectrum(tlCcs.Wavelengths);
            Console.WriteLine($"Press <ENTER> to start measurement of {message}.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                for (int i = 0; i < numberSamples; i++)
                {
                    spectrum.UpdateSignal(tlCcs.GetSingleScanData());
                    Console.Write(".");
                }
                Console.WriteLine();
            }
            return spectrum;
        }

        internal static double GetOptimalIntegrationTime(TlCcs tlCcs) => GetOptimalIntegrationTime(tlCcs, 1.0, false);

        internal static double GetOptimalIntegrationTime(TlCcs tlCcs, double targetSignal, bool debug)
        {
            double optimalIntegrationTime = 0;
            MeasuredSpectrum spectrum = new MeasuredSpectrum(tlCcs.Wavelengths);
            double integrationTime = 0.00001; // seconds
            while (integrationTime < 58)
            {
                spectrum.Clear();
                tlCcs.SetIntegrationTime(integrationTime);
                spectrum.UpdateSignal(tlCcs.GetSingleScanData());
                if (debug)
                {
                    Console.WriteLine($"Trying {tlCcs.GetIntegrationTime():F5} s -> {spectrum.MaximumSignal:F4}");
                }   
                if (spectrum.MaximumSignal >= 0.49)
                {
                    optimalIntegrationTime = tlCcs.GetIntegrationTime() * (targetSignal / spectrum.MaximumSignal);
                    break;
                }
                integrationTime *= 2;
            }
            return RoundToSignificantDigits(optimalIntegrationTime, 2);
        }

        private static double RoundToSignificantDigits(double number, int digits)
        {
            int sign = Math.Sign(number);
            if (sign < 0) number *= -1;
            if (number == 0) return 0;
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(number))) + 1);
            return sign * scale * Math.Round(number / scale, digits);
        }

    }
}
