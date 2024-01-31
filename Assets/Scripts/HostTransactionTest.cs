using System;
using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using Nethereum.Signer;
using Web3Unity.Scripts.Library.Web3Wallet;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using SysRandom = System.Random;
using Nethereum.Util;
using System.Text;
using Unity.Collections;
using Unity.Services.Vivox;


public class HostTransactionTest : MonoBehaviour
{
    //測試按鈕
    public Button mint;

    // Start is called before the first frame update
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


    void OnChannelMessageReceived(VivoxMessage message) //接收到訊息之後解碼 Debug.Log出來
    {
        Debug.Log(message.MessageText);
        string[] incomingMessage = JsonConvert.DeserializeObject<string[]>(message.MessageText);
    }

    private async void TokenMint()
    {
        var method = "mint";

        var to = "0xC79dbE9296E54e5C503Bd1820eE5dAC6376c98C5";
        string amount = "5000000000000000000000000";
        string abi = ContractManager.TokenABI;
        string[] obj = { to, amount };
        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);
            string data = contract.Calldata(method, new object[]
            {
                    to,
                    amount.ToString()
            });
            // send transaction
            string response = await Web3Wallet.SendTransaction(PlayerPrefs.GetString("ChainID"), ContractManager.TokenContract, "0", data, "", "");
            // display response in game
            print(response);
            print("Reward claiming successful!");
        }
        catch
        {
            print("Error with the transaction");
        }
    }

    private async void NFTMint()
    {
        var method = "safeMint";

        var to = "0xC79dbE9296E54e5C503Bd1820eE5dAC6376c98C5";
        var uri = "ipfs://QmTQqtfx15vKbs5dKdhxgiTuY7eBATcJpFy9XQEhKTpxTU/0";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
            var data = contract.Calldata(method, new object[]
            {
                to,
                uri
            });
            // send transaction
            string response = await Web3Wallet.SendTransaction(PlayerPrefs.GetString("ChainID"), ContractManager.NFTContract, "0", data, "", "");
            // display response in game
            print(response);
            print("Transaction successful!");
        }
        catch
        {
            print("Error with the transaction");
        }
    }

    void Start()
    {
        VivoxService.Instance.ChannelMessageReceived += OnChannelMessageReceived;
        ChannelSwitch();
        mint.onClick.AddListener(() => { NFTMint(); });

    }

    // Update is called once per frame
    void Update()
    {

    }
}
