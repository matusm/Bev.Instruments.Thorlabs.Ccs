using System;
using At.Matus.SpectrumPod;
using Bev.Instruments.Thorlabs.Ccs;

namespace CCStest
{
    internal static class Helper
    {
        // Updates the provided MeasuredSpectrum by taking numberSamples raw scans from the tlCcs device.
        internal static void UpdateRawSpectrumUI(MeasuredSpectrum spectrum, int numberSamples, string message, ThorlabsCcs tlCcs)
        {
            double[] rawData = new double[spectrum.NumberOfPoints];
            ConsoleProgressBar consoleProgressBar = new ConsoleProgressBar();
            Console.WriteLine($"Press any key to start raw measurement of {message} - 's' to skip");
            if (Console.ReadKey(true).Key != ConsoleKey.S)
            {
                for (int i = 0; i < numberSamples; i++)
                {
                    var intData = tlCcs.GetRawScanData();
                    for (int j = 0; j < rawData.Length; j++)
                    {
                        rawData[j] = intData[j];
                    }   
                    spectrum.UpdateSignal(rawData);
                    consoleProgressBar.Report(i + 1, numberSamples);
                }
                spectrum.Name = $"{message}";
            }
            else
            {
                Console.WriteLine($"Skipping measurement of {message}");
            }
        }

        // Creates and returns a MeasuredSpectrum by taking numberSamples raw scans from the tlCcs device.
        internal static MeasuredSpectrum GetRawSpectrumUI(int numberSamples, string message, ThorlabsCcs tlCcs)
        {
            MeasuredSpectrum spectrum = new MeasuredSpectrum(tlCcs.Wavelengths);
            UpdateRawSpectrumUI(spectrum, numberSamples, message, tlCcs);
            return spectrum;
        }

        // Updates the provided MeasuredSpectrum by taking numberSamples scans from the tlCcs device.
        internal static void UpdateSpectrumUI(MeasuredSpectrum spectrum, int numberSamples, string message, ThorlabsCcs tlCcs)
        {
            ConsoleProgressBar consoleProgressBar = new ConsoleProgressBar();
            Console.WriteLine($"Press any key to start measurement of {message} - 's' to skip");
            if (Console.ReadKey(true).Key != ConsoleKey.S)
            {
                for (int i = 0; i < numberSamples; i++)
                {
                    spectrum.UpdateSignal(tlCcs.GetScanData());
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
        internal static MeasuredSpectrum GetSpectrumUI(int numberSamples, string message, ThorlabsCcs tlCcs)
        {
            MeasuredSpectrum spectrum = new MeasuredSpectrum(tlCcs.Wavelengths);
            UpdateSpectrumUI(spectrum, numberSamples, message, tlCcs);
            return spectrum;
        }

        internal static double GetOptimalIntegrationTime(ThorlabsCcs tlCcs) => GetOptimalIntegrationTime(tlCcs, 1.0, false);

        internal static double GetOptimalIntegrationTime(ThorlabsCcs tlCcs, double targetSignal, bool debug)
        {
            double optimalIntegrationTime = 0;
            MeasuredSpectrum spectrum = new MeasuredSpectrum(tlCcs.Wavelengths);
            double integrationTime = 0.00001; // seconds
            while (integrationTime < 58)
            {
                spectrum.Clear();
                tlCcs.SetIntegrationTime(integrationTime);
                spectrum.UpdateSignal(tlCcs.GetScanData());
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
