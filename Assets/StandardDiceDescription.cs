using System;
using Assets.DiceCalculation;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.UI;

public class StandardDiceDescription : DiceDescription
{
    public Button        OperationButton;
    public DiceOperation Operation = DiceOperation.Add;

    public TMP_Text      Text;

    public int Value;
    public int Count;
    
    public override IDistribution Apply(IDistribution source)
    {
        
        var dice = Uniform.Distribution(1, Value).Repeat(Count);
        if (IsFirst)
        {
            return dice;
        }

        return Operation switch
               {
                   DiceOperation.Add      => source.Add(dice),
                   DiceOperation.Subtract => source.Substract(dice),
                   DiceOperation.Multiply => source.Multiply(dice),
                   _                      => throw new ArgumentOutOfRangeException()
               };
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


    public override void Updated()
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

        Text.text = $"{Count}D{Value}";

        base.Updated();
    }

    [UsedImplicitly]
    public void Increment()
    {
        Count++;
        Updated();
    }

    [UsedImplicitly]
    public void Decrement()
    {
        if (Count <= 1)
        {
            ParentList.Delete(this);
            return;
        }

        Count--;

        Updated();
    }
}