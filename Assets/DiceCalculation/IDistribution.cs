using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DiceCalculation
{
    public interface IDistribution
    {
        int              Sample();
        IEnumerable<int> Support();
        long             Weight(int t);
    }
}
