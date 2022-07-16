using System.Collections.Generic;

namespace Assets.DiceCalculation
{
    public sealed class Multiplied : IDistribution
    {
        private readonly IDistribution left;
        private readonly IDistribution right;

        private readonly Dictionary<int, int> weights;

        public static IDistribution Distribution(IDistribution left, IDistribution right)
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
}
