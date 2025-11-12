using System;
using At.Matus.OpticalSpectrumLib;
using Bev.Instruments.Thorlabs.Ccs;

namespace CcsFilter
{
    internal static class Helper
    {
        // Updates the provided MeasuredSpectrum by taking numberSamples raw scans from the tlCcs device.
        internal static void UpdateRawSpectrumUI(MeasuredOpticalSpectrum spectrum, int numberSamples, string message, ThorlabsCcs tlCcs)
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

        // Updates the provided MeasuredSpectrum by taking numberSamples scans from the tlCcs device.
        internal static void UpdateSpectrumUI(MeasuredOpticalSpectrum spectrum, int numberSamples, string message, ThorlabsCcs tlCcs)
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
        internal static MeasuredOpticalSpectrum GetSpectrumUI(int numberSamples, string message, ThorlabsCcs tlCcs)
        {
            MeasuredOpticalSpectrum spectrum = new MeasuredOpticalSpectrum(tlCcs.Wavelengths);
            UpdateSpectrumUI(spectrum, numberSamples, message, tlCcs);
            return spectrum;
        }

    }
}
