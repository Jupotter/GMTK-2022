using System.Linq;
using Assets.DiceCalculation;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    public DisplayDistribution Display;

    public DiceList SelectedList { get; private set; }

    public DiceList DefaultList;

    public IDistribution TargetDistribution = Uniform.Distribution(1, 6).Repeat(3).Add(Uniform.Distribution(1, 20));


    private Camera mainCamera;


    public void Start()
    {
        mainCamera   = Camera.main;
        SelectedList = DefaultList;

        MoveDisplayToViewport();
        Display.SetDistribution(DefaultList.GetDistribution(), TargetDistribution);
    }

    public void Update()
    {
        MoveDisplayToViewport();
    }

    private void MoveDisplayToViewport()
    {
        var screenHeight = mainCamera.scaledPixelHeight;
        var screenWidth  = mainCamera.scaledPixelWidth;

        var viewPortBottom = 350;
        var viewPortLeft   = 300;

        var viewportBottomLeft = new Vector3(viewPortLeft, viewPortBottom);

        var bottomLeft = mainCamera.ScreenToWorldPoint(viewportBottomLeft);
        var topRight   = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight));

        var center = (bottomLeft + topRight) / 2;
        center.z = 0;

        var scale = topRight - bottomLeft;
        scale.z = 1;

        Display.transform.position   = center;
        Display.transform.localScale = scale;
    }

    public void SelectDiceList(DiceList list)
    {
        SelectedList = list;
    }

    public void SelectDefaultList()
    {
        SelectedList = DefaultList;
    }

    public void UpdateDisplay()
    {
        Display.SetDistribution(DefaultList.GetDistribution(), TargetDistribution);
        CheckVictory();
    }

    public void CheckVictory()
    {
        var tested        = DefaultList.GetDistribution();
        var targetSupport = TargetDistribution.Support().ToList();

        var testedSupport = tested.Support().ToList();

        foreach (var value in targetSupport)
        {
            var ok = testedSupport.Contains(value);
            testedSupport.Remove(value);
            ok &= tested.Weight(value) == TargetDistribution.Weight(value);

            if (!ok)
            {
                Debug.Log("Not ok");
                return;
            }
        }

        if (testedSupport.Count > 0)
        {
            Debug.Log("Not ok");
            return;
        }

        Debug.Log("OK");
    }
}
