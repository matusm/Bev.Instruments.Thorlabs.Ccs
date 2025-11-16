/*
 * Purpose:
 *   Helper method to determine an optimal integration time for the Thorlabs spectrometer
 *   based on measured signal levels. Keep this file focused on the exposure/optimization
 *   concern; consider moving acquisition/driver logic into a dedicated service for SRP.
 *
 * Notes:
 *  - Targets C# 7.3 and .NET Framework 4.7.2.
 *  - Date: 2025-11-09
 *  - Author: GitHub Copilot
 */

using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public partial class ThorlabsCcs
    {
        public double GetOptimalIntegrationTime() => GetOptimalIntegrationTime(1.0, false);

        public double GetOptimalIntegrationTime(double targetSignal, bool debug)
        {
            double optimalIntegrationTime = 0;
            double integrationTime = 0.00001; // seconds
            while (integrationTime < 58)
            {
                SetIntegrationTime(integrationTime);
                var signal = GetIntensityData();
                var maxSignal = GetMaximumSignal(signal);

                if (debug)
                {
                    Console.WriteLine($"Trying {GetIntegrationTime():F5} s -> {maxSignal:F4}");
                }
                if (maxSignal >= 0.49*targetSignal)
                {
                    optimalIntegrationTime = GetIntegrationTime() * (targetSignal / maxSignal);
                    break;
                }
                integrationTime *= 2;
            }
            var finalIntegrationTime = RoundToSignificantDigits(optimalIntegrationTime, 2);
            SetIntegrationTime(finalIntegrationTime);
            return finalIntegrationTime;
        }

        private double GetMaximumSignal(double[] signal)
        {
            double maxSignal = double.MinValue;
            foreach (var value in signal)
            {
                if (value > maxSignal)
                {
                    maxSignal = value;
                }
            }
            return maxSignal;
        }

        private double RoundToSignificantDigits(double number, int digits)
        {
            int sign = Math.Sign(number);
            if (sign < 0) number *= -1;
            if (number == 0) return 0;
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(number))) + 1);
            return sign * scale * Math.Round(number / scale, digits);
        }
    }
}
