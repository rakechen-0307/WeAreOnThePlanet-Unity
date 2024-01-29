using UnityEngine;

public class CreateNFTSave : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveNFTData();
        }
    }

    private void SaveNFTData()
    {
        // loadedData.NFTs => back
    }
}
