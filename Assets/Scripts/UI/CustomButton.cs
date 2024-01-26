using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    private Button button;
    
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    protected virtual void OnClick()
    {
        Debug.Log("clicked");
    }
}
