using TMPro;
using UnityEngine;
using Unity.Services.Vivox;
using Newtonsoft.Json;

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
        VivoxService.Instance.ChannelJoined += OnChannelJoined;
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
            int award = await BackendCommunicator.instance.ProgressUpdate(loadedData.playerId, 1);
            loadedData.achievements[5].progress += 1;
            mainPlanetInit.initGarden();
            if (award > 0)
            {
                string[] preJsonData = { "reward", PlayerPrefs.GetString("Email"), award.ToString() };
                string jsonData = JsonConvert.SerializeObject(preJsonData);
                string signature = "";
                string[] messageObj = { jsonData, signature };
                string message = JsonConvert.SerializeObject(messageObj);
                Debug.Log(message);
                // SendMessageRequest(message);
                MessageLaunch(message);
            }

        }
    }

    public async void OnChannelJoined(string channelName) 
    {
        if (channelName == "hostChannel")
        {
            Debug.Log("nice");
        }
    }
    public async void MessageLaunch(string message) 
    {
        await VivoxService.Instance.LeaveAllChannelsAsync();

        string channelToJoin = "hostChannel";
        JoinChannelAsync(channelToJoin, message);

        Debug.Log(channelToJoin);
    }
    public async void JoinChannelAsync(string channelName, string message)
    {
        try
        {
            await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.TextOnly);
        }
        catch
        {
            Debug.Log("Join error");
        }

        await VivoxService.Instance.SendChannelTextMessageAsync("hostChannel", message);

        try
        {
            await VivoxService.Instance.LeaveAllChannelsAsync();
        }
        catch
        {
            Debug.Log("Leave error");
        }
    }
}
