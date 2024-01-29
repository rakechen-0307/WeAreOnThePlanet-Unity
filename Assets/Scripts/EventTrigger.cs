using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [SerializeField]
    private bool visible = false;
    [SerializeField]
    private GameObject UICanvas;

    private Renderer rd;
    
    private void Start()
    {
        rd = GetComponent<Renderer>();
        if (rd != null)
        {
            rd.enabled = visible;
        }

        UICanvas.SetActive(false);
    }
    private void OnTriggerEnter()
    {
        Debug.Log("Trigger");
        UICanvas.SetActive(true);
    }
    private void OnTriggerExit()
    {
        Debug.Log("Exit");
        UICanvas.SetActive(false);
    }
}
