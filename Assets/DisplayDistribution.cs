using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Assets.DiceCalculation;
using TMPro;
using UnityEngine;

public class DisplayDistribution : MonoBehaviour
{
    private Camera        mainCamera;
    private RectTransform canvas;

    public GameObject LabelPrefab;
    public GameObject LabelTarget;
    public int        MaxLabels = 10;

    public int PaddingTop    = 0;
    public int PaddingBottom = 0;
    public int PaddingLeft   = 0;
    public int PaddingRight  = 0;

    public LineRenderer CurrentDiceLine;
    public LineRenderer TargetDiceLine;

    public void Start()
    {
        canvas     = GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        mainCamera = Camera.main;

        MoveDisplayToViewport();
    }

    public void Update()
    {
        MoveDisplayToViewport();
    }

    private void MoveDisplayToViewport()
    {
        var screenHeight = mainCamera.scaledPixelHeight;
        var screenWidth  = mainCamera.scaledPixelWidth;

        var viewPortBottom = 250;
        var viewPortLeft   = 300;

        var viewportBottomLeft = new Vector3(viewPortLeft, viewPortBottom);
        var viewPortTopRight   = new Vector3(screenWidth,  screenHeight);

        var bottomLeft = mainCamera.ScreenToWorldPoint(viewportBottomLeft);
        var topRight   = mainCamera.ScreenToWorldPoint(viewPortTopRight);

        bottomLeft.x += PaddingLeft   / 100f;
        bottomLeft.y += PaddingBottom / 100f;
        topRight.x   += PaddingRight  / 100f;
        topRight.y   += PaddingTop    /100f;

        var center = (bottomLeft + topRight) / 2;
        center.z = 0;

        var scale = topRight - bottomLeft;
        scale.z = 1;

        transform.position   = center;
        transform.localScale = scale;

        var reverseScale = new Vector3(1, scale.x / scale.y);
        canvas.localScale = reverseScale * 0.001f;
    }


    public void SetDistribution(IDistribution distribution, IDistribution targetDistribution)
    {
        foreach (Transform label in LabelTarget.transform)
        {
            Destroy(label.gameObject);
        }

        var targetValues    = targetDistribution.Support().OrderBy(v => v).ToList();
        var maxTargetWeight = targetValues.Select(targetDistribution.Weight).Max();

        var values           = distribution.Support().OrderBy(v => v).ToList();
        var maxCurrentWeight = values.Select(distribution.Weight).Max();


        var maxWeight = maxCurrentWeight > maxTargetWeight ? maxCurrentWeight : maxTargetWeight;

        if (maxWeight == 0)
            return;

        var minValue = Math.Min(values[0], targetValues[0]);
        var maxValue = Math.Max(values[^1], targetValues[^1]);

        var count = maxValue - minValue + 1;
        Debug.Log(count);
        if (count < MaxLabels)
        {
            var labels = Enumerable.Range(minValue, count);
            foreach (var label in labels)
            {
                var labelObject = Instantiate(LabelPrefab, LabelTarget.transform);
                var labelText   = labelObject.GetComponentInChildren<TMP_Text>();
                labelText.text = label.ToString();
            }
        }
        else
        {
            var labels = GetLabels(minValue, maxValue, 2);
            labels.Add(minValue);
            labels.Add(maxValue);

            labels = labels.OrderBy(x => x).ToList();

            foreach (var label in labels)
            {
                var labelObject = Instantiate(LabelPrefab, LabelTarget.transform);
                var labelText   = labelObject.GetComponentInChildren<TMP_Text>();
                labelText.text = label.ToString();
            }
        }

        var delta  = transform.localScale.x / (count - 1);
        var origin = new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2);

        var currentFirst = values[0];
        var targetFirst  = targetValues[0];

        var offset = currentFirst - targetFirst;

        var posX = offset > 0 ? delta * offset : 0f;
        posX = 0;

        CurrentDiceLine.positionCount = values.Count;
        var previousValue = minValue;
        for (var i = 0; i < values.Count; i++)
        {
            var value = values[i];
            posX += delta * (value - previousValue);
            var weight = distribution.Weight(value) * (1.0f / maxCurrentWeight) * transform.localScale.y;
            CurrentDiceLine.SetPosition(i, origin + new Vector3(posX, weight));
            previousValue = value;
        }

        posX = offset < 0 ? delta * -offset : 0f;
        posX = 0;

        TargetDiceLine.positionCount = targetValues.Count;
        previousValue                = minValue;
        for (var i = 0; i < targetValues.Count; i++)
        {
            var value = targetValues[i];
            posX += delta * (value - previousValue);
            var weight = targetDistribution.Weight(value) * (1.0f / maxTargetWeight) * transform.localScale.y;
            TargetDiceLine.SetPosition(i, origin + new Vector3(posX, weight));
            previousValue = value;
        }
    }

    private ICollection<int> GetLabels(int min, int max, int currentCount)
    {
        if (currentCount >= MaxLabels)
            return new List<int>();

        List<int> labels = new List<int>();

        var midpoint = (max - min) / 2 + min;
        labels.Add(midpoint);
        var left  = GetLabels(min,      midpoint, currentCount + 1);
        var right = GetLabels(midpoint, max,      currentCount + 1);

        if (left.Count + right.Count + currentCount < MaxLabels)
        {
            labels.AddRange(left);
            labels.AddRange(right);
        }

        return labels;
    }
}
