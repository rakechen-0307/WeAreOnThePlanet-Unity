using UnityEngine;

public class MainPlanetSave : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;
    
    [SerializeField]
    private Transform mainPlayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SavePlayerData();
        }
    }

    private async void SavePlayerData()
    {
        Debug.Log("save player position");
        loadedData.mainPlayer.lastPosition = mainPlayer.position;
        loadedData.mainPlayer.lastEuler = mainPlayer.eulerAngles;
        bool success = await BackendManager.instance.saveMainPlayerData(loadedData.playerId, loadedData.mainPlayer);
    }
}
