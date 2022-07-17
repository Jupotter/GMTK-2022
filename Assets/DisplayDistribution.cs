using System;
using System.Linq;
using Assets.DiceCalculation;
using TMPro;
using UnityEngine;

public class DisplayDistribution : MonoBehaviour
{
    public TMP_Text TextPrefab;

    public LineRenderer CurrentDiceLine;
    public LineRenderer TargetDiceLine;

    public void SetDistribution(IDistribution distribution, IDistribution targetDistribution)
    {
        var texts = GetComponentsInChildren<TMP_Text>();
        foreach (var bar in texts)
        {
            Destroy(bar);
        }

        var targetValues    = targetDistribution.Support().OrderBy(v => v).ToList();
        var maxTargetWeight = targetValues.Select(targetDistribution.Weight).Max();

        var values           = distribution.Support().OrderBy(v => v).ToList();
        var maxCurrentWeight = values.Select(distribution.Weight).Max();


        var maxWeight = maxCurrentWeight > maxTargetWeight ? maxCurrentWeight : maxTargetWeight;

        if (maxWeight == 0)
            return;

        var count = Math.Max(values[^1], targetValues[^1]) - Math.Min(values[0], targetValues[0]);

        var delta  = this.transform.localScale.x / count;
        var origin = new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2);

        var currentFirst = values[0];
        var targetFirst  = targetValues[0];

        var offset = currentFirst - targetFirst;

        var posX = offset > 0 ? delta * offset : 0f;
        CurrentDiceLine.positionCount = values.Count;
        for (var i = 0; i < values.Count; i++)
        {
            var value  = values[i];
            var weight = distribution.Weight(value) * (1.0f / maxCurrentWeight) * transform.localScale.y;
            CurrentDiceLine.SetPosition(i, origin + new Vector3(posX, weight));

            posX += delta;
        }

        posX                         = offset < 0 ? delta * -offset : 0f;
        TargetDiceLine.positionCount = targetValues.Count;
        for (var i = 0; i < targetValues.Count; i++)
        {
            var value  = targetValues[i];
            var weight = targetDistribution.Weight(value) * (1.0f / maxTargetWeight) * transform.localScale.y;
            TargetDiceLine.SetPosition(i, origin + new Vector3(posX, weight));

            posX += delta;
        }
    }
}
