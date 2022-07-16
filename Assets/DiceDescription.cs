using Assets.DiceCalculation;
using UnityEngine;

public abstract class DiceDescription : MonoBehaviour
{
    public abstract IDistribution Apply(IDistribution source);

    protected DiceList ParentList { get; private set; }
    public void SetParent(DiceList list)
    {
        this.ParentList = list;
    }

    public void Start()
    {
        Updated();
    }

    public virtual void Updated()
    {
        ParentList.UpdateHistogram();
    }
}
