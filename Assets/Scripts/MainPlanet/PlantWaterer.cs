using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlantWaterer : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private bool visible = false;

    [SerializeField]
    private GameObject noticeUI;

    [SerializeField]
    private TMP_Text noticeText;

    [SerializeField]
    private float waterDelay = 100f;

    [SerializeField]
    private MainPlanetInit mainPlanetInit;

    private Renderer rd;
    private bool triggerActive = false;
    private float lastWaterTime;

    private void Start()
    {
        lastWaterTime = Time.time;

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
    }
    private void OnTriggerStay()
    {
        float pastTime = Time.time - lastWaterTime;
        if (pastTime < waterDelay)
        {
            triggerActive = false;
            noticeText.text = $"Water the Plants Again in {(int)(waterDelay - pastTime)} seconds";
        }
        else
        {
            triggerActive = true;
            noticeText.text = "[Enter] Water the Plants";
        }
    }
    private void OnTriggerExit()
    {
        noticeUI.SetActive(false);
        triggerActive = false;
    }

    private async void Update()
    {
        if (triggerActive && Input.GetKeyDown(KeyCode.Return))
        {
            lastWaterTime = Time.time;
            triggerActive = false;
            bool done = await BackendCommunicator.instance.FlowerAddExp(loadedData.playerId, 1);
            loadedData.experience += 1;
            int award = await BackendCommunicator.instance.ProgressUpdate(loadedData.playerId, 5);
            mainPlanetInit.initGarden();
            if (award > 0)
            {
                // TODO: give money
            }

        }
    }
}
