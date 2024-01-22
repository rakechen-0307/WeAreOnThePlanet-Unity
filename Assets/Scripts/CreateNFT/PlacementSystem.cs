using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> blockDB = new List<GameObject>();
    
    [SerializeField]
    private ArtWork artWorkInProgress;

    [SerializeField]
    private GameObject blockPrefab;
    
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
            newBlock.GetComponent<VisualizeBlock>().placementSystem = this;
            blockDB.Add(newBlock);
            // record
            // BlockData newBlockData = new BlockData(position, Color.white);
            // artWorkInProgress.blockDatas.Add(newBlockData);
        }
    }
}
