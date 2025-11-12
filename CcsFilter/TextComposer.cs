using System.Text;
using At.Matus.OpticalSpectrumLib;

namespace CcsFilter
{
    internal static class TextComposer
    {
        internal static string ToCsvLines(this IOpticalSpectrum spectrum)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spectrum.NumberOfPoints; i++)
            {
                sb.AppendLine(spectrum.DataPoints[i].ToCsvLine());
            }
            return sb.ToString();
        }

        internal static string ToThorlabsCsvLines(this IOpticalSpectrum spectrum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Header]");
            sb.AppendLine($"{spectrum.Name}");
            sb.AppendLine("[Data]");
            for (int i = 0; i < spectrum.NumberOfPoints; i++)
            {
                var dp = spectrum.DataPoints[i];
                sb.AppendLine($"{dp.Wavelength:e9},{dp.Signal:e9}");
            }
            sb.AppendLine("[EndOfFile]");
            return sb.ToString();
        }

    }
}
