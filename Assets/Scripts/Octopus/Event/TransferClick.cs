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
using UnityEngine.UIElements;

public class TransferClick : MonoBehaviour
{

    List<NFTInfo> Empty = new List<NFTInfo>
    {
        
    };

    public SushiManager sushiManager;

    async void OnMouseDown(){

        UIManager.Instance.UpdateDialog("none", "");
        UIManager.Instance.DeleteAllInputFields();

        // Load NFTs the player have
        bool success = sushiManager.PreviewMintedNFT("transfer"); // set to the result of DB

        // Show unminted NFTs
        if (!success)
        {
            sushiManager.SetTransfer(Empty);
            UIManager.Instance.UpdateDialog("none", "You don't have any minted NFTs!");
            
        }
        sushiManager.SetDisplayed(0, "transfer");

    }

}
