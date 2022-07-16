using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.DiceCalculation
{
    public interface IDistribution
    {
        int              Sample();
        IEnumerable<int> Support();
        int              Weight(int t);
    }
}
