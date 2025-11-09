using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public interface ITlCcsDriver : IDisposable
    {
        void SetIntegrationTime(double seconds);
        double GetIntegrationTime();
        void StartScan();
        int GetDeviceStatus();
        int GetScanData(double[] intensity);   // returns driver return code
        int GetRawScanData(int[] intensity);   // returns driver return code
        void GetWavelengthData(double[] wavelengths, out double minimumWavelength, out double maximumWavelength);
        InstrumentIdentification GetIdentification();
        string GetUserText();
    }

    public struct InstrumentIdentification
    {
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public string DriverRevision { get; set; }
    }
}