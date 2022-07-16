using System.Collections.Generic;

namespace Assets.DiceCalculation
{
    public static class Distributions
    {
        public static IEnumerable<int> Samples(this IDistribution distribution)
        {
            while (true)
            {
                yield return distribution.Sample();
            }
        }

        public static IDistribution Add(this IDistribution left, IDistribution right)
        {
            return Added.Distribution(left, right);
        }

        public static IDistribution Substract(this IDistribution left, IDistribution right)
        {
            return Substracted.Distribution(left, right);
        }

        public static IDistribution Add(this IDistribution left, int right)
        {
            return AddedInt.Distribution(left, right);
        }

        public static IDistribution Multiply(this IDistribution left, int right)
        {
            return MultipliedInt.Distribution(left, right);
        }



        public static IDistribution Multiply(this IDistribution left, IDistribution right)
        {
            return Multiplied.Distribution(left, right);
        }

        public static IDistribution Repeat(this IDistribution source, int count)
        {
            return Repeated.Distribution(source, count);
        }

        public static IDistribution Explode(this IDistribution source)
        {
            return Exploded.Distribution(source);
        }
    }
}
