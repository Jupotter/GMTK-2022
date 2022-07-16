using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Assets.DiceCalculation
{
    public sealed class Conditioned : IDistribution
    {
        private readonly IDistribution        source;
        private readonly Condition            condition;
        private readonly Dictionary<int, int> weights = new();

        public static IDistribution Distribution(IDistribution source, Condition condition)
        {
            return new Conditioned(source, condition);
        }

        public readonly struct Condition
        {
            public readonly int           Limit;
            public readonly IDistribution DistributionIf;
            public readonly IDistribution DistributionElse;

            public Condition(int limit, IDistribution distributionIf, IDistribution distributionElse)
            {
                this.Limit       = limit;
                DistributionIf   = distributionIf;
                DistributionElse = distributionElse;
            }

            [Pure]
            public IDistribution GetDistribution(int value)
            {
                return value <= Limit ? DistributionIf : DistributionElse;
            }
        }

        private Conditioned(IDistribution source, Condition condition)
        {
            this.source    = source;
            this.condition = condition;

            var values = source.Support().ToList();
            foreach (var value in values)
            {
                var selected = condition.GetDistribution(value);

                foreach (var i in selected.Support())
                {
                    if (!weights.ContainsKey(i))
                        weights[i] = 0;

                    weights[i] += selected.Weight(i);
                }
            }
        }

        public int Sample()
        {
            var value        = source.Sample();
            var distribution = condition.GetDistribution(value);
            return distribution.Sample();
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
