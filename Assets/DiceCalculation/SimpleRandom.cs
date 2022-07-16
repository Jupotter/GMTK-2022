using System;
using CRNG = System.Security.Cryptography.RandomNumberGenerator;
using System.Threading;

namespace Assets.DiceCalculation
{
    internal static class SimpleRandom
    {
        private static readonly ThreadLocal<CRNG> crng = new(CRNG.Create);

        private static readonly ThreadLocal<Random> prng = new(() =>
        {
            var bytes = new byte[sizeof(int)];
            crng.Value.GetBytes(bytes);
            var seed = BitConverter.ToInt32(bytes) & int.MaxValue;
            return new Random(seed);
        });

        public static int    NextInt()    => prng.Value.Next();
        public static double NextDouble() => prng.Value.NextDouble();
    }
}
