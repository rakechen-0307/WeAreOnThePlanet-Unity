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

public class AuctionManager : MonoBehaviour
{
    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-ouawh";

    // Start is called before the first frame update
    void Start()
    {
        RealmSetup();
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

    private async void CreateAuction(NFTInfo nft, int startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == PlayerPrefs.GetString("Email")).FirstOrDefault();
        var auctionsCount = _realm.All<Auction>().ToArray().Length;
        try
        {
            string[] data = { "launch", PlayerPrefs.GetString("Account"), (auctionsCount + 1).ToString() };
            string jsonData = JsonConvert.SerializeObject(data);
            string signature = await Web3Wallet.Sign(jsonData);
            string[] messageObj = { jsonData, signature };
            string message = JsonConvert.SerializeObject(messageObj);
            Debug.Log(message);
            // SendMessageRequest(message);
        }
        catch
        {
            Debug.Log("Signing error");
            return;
        }
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
    private List<Auction> PreviewActiveAuctions()
    {
        DateTimeOffset now = System.DateTime.UtcNow;
        List<Auction> activeAuctions = _realm.All<Auction>().Where(auction => (
            auction.EndTime >= now
        )).ToList();
        return activeAuctions;
    }    
    private async void AuctionBid(int id, int bidPrice)
    {
        Auction auction = _realm.All<Auction>().Where(auction => auction.Id == id).FirstOrDefault();
        bool result = await CheckBalanceAndBid(bidPrice, auction.Id);
        if(result)
        {
            PlayerData player = _realm.All<PlayerData>().Where(user => user.Email == PlayerPrefs.GetString("Email")).FirstOrDefault();
            await _realm.WriteAsync(() =>
            {
                auction.BidPlayer = player;
                auction.BidPrice = bidPrice;
            });
        }
    }
    private async Task<bool> CheckBalanceAndBid(int price, int auction)
    {
        try
        {
            string method = "balanceOf";

            var provider = new JsonRpcProvider(ContractManager.RPC);

            Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);

            var data = await contract.Call(method, new object[]
            {
                PlayerPrefs.GetString("Account")
            });


            BigInteger balanceOf = BigInteger.Parse(data[0].ToString());
            BigInteger realPrice = (BigInteger)1000000000000000000 * price;
            Debug.Log("Balance Of: " + balanceOf);
            Debug.Log("Price:" + realPrice);
            if (balanceOf < 100)
            {
                Debug.Log("Your balance is NOT enough!");
                return false;
            }
            else
            {
                string[] preJsonData = { "bid", PlayerPrefs.GetString("Account"), auction.ToString() };
                string jsonData = JsonConvert.SerializeObject(preJsonData);
                string signature = await Web3Wallet.Sign(jsonData);
                string[] messageObj = { jsonData, signature };
                string message = JsonConvert.SerializeObject(messageObj);
                Debug.Log(message);
                // SendMessageRequest(message);
                return true;
            }
        }
        catch
        {
            Debug.Log("Signing error");
            return false;
        }
    }
}
