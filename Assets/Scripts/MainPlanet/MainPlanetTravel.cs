using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPlanetTravel : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;
    public void TravelPlanet(int planetId)
    {
        if (loadedData.mainPlayer.lastPlanetId == planetId)
        {
            return;
        }

        string sceneName = "MainPlanet";
        if (planetId == -1) // NFT workshop
        {
            sceneName = "CreateNFT";
        }
        else if (planetId == -2)
        {
            sceneName = "Sushi";
        }
        else
        {
            loadedData.mainPlayer.lastPosition = new Vector3(0f, 50.7f, 0f);
            loadedData.mainPlayer.lastEuler = new Vector3(0, 0, 0);
        }
        loadedData.mainPlayer.lastPlanetId = planetId;
        
        SceneManager.LoadScene(sceneName);
    }
}
