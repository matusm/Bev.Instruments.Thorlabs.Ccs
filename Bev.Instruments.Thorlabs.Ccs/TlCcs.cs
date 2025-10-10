using System;
using System.Text;
using System.Threading;
using Thorlabs.ccs.interop64;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public class TlCcs
    {
        public string ResourceName { get; }
        public string InstrumentManufacturer { get; private set; } = "";
        public string InstrumentType { get; private set; } = "";
        public string InstrumentSerialNumber { get; private set; } = "";
        public string InstrumentFirmwareVersion { get; private set; } = "";
        public string InstrumentDriverRevision { get; private set; } = "";
        public string InstrumentUserText => GetUserText();

        public double[] Wavelengths => wavelengths;
        public double MinimumWavelength => wavelengths[0];
        public double MaximumWavelength => wavelengths[wavelengths.Length - 1];

        public TlCcs(string serialNumber) : this(ProductID.CCS100, serialNumber) { }

        public TlCcs(ProductID id, string serialNumber)
        {
            ResourceName = $"USB0::0x1313::0x{((int)id).ToString("X4")}::{serialNumber}::RAW";
            ccs = new TLCCS(ResourceName, true, true);
            QueryWavelengths();
            QueryIdentification();
        }

        public void SetIntegrationTime(double seconds)
        {
            if (seconds < 0.00001) seconds = 0.00001;
            if (seconds > 52) seconds = 52; // the manual says max 60s, but only up to 52s works
            ccs.setIntegrationTime(seconds);
        }

        public double GetIntegrationTime()
        {
            ccs.getIntegrationTime(out double value);
            return value;   
        }   

        public double[] GetSingleScanData()
        {
            //TODO try catch
            double[] intensity = new double[nPixels];
            ccs.startScan();
            int status = 0;
            while (status != 17)
            {
                ccs.getDeviceStatus(out status);
                Thread.Sleep(100);
            }
            int ret = ccs.getScanData(intensity); // when overexposed ret!=0
            if (ret != 0) throw new Exception($"Error reading scan data, return code: {ret}");
            return intensity;
        }

        public int[] GetSingleRawScanData()
        {
            // values go from ~6180 to 65535
            int[] intensity = new int[nPixels];
            ccs.startScan();
            int status = 0;
            while (status != 17)
            {
                ccs.getDeviceStatus(out status);
                Thread.Sleep(100);
            }
            int ret = ccs.getRawScanData(intensity); // when overexposed ret!=0
            if (ret != 0) throw new Exception($"Error reading scan data, return code: {ret}");
            return intensity;
        }

        private void QueryWavelengths()
        {
            short dataSet = 0;
            double minimumWavelength = 0;
            double maximumWavelength = 0;
            ccs.getWavelengthData(dataSet, wavelengths, out minimumWavelength, out maximumWavelength);
        }

        private void QueryIdentification()
        {
            StringBuilder sb1 = new StringBuilder(256);
            StringBuilder sb2 = new StringBuilder(256);
            StringBuilder sb3 = new StringBuilder(256);
            StringBuilder sb4 = new StringBuilder(256);
            StringBuilder sb5 = new StringBuilder(256);
            ccs.identificationQuery(sb1, sb2, sb3, sb4, sb5);
            InstrumentManufacturer = sb1.ToString().Trim();
            InstrumentType = sb2.ToString().Trim();
            InstrumentSerialNumber = sb3.ToString().Trim();
            InstrumentFirmwareVersion = sb4.ToString().Trim();
            InstrumentDriverRevision = sb5.ToString().Trim();
        }

        private double[] wavelengths = new double[nPixels]; 

        private string GetUserText()
        {
            StringBuilder uText = new StringBuilder();
            ccs.getUserText(uText);
            return uText.ToString();
        }

        private readonly TLCCS ccs;
        private const int nPixels = 3648;

    }
}
