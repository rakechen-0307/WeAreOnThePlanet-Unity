using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateNFTinit : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;
    
    [SerializeField]
    private GameObject UIPannel;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UIPannel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BackendManager.instance.loadOwnedNFT(loadedData.playerId, loadedData);
        
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
