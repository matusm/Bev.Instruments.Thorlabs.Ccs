using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bev.Instruments.Thorlabs.Ccs
{
    internal enum MaskMode
    {
        LongPass,
        ShortPass
    }

    internal static class Masker
    {

        public static Spectrum ApplyMask(ISpectrum spectrum, double w0, double hw, MaskMode mode = MaskMode.LongPass)
        {
            double[] mask;
            switch (mode)
            {
                case MaskMode.ShortPass:
                    mask = SyntheticShortpassFilter(spectrum.Wavelengths, w0, hw);
                    break;
                case MaskMode.LongPass:
                    mask = SynthLongpassFilter(spectrum.Wavelengths, w0, hw);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown MaskMode");
            }
            return SpecMath.Weighting(spectrum, mask);
        }

        private static double[] SynthLongpassFilter(double[] wavelengths, double w0, double hw)
        {
            double[] mask = new double[wavelengths.Length];
            double slope = 0.5 / hw;
            for (int i = 0; i < wavelengths.Length; i++)
            {
                var w = wavelengths[i] - w0;
                var m0 = 0.5 + slope * w;
                if (m0 < 0) m0 = 0.0;
                if (m0 > 1) m0 = 1.0;
                mask[i] = m0;
            }
            return mask;
        }

        private static double[] SynthLongpassFilterQ(double[] wavelengths, double w0, double hw)
        {
            double[] mask = new double[wavelengths.Length];
            for (int i = 0; i < wavelengths.Length; i++)
            {
                double w = wavelengths[i] - w0;
                double m0 = 0.0;
                if (w < -hw) m0 = 0.0;
                if (w > hw) m0 = 1.0;
                if (w >= -hw && w < 0)
                {
                    m0 = w * w * 0.5 / (hw * hw) + w / hw + 0.5;
                }
                if (w >= 0 && w <= hw)
                {
                    m0 = -w * w * 0.5 / (hw * hw) + w / hw + 0.5;
                }
                if (m0 < 0) m0 = 0.0;
                if (m0 > 1) m0 = 1.0;
                mask[i] = m0;
            }
            return mask;
        }


        private static double[] SyntheticShortpassFilter(double[] wavelengths, double w0, double hw)
        {
            return InvertFilter(SynthLongpassFilter(wavelengths, w0, hw));
        }


        private static double[] InvertFilter(double[] f)
        {
            double[] mask = new double[f.Length];
            for (int i = 0; i < f.Length; i++)
            {
                mask[i] = 1.0 - f[i];
            }
            return mask;
        }

        private static double[] CombineFilter(double[] f1, double[] f2)
        {
            if (f1.Length != f2.Length)
                throw new ArgumentException("Filter lengths do not match");
            double[] mask = new double[f1.Length];
            for (int i = 0; i < f1.Length; i++)
            {
                mask[i] = f1[i] * f2[i];
            }
            return mask;
        }

    }
}
