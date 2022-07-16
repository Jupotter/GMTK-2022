using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DiceCalculation
{
    public sealed class Uniform : IDistribution
    {
        public static Uniform Distribution(
            int min,
            int max)
        {
            if (min > max)
                throw new ArgumentException();
            return new Uniform(min, max);
        }

        public int Min { get; }
        public int Max { get; }

        private Uniform(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public IEnumerable<int> Support() => Enumerable.Range(Min, 1 + Max - Min);

        public int Sample() => (int)(SimpleRandom.NextDouble() * (1 + Max - Min) + Min);

        public int Weight(int i) => (Min <= i && i <= Max) ? 1 : 0;

        public int TotalWeight => 1 + Max - Min;
    }
}