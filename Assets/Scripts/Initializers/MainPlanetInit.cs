using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainPlanetInit : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private Transform mainPlayer;

    [SerializeField]
    private List<Transform> NFTDisplaySlots = new List<Transform>();

    [SerializeField]
    private GameObject blockPrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        // TODO: call backend manager to load planet
        // TODO end
        initPlayer();
        initNFTDisplay();
        initGarden();
    }

    private void initPlayer()
    {
        mainPlayer.transform.position = loadedData.mainPlayer.lastPosition;
        mainPlayer.transform.eulerAngles = loadedData.mainPlayer.lastEuler;
    }
    private void initNFTDisplay()
    {
        int NFTidx = 0;
        int displayedNFTCount = 0;
        while (displayedNFTCount < NFTDisplaySlots.Count && NFTidx < loadedData.NFTs.Count)
        {
            if (loadedData.NFTs[NFTidx].isShown)
            {
                // TODO: show NFT
                ShowNFT(NFTDisplaySlots[displayedNFTCount], loadedData.NFTs[NFTidx]);
                displayedNFTCount++;
            }
            NFTidx++;
        }
    }
    private void initGarden()
    {

    }

    private void ShowNFT(Transform NFTtransform, ArtWork NFT)
    {
        Debug.Log("new");
        GameObject NFTDisplay = new GameObject(NFT.artName);
        NFTDisplay.transform.position = NFTtransform.position;
        NFTDisplay.transform.rotation = NFTtransform.rotation;
        NFTDisplay.transform.SetParent(NFTtransform);
        foreach (BlockData blockData in NFT.blockDatas)
        {
            GameObject newBlock = Instantiate(blockPrefab, NFTDisplay.transform);
            newBlock.transform.localPosition = blockData.position;
            newBlock.GetComponent<Renderer>().material.color = blockData.color;
        }
    }
}
