using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DiceCalculation
{
    public sealed class Added : IDistribution
    {
        private readonly IDistribution left;
        private readonly IDistribution right;

        private readonly Dictionary<int, int> weights;

        public static Added Distribution(IDistribution left, IDistribution right)
        {
            return new Added(left, right);
        }


        private Added(IDistribution left, IDistribution right)
        {
            this.left  = left;
            this.right = right;
            weights    = new Dictionary<int, int>();
            foreach (var i in left.Support())
            {
                foreach (var j in right.Support())
                {
                    var sum = i + j;
                    if (!weights.ContainsKey(sum))
                        weights[sum] = 0;

                    weights[sum] += left.Weight(i) + right.Weight(j);
                }
            }
        }

        public int Sample()
        {
            return left.Sample() + right.Sample();
        }

        public IEnumerable<int> Support()
        {
            return weights.Keys;
        }

        public int Weight(int t)
        {
            return weights.ContainsKey(t) ? weights[t] : 0;
        }
    }

    public sealed class AddedInt : IDistribution
    {
        private readonly IDistribution left;
        private readonly int value;

        public static AddedInt Distribution(IDistribution left, int value)
        {
            return new AddedInt(left, value);
        }


        private AddedInt(IDistribution left, int value)
        {
            this.left  = left;
            this.value = value;
        }

        public int Sample()
        {
            return left.Sample() + value;
        }

        public IEnumerable<int> Support()
        {
            return left.Support().Select(v => v + value);
        }

        public int Weight(int t)
        {
            return left.Weight(t - value);
        }
    }

    public sealed class Multiplied : IDistribution
    {
        private readonly IDistribution left;
        private readonly IDistribution right;

        private readonly Dictionary<int, int> weights;

        public static Multiplied Distribution(IDistribution left, IDistribution right)
        {
            return new Multiplied(left, right);
        }


        private Multiplied(IDistribution left, IDistribution right)
        {
            this.left  = left;
            this.right = right;
            weights    = new Dictionary<int, int>();
            foreach (var i in left.Support())
            {
                foreach (var j in right.Support())
                {
                    var product = i * j;
                    if (!weights.ContainsKey(product))
                        weights[product] = 0;

                    weights[product] += left.Weight(i) + right.Weight(j);
                }
            }
        }

        public int Sample()
        {
            return left.Sample() * right.Sample();
        }

        public IEnumerable<int> Support()
        {
            return weights.Keys;
        }

        public int Weight(int t)
        {
            return weights.ContainsKey(t) ? weights[t] : 0;
        }
    }

    public sealed class Repeated : IDistribution
    {
        private readonly IDistribution source;
        private readonly int           count;

        private readonly Dictionary<int, int> _weights;

        public static Repeated Distribution(IDistribution source, int count)
        {
            return new Repeated(source, count);
        }

        private Repeated(IDistribution source, int count)
        {
            this.source = source;
            this.count  = count;
            var weights    = new Dictionary<int, int> { { 0, 0 } };
            var newWeights = new Dictionary<int, int>();
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

        public int Weight(int t)
        {
            return _weights.ContainsKey(t) ? _weights[t] : 0;
        }
    }
}
