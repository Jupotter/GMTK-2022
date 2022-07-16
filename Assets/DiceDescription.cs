using System;
using System.Linq;using Assets.DiceCalculation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class DiceDescription : MonoBehaviour
{
    public abstract IDistribution Apply(IDistribution source);

    public Button OperationButton;

    protected DiceList ParentList { get; private set; }
    public void SetParent(DiceList list, bool first)
    {
        this.ParentList = list;
        this.IsFirst = first;
    }

    public void Start()
    {
        Updated();
    }

    public virtual void Updated()
    {
        OperationButton.gameObject.SetActive(!IsFirst);
        if (!IsFirst)
        {
            var buttonText = OperationButton.GetComponentInChildren<TMP_Text>();

            buttonText.text = Operation switch
                              {
                                  DiceOperation.Add      => "+",
                                  DiceOperation.Subtract => "-",
                                  DiceOperation.Multiply => "*",
                                  _                      => throw new ArgumentOutOfRangeException()
                              };
        }

        ParentList.UpdateHistogram();
    }

    public void OperationChange()
    {
        Operation = Operation switch
                    {
                        DiceOperation.Add      => DiceOperation.Subtract,
                        DiceOperation.Subtract => DiceOperation.Multiply,
                        DiceOperation.Multiply => DiceOperation.Add,
                        _                      => throw new ArgumentOutOfRangeException()
                    };
        Updated();
    }

    public void MoveLeft()
    {
        ParentList.MoveLeft(this);
    }


    public void MoveRight()
    {
        ParentList.MoveRight(this);
    }

    public  DiceOperation Operation = DiceOperation.Add;
    private bool          isFirst;

    public bool IsFirst
    {
        get => isFirst;
        set { isFirst = value; Updated();; }
    }
}