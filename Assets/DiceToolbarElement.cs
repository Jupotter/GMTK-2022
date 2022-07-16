using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceToolbarElement : MonoBehaviour
{
    [SerializeReference]
    public DiceDescription DicePrefab;

    public int Value = 6;

    private SceneContext context;

    public void Start()
    {
        context = FindObjectOfType<SceneContext>();
    }

    public void AddPressed()
    {
        var list = context.SelectedList;
        var dice = Instantiate(DicePrefab, list.transform);
        if (dice is StandardDiceDescription std)
        {
            std.Value = Value;
        }
        list.Add(dice);
    }
}
