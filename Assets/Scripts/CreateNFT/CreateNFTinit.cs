using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateNFTinit : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;
    
    [SerializeField]
    private GameObject mainMenu;
    
    [SerializeField]
    private GameObject colorMenu;

    [SerializeField]
    private GameObject brushMenu;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BackendManager.instance.loadOwnedNFT(loadedData.playerId, loadedData);

        mainMenu.SetActive(true);
        colorMenu.SetActive(false);
        brushMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
