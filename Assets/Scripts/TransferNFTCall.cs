using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Vivox;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

//********這是給client用的*******//
public class TransferNFTCall : MonoBehaviour
{
    // Start is called before the first frame update
    public Button CallButton; //把hash跟config傳給host
    //public TMP_InputField HashInputField; //填hash
    //public TMP_InputField ConfigInputField; //填config
    public class Hash_and_Config //這是我隨便測的object
    {
        public string hash { get; set; }
        public string config { get; set; }
    }

    //public Hash_and_Config hashconfig = new Hash_and_Config()
    //{
    //    hash = "ThisIsHash",
    //    config = "ThisIsConfig"
    //};

    public async void OnChannelJoined(string channelName) //傳訊息給host -> 離開host channel
    {
        if (channelName == "hostChannel")
        {
            Debug.Log("nice");

        Hash_and_Config hashconfig = new Hash_and_Config()
        {
            //hash = HashInputField.text,
            //config = ConfigInputField.text
            hash = "hash",
            config = "config"
        };

            await VivoxService.Instance.SendChannelTextMessageAsync("hostChannel", JsonConvert.SerializeObject(hashconfig));
            await VivoxService.Instance.LeaveAllChannelsAsync();
            Debug.Log(JsonConvert.SerializeObject(hashconfig));
        }
    }
    public async void ChannelSwitch() //進host channel -> OnChannelJoined
    {
        await VivoxService.Instance.LeaveAllChannelsAsync();

        string channelToJoin = "hostChannel";
        JoinChannelAsync(channelToJoin);//進host channel

        Debug.Log(channelToJoin);
    }
    public async void JoinChannelAsync(string channelName)
    {
        await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.TextOnly);
    }
    void OnChannelMessageReceived(VivoxMessage message) //接收到訊息之後解碼 Debug.Log出來
    {
        //Debug.Log(message.MessageText);
        Hash_and_Config incomingData = JsonConvert.DeserializeObject<Hash_and_Config>(message.MessageText);
        Debug.Log("Hash = " + incomingData.hash);
        Debug.Log("Config = " + incomingData.config);
    }

    void Start()
    {
        VivoxService.Instance.ChannelJoined += OnChannelJoined;
        VivoxService.Instance.ChannelMessageReceived += OnChannelMessageReceived;
        CallButton.onClick.AddListener(() => { ChannelSwitch(); });
    }  

    // Update is called once per frame
    void Update()
    {
        
    }
}