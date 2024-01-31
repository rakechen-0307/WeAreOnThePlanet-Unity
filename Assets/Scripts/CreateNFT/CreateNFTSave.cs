using System;
using System.Threading.Tasks;
using UnityEngine;

public class CreateNFTSave : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    public async Task<ArtWork> NewNFT(string NFTName)
    {
        ArtWork NFT = new ArtWork();
        NFT.artName = NFTName;
        NFT.ownerID = loadedData.playerId;
        NFT.author = loadedData.playerName;
        NFT.createdTime = DateTimeOffset.Now;
        NFT.isMinted = false;
        NFT.isShown = false;
        NFT.isPending = false;
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
