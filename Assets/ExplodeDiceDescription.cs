using Assets.DiceCalculation;

public class ExplodeDiceDescription : DiceDescription
{
    public override IDistribution Apply(IDistribution source)
    {
        return source.Explode();
    }
}
