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

public class TransferClick : MonoBehaviour
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

        UIManager.Instance.UpdateDialog("none", "");

        // Load NFTs the player have
        sushiManager.SetTransfer(Test); // set to the result of DB

        // Show unminted NFTs
        sushiManager.SetDisplayed(0, "transfer");

    }

}
