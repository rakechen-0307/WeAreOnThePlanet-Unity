using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPlanetInit : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private Transform mainPlayer;

    [SerializeField]
    private NFTDisplayer nftDisplayer;

    [SerializeField]
    private GameObject blockPrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BackendManager.instance.loadPlanetData(loadedData.mainPlayer.lastPlanetId, loadedData);
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
        List<ArtWork> NFTsToShow = new List<ArtWork>();
        foreach (ArtWork NFT in loadedData.NFTs)
        {
            if (NFT.isShown)
            {
                NFTsToShow.Add(NFT);
            }
        }
        nftDisplayer.ShowNFTList(NFTsToShow);
    }
    private void initGarden()
    {

    }

    private void ShowNFT(Transform NFTtransform, ArtWork NFT)
    {
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
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
