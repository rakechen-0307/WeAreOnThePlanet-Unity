using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    public ArtWork NFTinProgress;

    [SerializeField]
    private Button saveButton;

    [SerializeField]
    private Button leaveButton;

    [SerializeField]
    private GameObject leaveModal;

    [SerializeField]
    private Button leaveNoSaveButton;

    [SerializeField]
    private Button leaveSaveButton;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private NFTMenu nftMenu;

    [SerializeField]
    private CreateNFTSave createNFTSave;

    [SerializeField]
    private GameObject colorPannel;

    [SerializeField]
    private GameObject brushPannel;

    [SerializeField]
    private PlacementSystem placementSystem;

    public bool modified;

    private void Start()
    {
        saveButton.onClick.AddListener(saveNFT);
        leaveButton.onClick.AddListener(leaveButtonOnClick);
        leaveSaveButton.onClick.AddListener(leaveSaveButtonOnClick);
        leaveNoSaveButton.onClick.AddListener(leaveNoSaveButtonOnClick);
        cancelButton.onClick.AddListener(cancelButtonOnClick);
    }

    private void leaveButtonOnClick()
    {
        if (!modified)
        {
            leave();   
        }
        else
        {
            leaveModal.SetActive(true);
        }
    }

    private void leaveNoSaveButtonOnClick()
    {
        leave();
    }
    private void leaveSaveButtonOnClick()
    {
        saveNFT();
        leave();
    }
    private void cancelButtonOnClick()
    {
        leaveModal.SetActive(false);
    }

    private void leave()
    {
        placementSystem.clearVisulization();
        nftMenu.showMainMenu();
    }
    private void saveNFT()
    {
        createNFTSave.SaveNFTData(NFTinProgress);
        modified = false;
    }

    public void startBuilding(ArtWork NFT)
    {
        NFTinProgress = new ArtWork(NFT);
        colorPannel.SetActive(true);
        brushPannel.SetActive(true);
        leaveModal.SetActive(false);
        modified = false;
        placementSystem.loadVisulaization(NFT);
    }
}
