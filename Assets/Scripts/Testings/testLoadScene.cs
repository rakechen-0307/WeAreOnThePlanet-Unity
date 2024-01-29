using UnityEngine;
using UnityEngine.SceneManagement;

public class testLoadScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainPlanet", LoadSceneMode.Single);
    }
}
