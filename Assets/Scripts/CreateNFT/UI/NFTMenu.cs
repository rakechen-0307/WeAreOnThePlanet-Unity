using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTMenu : MonoBehaviour
{
    [SerializeField]
    private CreateNFTSave createNFTSave;

    [SerializeField]
    private NFTDisplayer nftDisplayer;

    [SerializeField]
    private LoadedData loadedData;

    // root
    [SerializeField]
    private GameObject menuPannel;

    [SerializeField]
    private GameObject buildPannel;

    // root/menuPannel
    [SerializeField]
    private Button buildButton;

    [SerializeField]
    private Button viewButton;

    [SerializeField]
    private GameObject buildModal;

    [SerializeField]
    private GameObject viewMenu;
    // root/menuPannel/viewMenu
    
    [SerializeField]
    private Button viewBackButton;

    [SerializeField]
    private Transform NFTDisplay;

    [SerializeField]
    private List<TMP_Text> NFTNames;

    // root/menuPannel/buildModal
    [SerializeField]
    private TMP_InputField NFTNameInput;

    [SerializeField]
    private Button comfirmButton;

    [SerializeField]
    private Button cancelButton;

    // root/menuPannel/viewMenu
    [SerializeField]
    private Button nextPage;

    [SerializeField]
    private Button previousPage;

    [SerializeField]
    private GameObject NFTDataModal;

    // NFTModal
    [SerializeField]
    private List<RenderTexture> NFTTextures;

    [SerializeField]
    private RawImage displayImage;

    [SerializeField]
    private TMP_InputField changeNameInput;

    [SerializeField]
    private Toggle isShownToggle;

    [SerializeField]
    private Toggle isMintedToggle;

    [SerializeField]
    private TMP_Text authorName;

    [SerializeField]
    private TMP_Text ownerId;

    [SerializeField]
    private Button saveNFTInfo;

    [SerializeField]
    private Button leaveNFTInfo;

    [SerializeField]
    private Button editNFTButton;

    private ArtWork currentViewingNFT;

    // Other
    [SerializeField]
    private SaveManager saveManager;

    private int displayIndex = 0;

    private void Start()
    {
        showMainMenu();
        buildButton.onClick.AddListener(buildButtonOnClick);
        comfirmButton.onClick.AddListener(comfirmButtonOnClick);
        cancelButton.onClick.AddListener(cancelButtonOnClick);
        viewButton.onClick.AddListener(viewButtonOnClick);
        viewBackButton.onClick.AddListener(viewBackButtonOnClick);
        nextPage.onClick.AddListener(nextPageOnClick);
        previousPage.onClick.AddListener(previousPageOnClick);
        for (int i = 0; i < NFTDisplay.childCount; i++)
        {
            Button nftSlotButton = NFTDisplay.GetChild(i).GetComponent<Button>();
            int offset = i;
            nftSlotButton.onClick.AddListener(() => NFTSlotOnClick(offset));
        }
        saveNFTInfo.onClick.AddListener(saveNFTInfoOnClick);
        leaveNFTInfo.onClick.AddListener(leaveNFTInfoOnClick);
        editNFTButton.onClick.AddListener(editNFTButtonOnClick);
    }

    private void buildButtonOnClick()
    {
        buildModal.SetActive(true);
    }

    private async void comfirmButtonOnClick()
    {
        ArtWork newNFT = await createNFTSave.NewNFT(NFTNameInput.text);
        showBuildMenu();
        saveManager.startBuilding(newNFT);
    }

    private void cancelButtonOnClick()
    {
        buildModal.SetActive(false);
    }

    private void viewButtonOnClick()
    {
        viewMenu.SetActive(true);
        NFTDataModal.SetActive(false);
        displayNFT(displayIndex);
    }

    private void viewBackButtonOnClick()
    {
        viewMenu.SetActive(false);
    }

    private void previousPageOnClick()
    {
        if (displayIndex - 6 >= 0)
        {
            displayIndex -= 6;
            displayNFT(displayIndex);
        }
    }

    private void nextPageOnClick()
    {
        if (displayIndex + 6 < loadedData.NFTs.Count)
        {
            displayIndex += 6;
            displayNFT(displayIndex);
        }
    }
    // NFT Modal
    private void NFTSlotOnClick(int offset)
    {
        int index = displayIndex + offset;
        if (index >= loadedData.NFTs.Count)
        {
            Debug.LogError("Warning: NFT index out of range!, something is wrong with displayIndex or the loadData");
            return;
        }
        ArtWork NFT = loadedData.NFTs[index];

        NFTDataModal.SetActive(true);
        displayNFTModal(NFT, offset);
    }

    private void saveNFTInfoOnClick()
    {
        bool NFTNameChanged = changeNameInput.text != currentViewingNFT.artName;
        bool NFTShowChanged = isShownToggle.isOn != currentViewingNFT.isShown;
        if (NFTNameChanged || NFTShowChanged)
        {
            currentViewingNFT.artName = changeNameInput.text;
            currentViewingNFT.isShown = isShownToggle.isOn;
            createNFTSave.SaveNFTData(currentViewingNFT);
        }
    }

    private void leaveNFTInfoOnClick()
    {
        NFTDataModal.SetActive(false);
        displayNFT(displayIndex);
    }

    private void editNFTButtonOnClick()
    {

    }
    // Menus
    private void showBuildMenu()
    {
        menuPannel.SetActive(false);
        buildModal.SetActive(false);
        viewMenu.SetActive(false);
        NFTDataModal.SetActive(false);
        buildPannel.SetActive(true);
    }

    public void showMainMenu()
    {
        menuPannel.SetActive(true);
        buildModal.SetActive(false);
        viewMenu.SetActive(false);
        buildPannel.SetActive(false);
        displayIndex = 0;
    }
    // Utility
    private void displayNFTModal(ArtWork NFT, int offset)
    {
        displayImage.texture = NFTTextures[offset];
        changeNameInput.text = NFT.artName;
        isShownToggle.isOn = NFT.isShown;
        isMintedToggle.isOn = NFT.isMinted;
        authorName.text = NFT.author;
        ownerId.text = NFT.ownerID.ToString();

        currentViewingNFT = NFT;
    }
    private void displayNFT(int displayIdx)
    {
        // display object
        nftDisplayer.DisplayNFTs(displayIdx);
        int index = displayIdx;
        // display ui
        for (int i = 0; i < 6; i++)
        {
            if (index < loadedData.NFTs.Count)
            {
                NFTDisplay.GetChild(i).gameObject.SetActive(true);
                NFTNames[i].text = loadedData.NFTs[index].artName;
                index++;
            }
            else
            {
                NFTDisplay.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
