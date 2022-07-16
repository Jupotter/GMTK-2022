using System.Collections.Generic;
using System.Linq;

namespace Assets.DiceCalculation
{
    public sealed class Single : IDistribution
    {
        private readonly int value;

        public static Single Distribution(int value)
        {
            return new Single(value);
        }

        private Single(int value)
        {
            this.value = value;
        }

        public int Sample()
        {
            return value;
        }

        public IEnumerable<int> Support()
        {
            yield return value;
        }

        public int Weight(int t)
        {
            return t == value ? 1 : 0;
        }
    }
}
