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
    public TMP_InputField mintInputField;



    SysRandom rnd = new SysRandom(Guid.NewGuid().GetHashCode());
    private BigInteger _nonce = 1;

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


    void OnChannelMessageReceived(VivoxMessage message) 
    {
        Debug.Log(message.MessageText);
        string[] messageObj = JsonConvert.DeserializeObject<string[]>(message.MessageText);
        string[] PreJsonData = JsonConvert.DeserializeObject<string[]>(messageObj[0]);
        Debug.Log("originalMessage = " + messageObj[0] + " , signatureString = " + messageObj[1] + " , function = " + PreJsonData[0]);
        switch (PreJsonData[0])
        {
            case "mint":
                string verifiedAddress = SignVerifySignature(messageObj[1], messageObj[0]);
                Debug.Log("verifiedAddress = " + verifiedAddress);
                string to = BackendCommunicator.instance.FindOnePlayerByEmail(PreJsonData[1]).Account;
                if (verifiedAddress == to)
                {   
                    Debug.Log("MintNFTTo = " + to);
                    NFTMint(to, Int16.Parse(PreJsonData[2]));
                    TokenTransfer(to, 4, 1);
                    //狀態改成isMinted = true
                }
                else
                {
                    //驗證不正確，要把pending改回false
                }
                break;
            case "transfer":
                break;
            default:
                break;
        }
    }

    public string SignVerifySignature(string signatureString, string originalMessage)
    {
        var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
        var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
        var signature = MessageSigner.ExtractEcdsaSignature(signatureString);
        var key = EthECKey.RecoverFromSignature(signature, msgHash);
        return key.GetPublicAddress();
    }

    private async void TokenMint()
    {
        var method = "mint";

        var to = mintInputField.text;
        string amount = "5000000000000000000000000";
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

    private async void NFTMint(string to, int id)
    {
        var method = "safeMint";

        var uri = "ipfs://QmTQqtfx15vKbs5dKdhxgiTuY7eBATcJpFy9XQEhKTpxTU/0";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
            var data = contract.Calldata(method, new object[]
            {
                to,
                uri,
                id
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

    private async void TokenTransfer(string from, BigInteger value, BigInteger fee)
    {
        // Send { from, to, value, fee, nonce, hash, signature } to host 
        var method = "transferPreSigned";

        string receiver = ContractManager.HostAddress;

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);

            var data = contract.Calldata(method, new object[]
            {
                from,
                receiver,
                value.ToString(),
                fee.ToString(),
                _nonce.ToString()
            });
            // send transaction
            string response = await Web3Wallet.SendTransaction(PlayerPrefs.GetString("ChainID"), ContractManager.TokenContract, "0", data, "", "");
            // display response in game
            print(response);
            print("Transaction successful!");
        }
        catch
        {
            print("Error with the transaction");
        }
        _nonce = rnd.Next();
    }

    void Start()
    {
        _nonce = rnd.Next();
        VivoxService.Instance.ChannelMessageReceived += OnChannelMessageReceived;
        ChannelSwitch();
        mint.onClick.AddListener(() => { TokenMint(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
