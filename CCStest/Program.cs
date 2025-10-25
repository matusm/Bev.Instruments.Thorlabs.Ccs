using System;
using Bev.Instruments.Thorlabs.Ccs;

namespace CCStest
{
    internal class Program
    {
        static TlCcs tlCcs;

        static void Main(string[] args)
        {
            tlCcs = new TlCcs(ProductID.CCS100, "M00928408");

            Console.WriteLine($"Resource Name:            {tlCcs.ResourceName}");
            Console.WriteLine($"Instrument Mamufacturer:  {tlCcs.InstrumentManufacturer}");
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

            int nSamples = 11;

            MeasuredSpectrum reference = Helper.GetSpectrumUI("reference spectrum", tlCcs, nSamples);
            //MeasuredSpectrum filter = Helper.GetSpectrumUI("sample spectrum", tlCcs, nSamples);
            //MeasuredSpectrum dark = Helper.GetSpectrumUI("dark spectrum", tlCcs, nSamples);

            //var transmission = SpecMath.RelXXX(filter, reference, dark);
            //var signal = SpecMath.Subtract(reference, dark);

            Console.WriteLine(reference.ToThorlabsCsvLines());


        }

    }
}
