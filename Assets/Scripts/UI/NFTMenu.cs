using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTMenu : MonoBehaviour
{
    [SerializeField]
    private CreateNFTSave createNFTSave;

    // root
    [SerializeField]
    private GameObject menuPannel;

    [SerializeField]
    private GameObject colorPannel;

    [SerializeField]
    private GameObject brushPannel;

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
    

    // root/menuPannel/buildModal
    [SerializeField]
    private TMP_InputField NFTNameInput;

    [SerializeField]
    private Button comfirmButton;

    [SerializeField]
    private Button cancelButton;

    private void Start()
    {
        showMainMenu();
        buildButton.onClick.AddListener(buildButtonOnClick);
        comfirmButton.onClick.AddListener(comfirmButtonOnClick);
        cancelButton.onClick.AddListener(cancelButtonOnClick);
    }

    private void buildButtonOnClick()
    {
        buildModal.SetActive(true);
    }

    private void comfirmButtonOnClick()
    {
        createNFTSave.NewNFT(NFTNameInput.text);
        showBuildMenu();
    }

    private void cancelButtonOnClick()
    {
        buildModal.SetActive(false);
    }



    private void showBuildMenu()
    {
        menuPannel.SetActive(false);
        buildModal.SetActive(false);
        colorPannel.SetActive(true);
        brushPannel.SetActive(true);
    }

    private void showMainMenu()
    {
        menuPannel.SetActive(true);
        buildModal.SetActive(false);
        colorPannel.SetActive(false);
        brushPannel.SetActive(false);
    }
}
