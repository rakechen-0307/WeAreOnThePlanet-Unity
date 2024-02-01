using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EventTrigger : MonoBehaviour
{
    [SerializeField]
    private bool visible = false;

    [SerializeField]
    private GameObject noticeUI;

    [SerializeField]
    private MainPlanetTravel mainPlanetTravel;

    private bool triggerActive = false;

    private Renderer rd;
    
    private void Start()
    {
        rd = GetComponent<Renderer>();
        if (rd != null)
        {
            rd.enabled = visible;
        }

        noticeUI.SetActive(false);
    }
    private void OnTriggerEnter()
    {
        noticeUI.SetActive(true);
        triggerActive = true;
    }
    private void OnTriggerExit()
    {
        noticeUI.SetActive(false);
        triggerActive = false;
    }

    private void Update()
    {
        if (triggerActive && Input.GetKeyDown(KeyCode.Return))
        {
            mainPlanetTravel.TravelPlanet(-1);
        }
    }

}
