using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> blockDB = new List<GameObject>();
    
    [SerializeField]
    private ArtWork artWorkInProgress;

    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private ColorManager colorManager;
    
    public void PlaceBlock(Vector3 position)
    {
        bool blockTaken = false;

        foreach (var block in blockDB)
        {
            if (block.transform.position == position)
            {
                // blockDB.Remove(block);
                // Destroy(block);
                blockTaken = true;
                break;
            }
        }
        if (!blockTaken) 
        {
            // visualize
            GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity, transform);
            VisualizeBlock newBlockVisulize = newBlock.GetComponent<VisualizeBlock>();
            newBlockVisulize.placementSystem = this;
            newBlockVisulize.color = colorManager.getBrushColor();
            blockDB.Add(newBlock);
            // record
            BlockData newBlockData = new BlockData(position, colorManager.getBrushColor());
            artWorkInProgress.blockDatas.Add(newBlockData);
        }
    }
}
