using Bev.Instruments.Thorlabs.Ccs;
using System;
using System.Globalization;
using System.IO;

namespace CCStest
{
    internal class Program
    {
        static TlCcs tlCcs;

        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            tlCcs = new TlCcs(ProductID.CCS100, "M00928408");

            Console.WriteLine($"Resource Name:            {tlCcs.ResourceName}");
            Console.WriteLine($"Instrument Manufacturer:  {tlCcs.InstrumentManufacturer}");
            Console.WriteLine($"Instrument Type:          {tlCcs.InstrumentType}");
            Console.WriteLine($"Instrument Serial Number: {tlCcs.InstrumentSerialNumber}");
            Console.WriteLine($"Firmware Revision:        {tlCcs.InstrumentFirmwareVersion}");
            Console.WriteLine($"Driver Revision:          {tlCcs.InstrumentDriverRevision}");
            Console.WriteLine($"User Text:                {tlCcs.InstrumentUserText}");
            Console.WriteLine($"Min Wavelength:           {tlCcs.MinimumWavelength:F2} nm");
            Console.WriteLine($"Max Wavelength:           {tlCcs.MaximumWavelength:F2} nm");
            Console.WriteLine();

            Console.WriteLine("Estimating optimal integration time...");
            double optimalIntegrationTime = Helper.GetOptimalIntegrationTime(tlCcs, 1, true);
            Console.WriteLine($"Optimal Integration Time: {optimalIntegrationTime} s");
            Console.WriteLine();
            tlCcs.SetIntegrationTime(optimalIntegrationTime);

            int nSamples = 40;

            MeasuredSpectrum reference = new MeasuredSpectrum(tlCcs.Wavelengths);
            MeasuredSpectrum filter = new MeasuredSpectrum(tlCcs.Wavelengths);
            MeasuredSpectrum dark = new MeasuredSpectrum(tlCcs.Wavelengths);

            Helper.UpdateSpectrumUI(reference, nSamples, "reference spectrum #1", tlCcs);
            Console.WriteLine(reference);
            Helper.UpdateSpectrumUI(filter, nSamples, "sample spectrum #1", tlCcs);
            Console.WriteLine(filter);
            Helper.UpdateSpectrumUI(dark, nSamples*2, "dark spectrum #1", tlCcs);
            Console.WriteLine(dark);
            Helper.UpdateSpectrumUI(filter, nSamples, "sample spectrum #2", tlCcs);
            Console.WriteLine(filter);
            Helper.UpdateSpectrumUI(reference, nSamples, "reference spectrum #2", tlCcs);
            Console.WriteLine(reference);

            //reference = Helper.GetSpectrumUI("reference spectrum", tlCcs, nSamples);
            //filter = Helper.GetSpectrumUI("sample spectrum", tlCcs, nSamples);
            //dark = Helper.GetSpectrumUI("dark spectrum", tlCcs, nSamples);

            Spectrum signal = SpecMath.ComputeBiasCorrectedRatio(filter, reference, dark);

            string csvString = signal.ToCsvLines();
            string fileName = $"spectrum_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            string outPath = Path.Combine(Environment.CurrentDirectory, fileName);
            File.WriteAllText(outPath, csvString);

            Console.WriteLine("done.");

        }

    }
}
