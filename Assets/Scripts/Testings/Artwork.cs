using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ArtWork : ScriptableObject
{
    public List<BlockData> blockDatas;  
}

public class BlockData
{
    [field: SerializeField]
    private Vector3 position;
    [field: SerializeField]
    private Color color;
    public BlockData()
    {
        position = Vector3.zero;
        color = Color.white;
    }
    public BlockData(Vector3 Position, Color Color)
    {
        position = Position;
        color = Color;
    }
}
