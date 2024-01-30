using System.Collections.Generic;
using UnityEngine;

public class NFTDisplayer : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private Transform displayObjects;

    [SerializeField]
    private List<Transform> NFTDisplaySlots;

    [SerializeField]
    private Camera displayCamera;

    [SerializeField]
    private Vector2 centerPoint = new Vector2(0f, 0f);

    [SerializeField]
    private Vector2 size = new Vector2(0.5f, 0.5f);

    private Vector2 screenSize;

    private void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        Resize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DisplayNFTs(0);
        }
        if (screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            screenSize.x = Screen.width;
            screenSize.y = Screen.height;
            Resize();
        }
    }

    private void DisplayNFTs(int startIdx)
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
            Destroy(child);
        }

        foreach (BlockData blockData in NFT.blockDatas)
        {
            GameObject newBlock = Instantiate(blockPrefab, NFTtransform);
            newBlock.transform.localPosition = blockData.position;
            newBlock.GetComponent<Renderer>().material.color = blockData.color;
            // newBlock.layer = LayerMask.NameToLayer("UI");
        }
    }

    private void Resize()
    {
        Vector3 screenCenter = new Vector3(2 * displayCamera.orthographicSize * displayCamera.aspect * centerPoint.x, 2 * displayCamera.orthographicSize * centerPoint.y, 10f);
        displayObjects.localPosition = screenCenter;
        
        // float displaySizeX = 2 * displayCamera.orthographicSize * displayCamera.aspect * size.x;
        // float displaySizeY = 2 * displayCamera.orthographicSize * size.y;

        // Vector3 leftBottomPoint = screenCenter - new Vector3(0.1f * displaySizeX, 0.1f * displaySizeY, 0f);
        // for (int i = 0; i < NFTDisplaySlots.Count; i++)
        // {
        //     float xIdx = i % 3;
        //     float yIdx = 1 - i / 3;

        //     Vector3 newPos = leftBottomPoint + new Vector3(displaySizeX / 3 * xIdx, displaySizeY * yIdx, 0f);
        //     NFTDisplaySlots[i].localPosition = newPos;
        // }
    }
}
