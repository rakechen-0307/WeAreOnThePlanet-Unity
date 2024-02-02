using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPlanetTravel : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private Transform mainPlayer;

    public async void TravelPlanet(int planetId)
    {
        if (loadedData.mainPlayer.lastPlanetId == planetId)
        {
            return;
        }

        string sceneName = "MainPlanet";
        if (planetId == -1)  // NFT workshop
        {
            sceneName = "CreateNFT";
            loadedData.mainPlayer.lastPosition = mainPlayer.transform.position;
            loadedData.mainPlayer.lastEuler = mainPlayer.transform.eulerAngles;
        }
        else if (planetId == -2)  // Octopus
        {
            sceneName = "Octopus";
            loadedData.mainPlayer.lastPosition = mainPlayer.transform.position;
            loadedData.mainPlayer.lastEuler = mainPlayer.transform.eulerAngles;
        }
        else
        {
            loadedData.mainPlayer.lastPosition = new Vector3(0f, 52f, 0f);
            loadedData.mainPlayer.lastEuler = new Vector3(0, 0, 0);
            await BackendCommunicator.instance.UpdatePlayerPosition(loadedData.playerId, planetId, loadedData.mainPlayer.lastPosition, loadedData.mainPlayer.lastEuler);
        }
        loadedData.mainPlayer.lastPlanetId = planetId;
        
        SceneManager.LoadScene(sceneName);
    }
}
