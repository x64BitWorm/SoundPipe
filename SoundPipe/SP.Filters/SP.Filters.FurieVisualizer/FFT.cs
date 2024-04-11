namespace SP.Filters.FurieVisualizer
{
    public class FFT
    {
        public static double[] Transform(double[] pcm)
        {
            var AVal = new double[NearestUpperPower(pcm.Length)];
            var FTvl = new double[AVal.Length];
            int i, j, n, m, Mmax, Istp;
            for (i = 0; i < pcm.Length; i++)
            {
                AVal[i] = pcm[i];
            }
            double Tmpr, Tmpi, Wtmp, Theta;
            double Wpr, Wpi, Wr, Wi;
            n = AVal.Length * 2;
            var Tmvl = new double[n];
            for (i = 0; i < n; i += 2)
            {
                Tmvl[i] = 0;
                Tmvl[i + 1] = AVal[i / 2];
            }
            i = 1; j = 1;
            while (i < n)
            {
                if (j > i)
                {
                    Tmpr = Tmvl[i]; Tmvl[i] = Tmvl[j]; Tmvl[j] = Tmpr;
                    Tmpr = Tmvl[i + 1]; Tmvl[i + 1] = Tmvl[j + 1]; Tmvl[j + 1] = Tmpr;
                }
                i = i + 2; m = AVal.Length;
                while ((m >= 2) && (j > m))
                {
                    j = j - m; m = m >> 1;
                }
                j = j + m;
            }
            Mmax = 2;
            var TwoPi = Math.PI * 2;
            while (n > Mmax)
            {
                Theta = -TwoPi / Mmax; Wpi = Math.Sin(Theta);
                Wtmp = Math.Sin(Theta / 2); Wpr = Wtmp * Wtmp * 2;
                Istp = Mmax * 2; Wr = 1; Wi = 0; m = 1;
                while (m < Mmax)
                {
                    i = m; m = m + 2; Tmpr = Wr; Tmpi = Wi;
                    Wr = Wr - Tmpr * Wpr - Tmpi * Wpi;
                    Wi = Wi + Tmpr * Wpi - Tmpi * Wpr;
                    while (i < n)
                    {
                        j = i + Mmax;
                        Tmpr = Wr * Tmvl[j] - Wi * Tmvl[j - 1];
                        Tmpi = Wi * Tmvl[j] + Wr * Tmvl[j - 1];
                        Tmvl[j] = Tmvl[i] - Tmpr; Tmvl[j - 1] = Tmvl[i - 1] - Tmpi;
                        Tmvl[i] = Tmvl[i] + Tmpr; Tmvl[i - 1] = Tmvl[i - 1] + Tmpi;
                        i = i + Istp;
                    }
                }
                Mmax = Istp;
            }
            for (i = 0; i < FTvl.Length; i++)
            {
                j = i * 2; FTvl[i] = 2 * Math.Sqrt(Math.Pow(Tmvl[j], 2) + Math.Pow(Tmvl[j + 1], 2)) / AVal.Length;
            }
            Array.Resize(ref FTvl, FTvl.Length / 2);
            return FTvl;
        }

        private static uint NearestUpperPower(int num)
        {
            uint bit = 1;
            while (bit < num)
            {
                bit <<= 1;
            }
            return bit;
        }
    }
}
