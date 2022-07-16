using System;
using Assets.DiceCalculation;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.UI;
using Single = Assets.DiceCalculation.Single;

public class ValueDiceDescription : DiceDescription
{
    public Button         OperationButton;
    public DiceOperation  Operation = DiceOperation.Add;

    public TMP_InputField Input;

    public int Value;

    public override IDistribution Apply(IDistribution source)
    {
        var dice = Single.Distribution(Operation == DiceOperation.Add ? Value : -Value);
        return source.Add(dice);
    }


    public void OperationChange()
    {
        Operation = Operation switch
                    {
                        DiceOperation.Add      => DiceOperation.Subtract,
                        DiceOperation.Subtract => DiceOperation.Add,
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
                                  _                      => throw new ArgumentOutOfRangeException()
                              };
        }

        if (int.TryParse(Input.text, out int parsed))
        {
            Value = parsed;
        }
        base.Updated();
    }
}
