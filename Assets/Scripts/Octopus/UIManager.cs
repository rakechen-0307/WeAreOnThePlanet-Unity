using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance;

    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI buttonText;
    public GameObject dialogBox;
    public GameObject button;

    public string State;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UIManager.Instance.UpdateDialog("none", "");
        }
        else Destroy(gameObject);
        ToggleDialog(false);
        State = "none";
    }
    
    public void ToggleDialog(bool mode)
    {
        dialogBox.SetActive(mode);
        button.SetActive(mode);
        if (mode) buttonText.text = "OK";
        else buttonText.text = "";
    }

    public void UpdateDialog(string state, string message)
    {
        //text part
        if (dialogText != null) dialogText.text = message;
        else dialogText.text = "";
        if (message == "") ToggleDialog(false);
        else ToggleDialog(true);

        //button part
        State = state;
    }
}
