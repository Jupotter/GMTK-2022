using System.Collections.Generic;
using System.Linq;

namespace Assets.DiceCalculation
{
    public sealed class MultipliedInt : IDistribution
    {
        private readonly IDistribution source;
        private readonly int           value;

        public static IDistribution Distribution(IDistribution source, int value)
        {
            return new MultipliedInt(source, value);
        }


        private MultipliedInt(IDistribution source, int value)
        {
            this.source = source;
            this.value  = value;
        }

        public int Sample()
        {
            return source.Sample() * value;
        }

        public IEnumerable<int> Support()
        {
            return source.Support().Select(v => v * value);
        }

        public long Weight(int t)
        {
            var src = source.Support().Single(s => s * value == t);
            return source.Weight(src);
        }
    }
}
