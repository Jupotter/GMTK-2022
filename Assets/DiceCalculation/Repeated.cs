using System.Collections.Generic;

namespace Assets.DiceCalculation
{
    public sealed class Repeated : IDistribution
    {
        private readonly IDistribution source;
        private readonly int           count;

        private readonly Dictionary<int, long> _weights;

        public static IDistribution Distribution(IDistribution source, int count)
        {
            if (count == 1)
                return source;
            return new Repeated(source, count);
        }

        private Repeated(IDistribution source, int count)
        {
            this.source = source;
            this.count  = count;
            var weights    = new Dictionary<int, long> { { 0, 0 } };
            var newWeights = new Dictionary<int, long>();
            for (int c = 0; c < count; c++)
            {
                foreach (var i in source.Support())
                {
                    foreach (var j in weights.Keys)
                    {
                        var sum = i + j;
                        if (!newWeights.ContainsKey(sum))
                            newWeights[sum] = 0;

                        newWeights[sum] += source.Weight(i) + weights[j];
                    }
                }
                (weights, newWeights) = (newWeights, weights);
                newWeights.Clear();
            }

            _weights = weights;
        }

        public int Sample()
        {
            int sample = 0;
            for (int c = 0; c < count; c++)
            {
                sample += source.Sample();
            }

            return sample;
        }

        public IEnumerable<int> Support()
        {
            return _weights.Keys;
        }

        public long Weight(int t)
        {
            return _weights.ContainsKey(t) ? _weights[t] : 0;
        }
    }
}
