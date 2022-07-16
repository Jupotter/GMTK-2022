using System.Collections.Generic;
using System.Linq;

namespace Assets.DiceCalculation
{
    public sealed class Exploded : IDistribution
    {
        private readonly IDistribution source;

        private readonly Dictionary<int, int> weights;
        private readonly int                  max;

        public static IDistribution Distribution(IDistribution source)
        {
            return new Exploded(source);
        }


        private Exploded(IDistribution source)
        {
            this.source = source;
            this.max    = source.Support().Max();
            weights     = new Dictionary<int, int>();
            var totalWeights = source.Support().Select(source.Weight).Sum();

            for (int d = 0; d < 3; d++)
            {
                var keys = weights.Keys.ToList();
                foreach (var weightsKey in keys)
                {
                    weights[weightsKey] *= totalWeights;
                }

                var add = d * max;
                foreach (var i in source.Support())
                {
                    var sum = add + i;

                    if (i == max)
                    {
                        continue;
                    }

                    if (!weights.ContainsKey(sum))
                        weights[sum] = 0;

                    weights[sum] += source.Weight(i);
                }
            }
        }

        public int Sample()
        {
            var result = source.Sample();
            while (result == max)
            {
                result += source.Sample();
            }

            return result;
        }

        public IEnumerable<int> Support()
        {
            return weights.Keys.OrderBy(x => x);
        }

        public int Weight(int t)
        {
            return weights.ContainsKey(t) ? weights[t] : 0;
        }
    }
}
