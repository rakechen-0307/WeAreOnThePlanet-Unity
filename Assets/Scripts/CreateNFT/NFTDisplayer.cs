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

    public void DisplayNFTs(int startIdx)
    {
        int NFTidx = startIdx;
        int displayedNFTCount = 0;
        while (displayedNFTCount < NFTDisplaySlots.Count && NFTidx < loadedData.NFTs.Count)
        {
            ShowNFT(NFTDisplaySlots[displayedNFTCount], loadedData.NFTs[NFTidx]);
            displayedNFTCount++;
        }
    }

    private void ShowNFT(Transform NFTtransform, ArtWork NFT)
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
            newBlock.layer = LayerMask.NameToLayer("RenderTexture");
        }
    }
}
