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
            Console.WriteLine(  );

            SpectralData darkSpec = new SpectralData(tlCcs.Wavelengths);

            tlCcs.SetIntegrationTime(50);
            darkSpec.UpdateSignal(tlCcs.GetSingleScanData());
            Console.WriteLine($"{tlCcs.GetIntegrationTime():F5} {darkSpec.MaximumSignal:F5}");

            int[] adCounts = tlCcs.GetSingleRawScanData();

            for (int i = 0; i < adCounts.Length; i += 1)
            {
                Console.WriteLine($"{i,5}: {adCounts[i]} AD counts");
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
