using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Vivox;
using UnityEngine.UI;
using Newtonsoft.Json;

//********這是給host的********//
public class Receive_and_Transfer : MonoBehaviour
{
    // Start is called before the first frame update
    public class Hash_and_Config //這是我隨便測的object
    {
        public string hash { get; set; }
        public string config { get; set; }
    }

    public void ChannelSwitch() //永遠待在host channel
    {
        string channelToJoin = "hostChannel";
        JoinChannelAsync(channelToJoin); //進host channel

        Debug.Log(channelToJoin);
    }
    public async void JoinChannelAsync(string channelName)
    {
        await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.TextOnly);
    }
    void Start()
    {
        VivoxService.Instance.ChannelMessageReceived += OnChannelMessageReceived;
        ChannelSwitch();
    }

    void OnChannelMessageReceived(VivoxMessage message) //接收到訊息之後解碼 Debug.Log出來
    {
        //Debug.Log(message.MessageText);
        Hash_and_Config incomingData = JsonConvert.DeserializeObject<Hash_and_Config>(message.MessageText);
        Debug.Log("Hash = " + incomingData.hash);
        Debug.Log("Config = " + incomingData.config);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
