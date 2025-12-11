using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Purpose:
 *   Core interface for array spectrometers used by the Thorlabs CCS adapter.
 *   Keep this file focused on the interface contract only; implementation and
 *   acquisition logic belong in separate classes to follow SRP.
 *
 * Notes:
 *  - Targets C# 7.3 and .NET Framework 4.7.2.
 *  - Date: 2025-11-09
 *  - Author: GitHub Copilot
 */

namespace Bev.Instruments.Thorlabs.Ccs
{
    public interface IArraySpectrometer
    {
        string InstrumentManufacturer { get; }
        string InstrumentType { get; }
        string InstrumentSerialNumber { get; }
        string InstrumentFirmwareVersion { get; }
        string InstrumentDriverRevision { get; }
        
        double[] Wavelengths { get; }        
        double MinimumWavelength { get; }
        double MaximumWavelength { get; }
        
        double[] GetIntensityData();
        void SetIntegrationTime(double seconds);
        double GetIntegrationTime();
    }
}
