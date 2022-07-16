using System.Collections.Generic;
using System.Linq;

namespace Assets.DiceCalculation
{
    public sealed class AddedInt : IDistribution
    {
        private readonly IDistribution left;
        private readonly int           value;

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
