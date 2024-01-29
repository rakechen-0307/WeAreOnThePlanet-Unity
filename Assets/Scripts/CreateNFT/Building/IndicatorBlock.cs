using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class IndicatorBlock : MonoBehaviour
{
    [SerializeField]
    private ColorManager colorManager;

    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (colorManager != null)
        {
            material.color = colorManager.getBrushColor();
        }
    }
}
