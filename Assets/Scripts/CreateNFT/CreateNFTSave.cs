using System;
using UnityEngine;

public class CreateNFTSave : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveNFTData(loadedData.NFTs[0]);
        }
    }

    private void NewNFT(string NFTName)
    {
        ArtWork NFT = new ArtWork();
        NFT.artName = NFTName;
        NFT.ownerID = loadedData.playerId;
        NFT.author = loadedData.playerName;
        NFT.createdTime = DateTimeOffset.Now;
        NFT.isMinted = false;
        NFT.isShown = false;
        BackendManager.instance.newNFT(NFT);
    }

    private void SaveNFTData(ArtWork NFT)
    {
        BackendManager.instance.saveNFT(NFT);
    }
}
