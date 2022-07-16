using System;
using System.Linq;
using Assets.DiceCalculation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class DiceDescription : MonoBehaviour
{
    public abstract IDistribution Apply(IDistribution source);


    protected DiceList ParentList { get; private set; }

    public void SetParent(DiceList list, bool first)
    {
        this.ParentList = list;
        this.IsFirst    = first;
    }

    public void Start()
    {
        Updated();
    }

    public virtual void Updated()
    {
        ParentList.UpdateHistogram();
    }

    public void MoveLeft()
    {
        ParentList.MoveLeft(this);
    }

    public void MoveRight()
    {
        ParentList.MoveRight(this);
    }

    private bool isFirst;

    public bool IsFirst
    {
        get => isFirst;
        set
        {
            isFirst = value;
            Updated();
            ;
        }
    }

    public void Remove()
    {
        ParentList.Delete(this);
    }
}
