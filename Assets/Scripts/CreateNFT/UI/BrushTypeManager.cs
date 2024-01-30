using UnityEngine;
using UnityEngine.UI;

public class BrushTypeManager : CustomButton
{
    private Image image;
    private BrushType brushType = BrushType.Add;

    protected override void Start()
    {
        base.Start();
        image = GetComponent<Image>();
        image.color = Color.white;
    }
    protected override void OnClick()
    {
        if (brushType == BrushType.Add)
        {
            brushType = BrushType.Remove;
            image.color = Color.red;
        }
        else
        {
            brushType = BrushType.Add;
            image.color = Color.white;
        }
    }
    public BrushType getBrushType()
    {
        return brushType;
    }
}
public enum BrushType
{
    Add,
    Remove
}