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
    // Menus
    private void showBuildMenu()
    {
        menuPannel.SetActive(false);
        buildModal.SetActive(false);
        viewMenu.SetActive(false);
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
