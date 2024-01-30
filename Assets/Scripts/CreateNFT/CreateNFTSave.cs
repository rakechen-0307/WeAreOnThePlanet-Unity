using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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

    public async Task<ArtWork> NewNFT(string NFTName)
    {
        ArtWork NFT = new ArtWork();
        NFT.artName = NFTName;
        NFT.ownerID = loadedData.playerId;
        NFT.author = loadedData.playerName;
        NFT.createdTime = DateTimeOffset.Now;
        NFT.isMinted = false;
        NFT.isShown = false;
        NFT.id = await BackendManager.instance.newNFT(NFT);
        loadedData.NFTs.Add(NFT);
        return NFT;
    }

    public void SaveNFTData(ArtWork newNFTData)
    {
        BackendManager.instance.saveNFT(newNFTData);

        for (int i = 0; i < loadedData.NFTs.Count; i++)
        {
            if (loadedData.NFTs[i].id == newNFTData.id)
            {
                loadedData.NFTs[i] = newNFTData;
            }
        }
    }
}
