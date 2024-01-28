using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ArtWork : ScriptableObject
{
    public List<BlockData> blockDatas = new List<BlockData>();  
}

[Serializable]
public class BlockData
{
    [field: SerializeField]
    private Vector3 position = Vector3.zero;
    [field: SerializeField]
    private Color color = Color.black;
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
