using Assets.DiceCalculation;
using TMPro;

public class ConditionDiceDescription : DiceDescription
{
    public DiceList IfDiceList;
    public DiceList ElseDiceList;

    public TMP_InputField Input;

    public int Value;

    private SceneContext context;

    public new void Start()
    {
        base.Start();
        context = FindObjectOfType<SceneContext>();
    }

    public override IDistribution Apply(IDistribution source)
    {
        var ifDistribution   = IfDiceList.GetDistribution();
        var elseDistribution = ElseDiceList.GetDistribution();

        var condition = new Conditioned.Condition(Value, ifDistribution, elseDistribution);

        return Conditioned.Distribution(source, condition);
    }

    public override void Updated()
    {
        if (int.TryParse(Input.text, out int parsed))
        {
            Value = parsed;
        }

        base.Updated();
    }

    public void SelectIfDiceList(bool selected)
    {
        if (selected)
            context.SelectDiceList(IfDiceList);
        else
            context.SelectDefaultList();
    }

    public void SelectElseDiceList(bool selected)
    {
        if (selected)
            context.SelectDiceList(ElseDiceList);
        else
            context.SelectDefaultList();
    }
}
