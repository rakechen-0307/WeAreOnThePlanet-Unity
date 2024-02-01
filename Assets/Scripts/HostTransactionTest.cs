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
using System.Threading.Tasks;

public class HostTransactionTest : MonoBehaviour
{
    //測試按鈕
    public Button mint;
    public TMP_InputField mintInputField;
    public BigInteger price = 1000000000000000000;



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


    public async void OnChannelMessageReceived(VivoxMessage message) 
    {
        Debug.Log(message.MessageText);
        string[] messageObj = JsonConvert.DeserializeObject<string[]>(message.MessageText);
        string[] PreJsonData = JsonConvert.DeserializeObject<string[]>(messageObj[0]);
        Debug.Log("originalMessage = " + messageObj[0] + " , function = " + PreJsonData[0]);
        switch (PreJsonData[0])
        {
            case "mint":
                string verifiedAddress = SignVerifySignature(messageObj[1], messageObj[0]);
                Debug.Log("verifiedAddress = " + verifiedAddress);
                string to = BackendCommunicator.instance.FindOnePlayerByEmail(PreJsonData[1]).Account;
                if (verifiedAddress == to)
                {   
                    Debug.Log("MintNFTTo = " + to);
                    await NFTMint(to, Int16.Parse(PreJsonData[2]));
                    await TokenTransfer(to, (BigInteger)4*price, (BigInteger)1*price);
                    //狀態改成isMinted = true
                    BackendCommunicator.instance.UpdateNFTMintStatus(Int16.Parse(PreJsonData[2]),true);
                    BackendCommunicator.instance.UpdateNFTStatus(Int16.Parse(PreJsonData[2]),false);
                    //重新check balance，把畫面中的錢包金額改掉
                }
                else
                {
                    BackendCommunicator.instance.UpdateNFTStatus(Int16.Parse(PreJsonData[2]), false);
                    //驗證不正確，要把pending改回false
                }
                break;
            case "transfer":
                verifiedAddress = SignVerifySignature(messageObj[2], messageObj[1]);
                Debug.Log("verifiedAddress = " + verifiedAddress);
                string from = BackendCommunicator.instance.FindOnePlayerByEmail(PreJsonData[1]).Account;
                Debug.Log("from = " + from);
                to = BackendCommunicator.instance.FindOnePlayerByEmail(PreJsonData[2]).Account;
                Debug.Log("to = " + to);
                if (verifiedAddress == from)
                {
                    Debug.Log("transfer success!");
                    await NFTTransfer(from, to, Int16.Parse(PreJsonData[3]));
                    await TokenTransfer(from, (BigInteger)4 * price, (BigInteger)1 * price);
                    //transfer成功
                    BackendCommunicator.instance.UpdateNFTOwner(Int16.Parse(PreJsonData[3]),PreJsonData[2]);
                    BackendCommunicator.instance.ChangeNFTOwner(PreJsonData[1], PreJsonData[2], Int16.Parse(PreJsonData[3]));
                    //重新check balance，把畫面中的錢包金額改掉
                }
                else
                {
                    Debug.Log("transfer fail!");
                    //transfer 失敗
                }
                BackendCommunicator.instance.UpdateNFTStatus(Int16.Parse(PreJsonData[3]), false);
                break;
            case "business":
                verifiedAddress = SignVerifySignature(messageObj[2], messageObj[0]);
                from = BackendCommunicator.instance.FindOnePlayerByEmail(PreJsonData[1]).Account;
                to = BackendCommunicator.instance.FindOnePlayerByEmail(PreJsonData[2]).Account;
                int howMuch = BackendCommunicator.instance.FindAuctionByNFTId(Int16.Parse(PreJsonData[3])).BidPrice;
                if (verifiedAddress == from)
                {
                    await NFTTransfer(from, to, Int16.Parse(PreJsonData[3]));
                    await TokenTransfer(from, 4 * price, 1 * price);
                    await TokenTransferBussiness(from, to, howMuch * price, 1);
                    //business成功]
                    BackendCommunicator.instance.UpdateNFTOwner(Int16.Parse(PreJsonData[3]), PreJsonData[2]);
                    BackendCommunicator.instance.ChangeNFTOwner(PreJsonData[1], PreJsonData[2], Int16.Parse(PreJsonData[3]));
                    //重新check balance，把畫面中的錢包金額改掉
                }
                else
                {
                    //business 失敗
                    Debug.Log("business fail!");
                }
                BackendCommunicator.instance.UpdateNFTStatus(Int16.Parse(PreJsonData[3]), false);
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
        BigInteger amount = 5 * price;
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

    private async Task<bool> NFTMint(string to, int id)
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
        return true;
    }

    private async Task<bool> TokenTransfer(string from, BigInteger value, BigInteger fee)
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
                value,
                fee,
                _nonce.ToString()
            });
            Debug.Log("value = " + value.ToString());
            Debug.Log("fee = " + fee.ToString());
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
        return true;
    }

    private async Task<bool> TokenTransferBussiness(string from, string receiver, BigInteger value, BigInteger fee)
    {
        // Send { from, to, value, fee, nonce, hash, signature } to host 
        var method = "transferPreSigned";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);

            var data = contract.Calldata(method, new object[]
            {
                from,
                receiver,
                value,
                fee,
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
        return true;
    }

    private async Task<bool> NFTTransfer(string from, string receiver, int tokenId)
    {
        // Send { from, to, tokenId, nonce, hash, signature } to host 
        var method = "transferPreSigned";
        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);

            //Check hash (information is correct)

            var data = contract.Calldata(method, new object[]
            {
                from,
                receiver,
                tokenId.ToString(),
                _nonce.ToString(),
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

        _nonce = rnd.Next();
        return true;
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
