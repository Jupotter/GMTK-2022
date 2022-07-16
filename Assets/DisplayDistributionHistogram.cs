using System.Linq;
using Assets.DiceCalculation;
using UnityEngine;

public class DisplayDistributionHistogram : MonoBehaviour
{
    public HistogramBar HistogramBarPrefab;
    

    public void SetDistribution(IDistribution distribution)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var values = distribution.Support().OrderBy(v => v).ToList();
        var maxWeight  = values.Select(distribution.Weight).Max();

        if (maxWeight == 0)
            return;

        foreach (var i in values)
        {
            var    bar    = Instantiate(HistogramBarPrefab, transform);
            double weight = distribution.Weight(i) * (1.0 / maxWeight);
            Debug.Log($"Weight for {i}: {weight}");
            bar.SetValue(i, weight);
        }
    }
}
