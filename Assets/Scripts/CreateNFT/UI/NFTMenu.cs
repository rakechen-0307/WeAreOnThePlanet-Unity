using System.Linq;
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

    // root/menuPannel/buildModal
    [SerializeField]
    private TMP_InputField NFTNameInput;

    [SerializeField]
    private Button comfirmButton;

    [SerializeField]
    private Button cancelButton;

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
        displayIndex = displayNFT(displayIndex);
    }

    private void viewBackButtonOnClick()
    {
        viewMenu.SetActive(false);
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
    }
    // Utility
    private int displayNFT(int displayIdx)
    {
        // display object
        nftDisplayer.DisplayNFTs(displayIdx);

        // display ui
        for (int i = 0; i < 6; i++)
        {
            if (displayIdx < loadedData.NFTs.Count)
            {
                NFTDisplay.GetChild(i).gameObject.SetActive(true);
                displayIdx++;
            }
            else
            {
                NFTDisplay.GetChild(i).gameObject.SetActive(false);
            }
        }
        return displayIdx;
    }
}
