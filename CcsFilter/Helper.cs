using System;
using At.Matus.SpectrumPod;
using Bev.Instruments.Thorlabs.Ccs;

namespace CcsFilter
{
    internal static class Helper
    {
        // Updates the provided MeasuredSpectrum by taking numberSamples raw scans from the tlCcs device.
        internal static void UpdateRawSpectrumUI(MeasuredSpectrum spectrum, int numberSamples, string message, TlCcs tlCcs)
        {
            double[] rawData = new double[spectrum.NumberOfPoints];
            ConsoleProgressBar consoleProgressBar = new ConsoleProgressBar();
            Console.WriteLine($"Press any key to start raw measurement of {message} - 's' to skip");
            if (Console.ReadKey(true).Key != ConsoleKey.S)
            {
                for (int i = 0; i < numberSamples; i++)
                {
                    var intData = tlCcs.GetSingleRawScanData();
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

    }
}
