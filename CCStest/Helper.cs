using System;
using Bev.Instruments.Thorlabs.Ccs;

namespace CCStest
{
    internal static class Helper
    {
        // Updates the provided MeasuredSpectrum by taking numberSamples scans from the tlCcs device.
        internal static void UpdateSpectrumUI(MeasuredSpectrum spectrum, int numberSamples, string message, TlCcs tlCcs)
        {
            ConsoleProgressBar consoleProgressBar = new ConsoleProgressBar();
            Console.WriteLine($"Press any key to start measurement of {message} - 's' to skip");
            if (Console.ReadKey(true).Key != ConsoleKey.S)
            {
                for (int i = 0; i < numberSamples; i++)
                {
                    spectrum.UpdateSignal(tlCcs.GetSingleScanData());
                    consoleProgressBar.Report(i + 1, numberSamples);
                }
                spectrum.Name = $"{message}";
            }
            else
            {
                Console.WriteLine($"Skipping measurement of {message}");
            }
        }

        // Creates and returns a MeasuredSpectrum by taking numberSamples scans from the tlCcs device.
        internal static MeasuredSpectrum GetSpectrumUI(int numberSamples, string message, TlCcs tlCcs)
        {
            MeasuredSpectrum spectrum = new MeasuredSpectrum(tlCcs.Wavelengths);
            UpdateSpectrumUI(spectrum, numberSamples, message, tlCcs);
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
                    Console.WriteLine($"Trying {tlCcs.GetIntegrationTime():F5} s -> {spectrum.MaximumValue:F4}");
                }   
                if (spectrum.MaximumValue >= 0.49)
                {
                    optimalIntegrationTime = tlCcs.GetIntegrationTime() * (targetSignal / spectrum.MaximumValue);
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
