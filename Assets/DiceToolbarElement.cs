using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceToolbarElement : MonoBehaviour
{
    [SerializeReference]
    public DiceDescription DicePrefab;

    public int Value = 6;

    private DiceList list;

    public void Start()
    {
        list = FindObjectOfType<DiceList>();
    }

    public void AddPressed()
    {
        var dice = Instantiate(DicePrefab, list.transform);
        if (dice is StandardDiceDescription std)
        {
            std.Value = Value;
        }
        list.Add(dice);
    }
}
