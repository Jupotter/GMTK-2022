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
}
