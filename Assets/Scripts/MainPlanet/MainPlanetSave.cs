using UnityEngine;

public class MainPlanetSave : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("save");
        }
    }

    private void SavePlayerData()
    {
        loadedData.mainPlayer.lastPosition = transform.position;
        loadedData.mainPlayer.lastEuler = transform.eulerAngles;
        loadedData.mainPlayer.lastPlanetId = loadedData.currentPlanet.ownerId;
        BackendManager.instance.saveMainPlayerData(loadedData.playerId, loadedData.mainPlayer);
    }
}
