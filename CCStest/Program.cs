using System;
using Bev.Instruments.Thorlabs.Ccs;

namespace CCStest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TlCcs tlCcs = new TlCcs(ProductID.CCS100, "M00928408");
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

            int nSamples = 20;

            SpectralData dark = new SpectralData(tlCcs.Wavelengths);
            SpectralData filter = new SpectralData(tlCcs.Wavelengths);
            SpectralData reference = new SpectralData(tlCcs.Wavelengths);

            tlCcs.SetIntegrationTime(0.37);


            //Measure the reference spectrum
            Console.WriteLine(  );
            Console.WriteLine("Press <ENTER> to start measurement of reference spectrum.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                for (int i = 0; i < nSamples; i++)
                {
                    reference.UpdateSignal(tlCcs.GetSingleScanData());
                    Console.Write(".");
                }
            }
            Console.WriteLine(  );
            Console.WriteLine("Press <ENTER> to start measurement of sample spectrum.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            { 
                for (int i = 0; i < nSamples; i++)
                {
                    filter.UpdateSignal(tlCcs.GetSingleScanData());
                    Console.Write(".");
                }
        }
            Console.WriteLine(  );
            Console.WriteLine("Press <ENTER> to start measurement of dark spectrum.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                for (int i = 0; i < nSamples; i++)
                {
                    dark.UpdateSignal(tlCcs.GetSingleScanData());
                    Console.Write(".");
                }
            }

            var refCalc = new CalcData(reference);
            var darkCalc = new CalcData(dark);  
            var filterCalc = new CalcData(filter);  

            var transmission = SpecMath.RelXXX(filterCalc,refCalc,darkCalc);

            Console.WriteLine("Wavelength (nm)  Transmission  Noise");  
            for (int i = 0; i < transmission.Wavelengths.Length; i++)
            {
                Console.WriteLine($"{transmission.Wavelengths[i],12:F2} {transmission.AverageValues[i],8:F5} {transmission.NoiseValues[i],8:F5}");
            }

        //double integrationTime = 0.00001; // seconds
        //while (integrationTime < 100)
        //{
        //    darkSpec.Clear();
        //    tlCcs.SetIntegrationTime(integrationTime);
        //    darkSpec.UpdateSignal(tlCcs.GetSingleScanData());
        //    Console.WriteLine($"{tlCcs.GetIntegrationTime():F5} {darkSpec.MaximumSignal:F5}");
        //    if (darkSpec.IsOverexposed) break;
        //    integrationTime *= 1.1;
        //}

        }
    }
}
