using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artwork", menuName = "SaveData/ArtWork", order = 1)]
public class ArtWork : ScriptableObject
{
    public int id;
    public string artName;
    public string author;
    public DateTimeOffset createdTime;
    public int ownerID;
    public bool isMinted;
    public bool isShown;
    public List<BlockData> blockDatas = new List<BlockData>();
}   

[Serializable]
public class BlockData
{
    public Vector3 position = Vector3.zero;
    public Color color = Color.black;
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
