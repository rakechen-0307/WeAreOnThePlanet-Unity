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

    [SerializeField]
    private ColorManager colorManager;
    
    [SerializeField]
    private BrushTypeManager brushTypeManager;
    
    public void ClickBlock(Vector3 position, Vector3 AddDisplacement = default(Vector3))
    {
        BrushType brushType = brushTypeManager.getBrushType();
        switch (brushType)
        {
            case BrushType.Remove:
                RemoveBlock(position);
                break;
            case BrushType.Add:
            default:
                PlaceBlock(position + AddDisplacement);
                break;
        }
    }
    private void PlaceBlock(Vector3 position)
    {
        bool blockTaken = false;

        foreach (var block in blockDB)
        {
            if (block.transform.position == position)
            {
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
    private void RemoveBlock(Vector3 position)
    {
        bool blockTaken = false;
        int removeIndex = 0;

        for (int i = 0; i < blockDB.Count; i++)
        {
            if (blockDB[i].transform.position == position)
            {
                removeIndex = i;
                blockTaken = true;
                break;
            }
        }
        if (blockTaken)
        {
            GameObject blockToRemove = blockDB[removeIndex];
            blockDB.RemoveAt(removeIndex);
            artWorkInProgress.blockDatas.RemoveAt(removeIndex);
            Destroy(blockToRemove);
        }
    }
}
