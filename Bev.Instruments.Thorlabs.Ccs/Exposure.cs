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
    public static class Exposure
    {
        public static double GetOptimalExposureTime(this ThorlabsCcs spectro) => spectro.GetOptimalExposureTime(spectro.SaturationLevel, false);

        public static double GetOptimalExposureTime(this ThorlabsCcs spectro, double targetSignal, bool debug)
        {
            double maxIntegrationTime = 58.0; // seconds
            double minIntegrationTime = 0.00001; // seconds
            double optimalIntegrationTime = 0;
            double integrationTime = minIntegrationTime;
            spectro.SetIntegrationTime(integrationTime);
            double minExposureSignal = GetMaximumSignal(spectro.GetIntensityData());

            while (integrationTime < maxIntegrationTime)
            {
                spectro.SetIntegrationTime(integrationTime);
                var maxSignal = GetMaximumSignal(spectro.GetIntensityData());

                if (debug)
                {
                    Console.WriteLine($">>> debug {spectro.GetIntegrationTime():F5} s -> {maxSignal:F3}");
                }
                if (maxSignal >= 0.49 * targetSignal)
                {
                    // Estimate optimal integration time by linear extrapolation
                    optimalIntegrationTime = spectro.GetIntegrationTime() * (targetSignal / (maxSignal - minExposureSignal));
                    break;
                }
                integrationTime *= 2;
            }
            var finalIntegrationTime = RoundToSignificantDigits(optimalIntegrationTime, 2);
            if (finalIntegrationTime > maxIntegrationTime)
            {
                finalIntegrationTime = maxIntegrationTime;
            }
            spectro.SetIntegrationTime(finalIntegrationTime);
            if (debug)
            {
                double maxSignal = GetMaximumSignal(spectro.GetIntensityData());
                Console.WriteLine($">>> debug final {spectro.GetIntegrationTime():F5} s -> {maxSignal:F3}");
            }
            return finalIntegrationTime;
        }

        private static double GetMaximumSignal(double[] signal)
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

        private static double RoundToSignificantDigits(double number, int digits)
        {
            int sign = Math.Sign(number);
            if (sign < 0) number *= -1;
            if (number == 0) return 0;
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(number))) + 1);
            return sign * scale * Math.Round(number / scale, digits);
        }
    }
}

