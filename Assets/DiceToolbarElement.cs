using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceToolbarElement : MonoBehaviour
{
    [SerializeReference]
    public DiceDescription DicePrefab;

    private DiceList list;

    public void Start()
    {
        list = FindObjectOfType<DiceList>();
    }

    public void AddPressed()
    {
        list.AddPrefab(DicePrefab);
    }
}
