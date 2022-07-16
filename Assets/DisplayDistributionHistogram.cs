using System.Linq;
using Assets.DiceCalculation;
using UnityEngine;

public class DisplayDistributionHistogram : MonoBehaviour
{
    public HistogramBar HistogramBarPrefab;

    public void Start()
    {
        var d6           = Uniform.Distribution(1, 6);
        var distribution = d6.Repeat(2).Add(3);

        var maxWeight = distribution.Support().Select(v => distribution.Weight(v)).Max();
        foreach (var i in distribution.Support())
        {
            var    bar    = Instantiate(HistogramBarPrefab, transform);
            double weight = distribution.Weight(i) * (1.0 / maxWeight);
            Debug.Log($"Weight for {i}: {weight}");
            bar.SetValue(i, weight);
        }
    }
}
