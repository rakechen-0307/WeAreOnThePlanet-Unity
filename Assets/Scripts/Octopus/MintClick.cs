using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Core.Singletons;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Nethereum.Signer;
using Nethereum.Util;
using Web3Unity.Scripts.Library.Web3Wallet;
using System.Text;
using Realms;
using Realms.Sync;
using System.Linq;

public class MintClick : MonoBehaviour
{

    List<NFTInfo> Test = new List<NFTInfo>
    {
        new NFTInfo{
            Author = "1", CreateTime = DateTimeOffset.Now, IsMinted = false, Id = 1, Name="NFT1", Owner = null,
        },
        new NFTInfo{
            Author = "1", CreateTime = DateTimeOffset.Now, IsMinted = false, Id = 2, Name="NFT2", Owner = null,
        },
        new NFTInfo{
            Author = "1", CreateTime = DateTimeOffset.Now, IsMinted = false, Id = 3, Name="NFT3", Owner = null,
        },
        new NFTInfo{
            Author = "1", CreateTime = DateTimeOffset.Now, IsMinted = false, Id = 4, Name="NFT4", Owner = null,
        },
        new NFTInfo{
            Author = "1", CreateTime = DateTimeOffset.Now, IsMinted = false, Id = 5, Name="NFT5", Owner = null,
        },
    };

    public SushiManager sushiManager;

    void OnMouseDown(){

        Debug.Log("Mint NFT clicked");

        // Load NFTs the player have
        // StartCoroutine(LoadMintNFTOperation());
        sushiManager.SetMint(Test); // set to the result of DB

        // Show unminted NFTs
        sushiManager.SetDisplayed(0, "mint");

    }

    IEnumerator LoadMintNFTOperation()
    {
        // Asynchronous operation
        bool isOperationComplete = false;
        PlayerData playerData = null;

        // Start the async operation here
        YourAsyncOperation((result) => {
            // This is a callback once the async operation is done
            result = null;
            isOperationComplete = true;

            // Handle the result
            // ...

        });

        // Wait until the async operation is complete
        yield return new WaitUntil(() => isOperationComplete);

        // Continue with other operations after the async call
        // ...
    }

    private Realm _realm;

    async void YourAsyncOperation(System.Action<PlayerData> onCompleted)
    {
        /*
        // subscription
        var playerQuery = _realm.All<PlayerData>();
        await playerQuery.SubscribeAsync();

        PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == "aaa@gmail.com").FirstOrDefault();
        if (findPlayer == null)
        {
            Debug.LogError("Player not found.");
            onCompleted?.Invoke(null);
            return;
        }
        if (findPlayer.NFTs == null || findPlayer.NFTs.Count == 0)
        {
            Debug.LogError("Player has no NFTs.");
            onCompleted?.Invoke(null);
            return;
        }
        Debug.Log(findPlayer.NFTs[0].Author);
        */

        // Once done, call the callback
        onCompleted?.Invoke(null);
    }

}
