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
            double optimalIntegrationTime = GetOptimalIntegrationTime(tlCcs, 1.0);
            Console.WriteLine($"Optimal Integration Time: {optimalIntegrationTime} s");
            Console.WriteLine();
            tlCcs.SetIntegrationTime(optimalIntegrationTime);

            int nSamples = 100;
            SpectralData dark = new SpectralData(tlCcs.Wavelengths);
            SpectralData filter = new SpectralData(tlCcs.Wavelengths);
            SpectralData reference = new SpectralData(tlCcs.Wavelengths);

            //Measure the reference spectrum
            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to start measurement of reference spectrum.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                for (int i = 0; i < nSamples; i++)
                {
                    reference.UpdateSignal(tlCcs.GetSingleScanData());
                    Console.Write(".");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to start measurement of sample spectrum.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                for (int i = 0; i < nSamples; i++)
                {
                    filter.UpdateSignal(tlCcs.GetSingleScanData());
                    Console.Write(".");
                }
            }
            Console.WriteLine();
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

            var transmission = SpecMath.RelXXX(filterCalc, refCalc, darkCalc);
            var signal = SpecMath.Subtract(refCalc, darkCalc);

            Console.WriteLine("Wavelength (nm)  Transmission  Noise");
            for (int i = 0; i < signal.Wavelengths.Length; i++)
            {
                Console.WriteLine(signal.DataPoints[i].ToCsvLine());
            }

        }

        public static double GetOptimalIntegrationTime(TlCcs tlCcs, double targetSignal)
        {
            double optimalIntegrationTime = 0;
            SpectralData spectrum = new SpectralData(tlCcs.Wavelengths);
            double integrationTime = 0.00001; // seconds
            while (integrationTime < 58)
            {
                spectrum.Clear();
                tlCcs.SetIntegrationTime(integrationTime);
                spectrum.UpdateSignal(tlCcs.GetSingleScanData());
                //Console.WriteLine($"{tlCcs.GetIntegrationTime():F5} {spectrum.MaximumSignal:F5}");
                //Console.Write(".");
                if (spectrum.MaximumSignal >= 0.49)
                {
                    optimalIntegrationTime = tlCcs.GetIntegrationTime() * (targetSignal / spectrum.MaximumSignal);
                    break;
                }
                integrationTime *= 2;
            }
            return RoundToSignificantDigits(optimalIntegrationTime, 2);
        }

        public static double RoundToSignificantDigits(double number, int digits)
        {
            int sign = Math.Sign(number);
            if (sign < 0) number *= -1;
            if (number == 0) return 0;
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(number))) + 1);
            return sign * scale * Math.Round(number / scale, digits);
        }
    }
}
