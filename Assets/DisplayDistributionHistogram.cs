using System.Linq;
using Assets.DiceCalculation;
using UnityEngine;

public class DisplayDistributionHistogram : MonoBehaviour
{
    public HistogramBar HistogramBarPrefab;

    public LineRenderer LineRenderer;

    public void UpdateFrom(DiceList source)
    {
        SetDistribution(source.GetDistribution());
    }

    public void SetDistribution(IDistribution distribution)
    {
        var bars = GetComponentsInChildren<HistogramBar>();
        foreach (var bar in bars)
        {
            Destroy(bar);
        }

        var values    = distribution.Support().OrderBy(v => v).ToList();
        var maxWeight = values.Select(distribution.Weight).Max();

        if (maxWeight == 0)
            return;

        var count = values.Count;
        LineRenderer.positionCount = count;

        var delta  = this.transform.localScale.x / count;
        var origin = new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2);

        var posX = 0f;
        for (var i = 0; i < values.Count; i++)
        {
            var value  = values[i];
            var weight = distribution.Weight(value) * (1.0f / maxWeight) * transform.localScale.y;
            LineRenderer.SetPosition(i, origin + new Vector3(posX, weight));

            posX += delta;
            //var    bar    = Instantiate(HistogramBarPrefab, transform);
            //double weight = distribution.Weight(i) * (1.0 / maxWeight);
            //Debug.Log($"Weight for {i}: {weight}");
            //bar.SetValue(i, weight);
        }
    }
}
