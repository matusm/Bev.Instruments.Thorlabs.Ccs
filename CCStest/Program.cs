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

            //Console.WriteLine("Estimating optimal integration time...");
            //double optimalIntegrationTime = Helper.GetOptimalIntegrationTime(tlCcs, 1, true);
            //Console.WriteLine($"Optimal Integration Time: {optimalIntegrationTime} s");
            //Console.WriteLine();
            //tlCcs.SetIntegrationTime(optimalIntegrationTime);
            tlCcs.SetIntegrationTime(0.01);

            int nSamples = 50;

            MeasuredSpectrum reference = Helper.GetSpectrumUI("reference spectrum", tlCcs, nSamples);
            //MeasuredSpectrum filter = Helper.GetSpectrumUI("sample spectrum", tlCcs, nSamples);
            MeasuredSpectrum dark = Helper.GetSpectrumUI("dark spectrum", tlCcs, nSamples*2);

            //var transmission = SpecMath.RelXXX(filter, reference, dark);
            var signal = SpecMath.Subtract(reference, dark);


            int n1 = 0;
            int n2 = 0;

            foreach (var dp in signal.DataPoints)
            {
                n1++;
                double En = Math.Abs(dp.Signal / (2*dp.Noise));
                if (En >= 1.0)
                    n2++;
                Console.WriteLine($"{dp.Wavelength:F2} nm: {En:F2}");

            }
            Console.WriteLine($"{100.0 * (double)n2 / (double)n1:f1} %");



            //Console.WriteLine(signal.ToCsvLines());
            Console.WriteLine(signal.Name);


        }

    }
}
