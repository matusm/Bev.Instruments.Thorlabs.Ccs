/*
 * Purpose:
 *   Lightweight adapter for the Thorlabs CCS family of array spectrometers.
 *   This file contains the device interop and simple acquisition methods.
 *   Keep high-level algorithms and processing (e.g., optimal exposure) in
 *   separate partials or services to follow SRP.
 *
 * Notes:
 *  - Targets C# 7.3 and .NET Framework 4.7.2.
 *  - Date: 2025-11-09
 *  - Author: GitHub Copilot
 */

using System;
using System.Threading;
using Thorlabs.ccs.interop64;
using Bev.Instruments.ArraySpectrometer.Abstractions;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public enum ProductID
    {
        CCS100 = 0x8081,
        CCS125 = 0x8083,
        CCS150 = 0x8085,
        CCS175 = 0x8087,
        CCS200 = 0x8089
    }

    public partial class ThorlabsCcs : IArraySpectrometer
    {
        private readonly TLCCS spectrometer;
        private const int N_PIXELS = 3648;

        public string ResourceName { get; }
        public string InstrumentManufacturer { get; private set; } = "";
        public string InstrumentType { get; private set; } = "";
        public string InstrumentSerialNumber { get; private set; } = "";
        public string InstrumentFirmwareVersion { get; private set; } = "";
        public string InstrumentDriverRevision { get; private set; } = "";
        public string InstrumentUserText => GetUserText();

        public double[] Wavelengths => wavelengthsCache;
        public double MinimumWavelength => wavelengthsCache[0];
        public double MaximumWavelength => wavelengthsCache[wavelengthsCache.Length - 1];
        public double SaturationLevel => 1.0;

        public ThorlabsCcs(string serialNumber) : this(ProductID.CCS100, serialNumber) { }

        public ThorlabsCcs(ProductID id, string serialNumber)
        {
            ResourceName = $"USB0::0x1313::0x{(int)id:X4}::{serialNumber}::RAW";
            spectrometer = new TLCCS(ResourceName, true, true);
            QueryWavelengths();
            QueryIdentification();
        }

        public void SetIntegrationTime(double seconds)
        {
            if (seconds < 0.00001) seconds = 0.00001;
            if (seconds > 52) seconds = 52; // the manual says max 60s, but only up to 52s works
            spectrometer.setIntegrationTime(seconds);
        }

        public double GetIntegrationTime()
        {
            spectrometer.getIntegrationTime(out double value);
            return value;   
        }   

        public double[] GetIntensityData()
        {
            double[] intensity = new double[N_PIXELS];
            spectrometer.startScan();
            int status = 0;
            while (status != 17)
            {
                spectrometer.getDeviceStatus(out status);
                Thread.Sleep(100);
            }
            int ret = spectrometer.getScanData(intensity); // when overexposed ret!=0
            if (ret != 0) throw new Exception($"Error reading scan data, return code: {ret}");
            return intensity;
        }

        public int[] GetRawScanData()
        {
            // values go from ~6180 to 65535
            int[] intensity = new int[N_PIXELS];
            spectrometer.startScan();
            int status = 0;
            while (status != 17)
            {
                spectrometer.getDeviceStatus(out status);
                Thread.Sleep(100);
            }
            int ret = spectrometer.getRawScanData(intensity); // when overexposed ret!=0
            if (ret != 0) throw new Exception($"Error reading scan data, return code: {ret}");
            return intensity;
        }


    }
}
