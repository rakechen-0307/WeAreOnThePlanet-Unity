using Realms;
using Realms.Sync;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Newtonsoft.Json;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Web3Unity.Scripts.Library.Web3Wallet;
using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using UnityEngine.UI;
using Unity.Services.Vivox;

public class ClientTransactionTest : MonoBehaviour
{
    // Start is called before the first frame update
    public static BigInteger price = 100;
    public Button checkBalanceMint;
    public LoadedData loadData;
    private async Task<bool> CheckBalanceAndMint(int _id)
    {
        string method = "balanceOf";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);

        Debug.Log("account = " + loadData.account);

        var data = await contract.Call(method, new object[]
        {
                loadData.account
        });


        BigInteger balanceOf = BigInteger.Parse(data[0].ToString());
        BigInteger realPrice = (BigInteger)1000000000000000000 * price;
        Debug.Log("Balance Of: " + balanceOf);
        Debug.Log("Price:" + realPrice);
        if (balanceOf < realPrice)
        {
            Debug.Log("Your balance is NOT enough!");
            return false;
        }
        else
        {
            string[] preJsonData = { "mint", PlayerPrefs.GetString("Account"), _id.ToString() };
            string jsonData = JsonConvert.SerializeObject(preJsonData);
            string signature = await Web3Wallet.Sign(jsonData);
            string[] messageObj = { jsonData, signature };
            string message = JsonConvert.SerializeObject(messageObj);
            Debug.Log(message);

            await VivoxService.Instance.SendChannelTextMessageAsync("hostChannel", message);
            await VivoxService.Instance.LeaveAllChannelsAsync();
            // SendMessageRequest(message);
            return true;
        }

        //string method = "name";

        //var provider = new JsonRpcProvider(ContractManager.RPC);

        //Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);

        //Debug.Log("account = " + loadData.account);

        //var data = await contract.Call(method, new object[]{});

        //string nameTest = data[0].ToString();
        //Debug.Log(nameTest);

        //string[] preJsonData = { "mint", loadData.account, _id.ToString() };
        //string jsonData = JsonConvert.SerializeObject(preJsonData);
        //string signature = await Web3Wallet.Sign(jsonData);
        //string[] messageObj = { jsonData, signature };
        //string message = JsonConvert.SerializeObject(messageObj);
        //Debug.Log(message);
        //// SendMessageRequest(message);
        //return true;

    }

    public async void OnChannelJoined(string channelName) //傳訊息給host -> 離開host channel
    {
        if (channelName == "hostChannel")
        {
            Debug.Log("nice");

            CheckBalanceAndMint(87);
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
    void Start()
    {
        // log function, delay time, repeat interval        
        // InvokeRepeating("rsay", 0.0f, 1.0f);
        //RealmSetup();
        //checkBalanceMint.onClick.AddListener(()=> { CheckBalanceAndMint(87); });
        VivoxService.Instance.ChannelJoined += OnChannelJoined;
        checkBalanceMint.onClick.AddListener(() => { ChannelSwitch(); });
    }


    // Update is called once per frame
    void Update()
    {

    }
}
