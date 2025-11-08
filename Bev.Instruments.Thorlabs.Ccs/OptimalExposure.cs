using System;

namespace Bev.Instruments.Thorlabs.Ccs
{
    public partial class TlCcs
    {
        public double GetOptimalIntegrationTime() => GetOptimalIntegrationTime(1.0, false);

        public double GetOptimalIntegrationTime(double targetSignal, bool debug)
        {
            double optimalIntegrationTime = 0;
            double integrationTime = 0.00001; // seconds
            while (integrationTime < 58)
            {
                SetIntegrationTime(integrationTime);
                var signal = GetSingleScanData();
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
            return RoundToSignificantDigits(optimalIntegrationTime, 2);
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
