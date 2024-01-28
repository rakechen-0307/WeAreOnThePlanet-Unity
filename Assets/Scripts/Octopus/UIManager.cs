using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance;

    public TextMeshProUGUI myText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UpdateText("");
        }
        else Destroy(gameObject);
    }    

    public void UpdateText(string message)
    {
        if (myText != null) myText.text = message;
        else myText.text = "";
    }

    public void UpdateButton(string message)
    {

    }
}
