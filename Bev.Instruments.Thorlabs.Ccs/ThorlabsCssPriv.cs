using System.Text;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public partial class ThorlabsCcs
    {

        private void QueryWavelengths()
        {
            short dataSet = 0;
            double minimumWavelength = 0;
            double maximumWavelength = 0;
            spectrometer.getWavelengthData(dataSet, wavelengthsCache, out minimumWavelength, out maximumWavelength);
        }

        private void QueryIdentification()
        {
            StringBuilder sb1 = new StringBuilder(256);
            StringBuilder sb2 = new StringBuilder(256);
            StringBuilder sb3 = new StringBuilder(256);
            StringBuilder sb4 = new StringBuilder(256);
            StringBuilder sb5 = new StringBuilder(256);
            spectrometer.identificationQuery(sb1, sb2, sb3, sb4, sb5);
            InstrumentManufacturer = sb1.ToString().Trim();
            InstrumentType = sb2.ToString().Trim();
            InstrumentSerialNumber = sb3.ToString().Trim();
            InstrumentFirmwareVersion = sb4.ToString().Trim();
            InstrumentDriverRevision = sb5.ToString().Trim();
        }

        private double[] wavelengthsCache = new double[N_PIXELS];

        private string GetUserText()
        {
            StringBuilder uText = new StringBuilder();
            spectrometer.getUserText(uText);
            return uText.ToString();
        }
    }
}
