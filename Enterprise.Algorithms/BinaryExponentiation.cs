using System;

namespace Enterprise.Algorithms
{

    public static class BinaryExponentiation
    {
        public static long RecBinaryBower(long baseNumber, long bowerNumber)
        {
            if (bowerNumber == 0) return 1;

            long res = RecBinaryBower(baseNumber, bowerNumber / 2);

            if (bowerNumber % 2 != 0)
                return res * res * baseNumber;
            else
                return res * res;
        }

        public static long BinaryBower(long a, long b)
        {
            long res = 1;

            while (b > 0)
            {
                if ((b & 1) == 1)
                    res = res * a;

                a = a * a;

                b >>= 1;
            }
            return res;
        }
    }
}
