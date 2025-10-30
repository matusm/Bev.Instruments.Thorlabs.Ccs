﻿using System.Text;
using Bev.Instruments.Thorlabs.Ccs;

namespace CCStest
{
    internal static class TextComposer
    {
        internal static string ToCsvLines(this ISpectrum spectrum)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spectrum.NumberOfPoints; i++)
            {
                sb.AppendLine(spectrum.DataPoints[i].ToCsvLine());
            }
            return sb.ToString();
        }

        internal static string ToThorlabsCsvLines(this ISpectrum spectrum)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Data]");
            for (int i = 0; i < spectrum.NumberOfPoints; i++)
            {
                var dp = spectrum.DataPoints[i];
                sb.AppendLine($"{dp.Wavelength:e9},{dp.Value:e9}");
            }
            sb.AppendLine("[EndOfFile]");
            return sb.ToString();
        }




    }
}
