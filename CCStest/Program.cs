using At.Matus.SpectrumPod;
using Bev.Instruments.Thorlabs.Ccs;
using System;
using System.Globalization;

namespace CCStest
{
    internal class Program
    {
        static ThorlabsCcs tlCcs;

        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            tlCcs = new ThorlabsCcs(ProductID.CCS100, "M00928408");

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

            int nSamples = 100;



            MeasuredSpectrum spc;
            int[] intData;
            tlCcs.SetIntegrationTime(0.001);

            for (int i = 0; i < nSamples; i++)
            {
                intData = tlCcs.GetRawScanData();
                Console.WriteLine($"{i+1,4}  {MinMax(intData)}   t={tlCcs.GetIntegrationTime()} s");
            }






            //double[] dblData;
            //tlCcs.SetIntegrationTime(0.00001);
            //dblData = tlCcs.GetSingleScanData();
            //Console.WriteLine($"{MinMax(dblData)}   t={tlCcs.GetIntegrationTime()} s"); 
            //tlCcs.SetIntegrationTime(0.001);
            //dblData = tlCcs.GetSingleScanData();
            //Console.WriteLine($"{MinMax(dblData)}   t={tlCcs.GetIntegrationTime()} s");
            //tlCcs.SetIntegrationTime(0.01);
            //dblData = tlCcs.GetSingleScanData();
            //Console.WriteLine($"{MinMax(dblData)}   t={tlCcs.GetIntegrationTime()} s");
            //tlCcs.SetIntegrationTime(0.1);
            //dblData = tlCcs.GetSingleScanData();
            //Console.WriteLine($"{MinMax(dblData)}   t={tlCcs.GetIntegrationTime()} s");
            //tlCcs.SetIntegrationTime(1);
            //dblData = tlCcs.GetSingleScanData();
            //Console.WriteLine($"{MinMax(dblData)}   t={tlCcs.GetIntegrationTime()} s");
            //tlCcs.SetIntegrationTime(2);
            //dblData = tlCcs.GetSingleScanData();
            //Console.WriteLine($"{MinMax(dblData)}   t={tlCcs.GetIntegrationTime()} s");
            //tlCcs.SetIntegrationTime(10);
            //dblData = tlCcs.GetSingleScanData();
            //Console.WriteLine($"{MinMax(dblData)}   t={tlCcs.GetIntegrationTime()} s");

            //spc = Helper.GetRawSpectrumUI(nSamples, $"spectrum t={tlCcs.GetIntegrationTime()}s", tlCcs);
            //Console.WriteLine($"{tlCcs.GetIntegrationTime()} s   min: {spc.MinimumValue:F1}   max: {spc.MaximumValue:F1}");
            //tlCcs.SetIntegrationTime(0.01);
            //spc = Helper.GetRawSpectrumUI(nSamples, $"spectrum t={tlCcs.GetIntegrationTime()}s", tlCcs);
            //Console.WriteLine($"{tlCcs.GetIntegrationTime()} s   min: {spc.MinimumValue:F1}   max: {spc.MaximumValue:F1}");
            //tlCcs.SetIntegrationTime(0.1);
            //spc = Helper.GetRawSpectrumUI(nSamples, $"spectrum t={tlCcs.GetIntegrationTime()}s", tlCcs);
            //Console.WriteLine($"{tlCcs.GetIntegrationTime()} s   min: {spc.MinimumValue:F1}   max: {spc.MaximumValue:F1}");
            ////tlCcs.SetIntegrationTime(1);
            ////spc = Helper.GetRawSpectrumUI(nSamples, $"spectrum t={tlCcs.GetIntegrationTime()}s", tlCcs);
            ////Console.WriteLine($"{tlCcs.GetIntegrationTime()} s   min: {spc.MinimumValue:F1}   max: {spc.MaximumValue:F1}");



            //string csvString = spc.ToCsvLines();
            //string fileName = $"spectrum_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            //string outPath = Path.Combine(Environment.CurrentDirectory, fileName);
            //File.WriteAllText(outPath, csvString);
            Console.WriteLine("done.");

        }

        static string MinMax(int[] data)
        {

            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (var v in data)
            {
                if (v < min) min = v;
                if (v > max) max = v;
            }
            return $"min: {min}, max: {max}";
        }

        static string MinMax(double[] data)
        {

            double min = double.MaxValue;
            double  max = double.MinValue;
            foreach (var v in data)
            {
                if (v < min) min = v;
                if (v > max) max = v;
            }
            return $"min: {min:F5}, max: {max:F5}";
        }

    }
}
