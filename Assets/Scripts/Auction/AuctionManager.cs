using Realms.Sync;
using Realms;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Web3Unity.Scripts.Library.Web3Wallet;
using Newtonsoft.Json;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using SysRandom = System.Random;
using Unity.Services.Vivox;

public class AuctionManager : MonoBehaviour
{
    public static AuctionManager instance;
    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-ouawh";
    SysRandom rnd = new SysRandom(Guid.NewGuid().GetHashCode());
    private BigInteger price = 1000000000000000000;

    // Start is called before the first frame update
    void Start()
    {
        RealmSetup();
        VivoxService.Instance.ChannelJoined += OnChannelJoined;
    }

    private async void RealmSetup()
    {
        if (_realm == null)
        {
            _realmApp = App.Create(new AppConfiguration(_realmAppID));
            if (_realmApp.CurrentUser == null)
            {
                _realmUser = await _realmApp.LogInAsync(Credentials.Anonymous());
                Debug.Log("user created");
                _realm = await Realm.GetInstanceAsync(new FlexibleSyncConfiguration(_realmUser));
            }
            else
            {
                _realmUser = _realmApp.CurrentUser;
                Debug.Log("user remain");
                _realm = Realm.GetInstance(new FlexibleSyncConfiguration(_realmUser));
            }
        }
    }
    /*
    private async void CreateAuction(NFTInfo nft, int startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == PlayerPrefs.GetString("Email")).FirstOrDefault();
        var auctionsCount = _realm.All<Auction>().ToArray().Length;
        await _realm.WriteAsync(() =>
        {
            var auction = _realm.Add(new Auction()
            {
                Id = auctionsCount + 1,
                Owner = findPlayer,
                NFT = nft,
                StartTime = startTime,
                EndTime = endTime,
                StartPrice = startPrice,
                BidPrice = startPrice
            });
        });
    }
    */
        
    public async Task<bool> AuctionBid(int id, int bidPrice)
    {
        BigInteger balance = await CheckBalance();
        if(bidPrice <= balance)
        {
            BackendCommunicator.instance.Bid(id, bidPrice);
        }
        return true;
    }
    public async Task<bool> CheckFinishedAuction(string email)
    {
        foreach(Auction auction in BackendCommunicator.instance.FindEndedAuctionsByEmail(email).ToList())
        {
            if (auction.BidPrice == auction.StartPrice)
            {
                BackendCommunicator.instance.UpdateNFTStatus(auction.NFT.Id, false);
            }
            else
            {
                string toEmail = auction.BidPlayer.Email;
                await CheckBalanceAndBusiness(toEmail, auction.NFT.Id);
            }
        }
        return true;
    }
    private async Task<BigInteger> CheckBalance()
    {
        string method = "balanceOf";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);

        var data = await contract.Call(method, new object[]
        {
            PlayerPrefs.GetString("Account")
        });


        BigInteger balanceOf = BigInteger.Parse(data[0].ToString());
        return balanceOf;
    }
    public async Task<NFTStatus> CheckBalanceAndLaunch()
    {
        string method = "balanceOf";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);
        Contract NFTcontract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
        try
        {
            var data = await contract.Call(method, new object[]
            {
                PlayerPrefs.GetString("Account")
            });

            BigInteger nonce = rnd.Next();
            BigInteger balanceOf = BigInteger.Parse(data[0].ToString());
            BigInteger realFee = 5 * price;
            Debug.Log("Balance Of: " + balanceOf);
            Debug.Log("Fee:" + realFee);
            if (balanceOf < realFee)
            {
                Debug.Log("Your balance is NOT enough!");
                return NFTStatus.Failure;
            }
            else
            {
                return NFTStatus.Success;
            }
        }
        catch
        {
            return NFTStatus.ContractError;
        }
    }
    public async Task<NFTStatus> CheckBalanceAndBusiness(string toEmail, int _id)
    {
        string method = "balanceOf";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);
        Contract NFTcontract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
        try
        {
            var data = await contract.Call(method, new object[]
            {
                PlayerPrefs.GetString("Account")
            });

            BigInteger nonce = rnd.Next();
            BigInteger balanceOf = BigInteger.Parse(data[0].ToString());
            BigInteger realFee = 5*price;
            Debug.Log("Balance Of: " + balanceOf);
            Debug.Log("Fee:" + realFee);
            if (balanceOf < realFee)
            {
                Debug.Log("Your balance is NOT enough!");
                return NFTStatus.Failure;
            }
            else
            {
                method = "getTransferPreSignedHash";
                string toAccount = BackendCommunicator.instance.FindOnePlayerByEmail(toEmail).Account;
                string[] preJsonData = { "business", PlayerPrefs.GetString("Email"), toEmail, _id.ToString(), nonce.ToString() };
                string jsonData = JsonConvert.SerializeObject(preJsonData);
                data = await NFTcontract.Call(method, new object[]
                {
                    PlayerPrefs.GetString("Account"),
                    toAccount,
                    _id.ToString(),
                    nonce.ToString()
                });
                var result_hash = BitConverter.ToString((byte[])data[0]).Replace("-", string.Empty).ToLower();
                string signature = await Web3Wallet.Sign(result_hash);
                string[] messageObj = { jsonData, result_hash, signature };
                string message = JsonConvert.SerializeObject(messageObj);
                Debug.Log(message);
                // SendMessageRequest(message);
                MessageLaunch(message);
                return NFTStatus.Success;
            }
        }
        catch
        {
            return NFTStatus.ContractError;
        }
    }
    public async void OnChannelJoined(string channelName) //傳訊息給host -> 離開host channel
    {
        if (channelName == "hostChannel")
        {
            Debug.Log("nice");
        }
    }
    public async void MessageLaunch(string message) //進host channel -> OnChannelJoined
    {
        await VivoxService.Instance.LeaveAllChannelsAsync();

        string channelToJoin = "hostChannel";
        JoinChannelAsync(channelToJoin, message);//進host channel

        Debug.Log(channelToJoin);
    }
    public async void JoinChannelAsync(string channelName, string message)
    {
        await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.TextOnly);
        await VivoxService.Instance.SendChannelTextMessageAsync("hostChannel", message);
        await VivoxService.Instance.LeaveAllChannelsAsync();
    }
}
