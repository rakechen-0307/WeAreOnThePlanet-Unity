using System.Collections.Generic;
using UnityEngine;

public class NFTDisplayer : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private List<Transform> NFTDisplaySlots = new List<Transform>();

    public void showNFTFromIdx(int startIdx, string layer = "Default")
    {
        int NFTidx = startIdx;
        int displayedNFTCount = 0;
        while (displayedNFTCount < NFTDisplaySlots.Count && NFTidx < loadedData.NFTs.Count)
        {
            ShowOneNFT(NFTDisplaySlots[displayedNFTCount], loadedData.NFTs[NFTidx], layer);
            displayedNFTCount++;
            NFTidx++;
        }
    }

    public void ShowNFTList(List<ArtWork> NFTs, string layer = "Default")
    {
        int NFTidx = 0;
        int displayedNFTCount = 0;
        while (displayedNFTCount < NFTDisplaySlots.Count && NFTidx < NFTs.Count)
        {
            ShowOneNFT(NFTDisplaySlots[displayedNFTCount], NFTs[NFTidx], layer);
            displayedNFTCount++;
            NFTidx++;
        }
    }

    public void ShowOneNFT(Transform NFTtransform, ArtWork NFT, string layer = "Default")
    {
        foreach (Transform child in NFTtransform)
        {
            Destroy(child.gameObject);
        }
        foreach (BlockData blockData in NFT.blockDatas)
        {
            GameObject newBlock = Instantiate(blockPrefab, NFTtransform);
            newBlock.transform.localPosition = blockData.position;
            newBlock.GetComponent<Renderer>().material.color = blockData.color;
            newBlock.layer = LayerMask.NameToLayer(layer);
        }
    }
}
