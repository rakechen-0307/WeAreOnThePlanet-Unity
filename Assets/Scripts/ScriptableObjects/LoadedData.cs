using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadedData", menuName = "SaveData/LoadedData", order = 1)]
public class LoadedData : ScriptableObject
{
    public string playerName;
    public int playerId;
    public Player mainPlayer;
    public List<ArtWork> NFTs;
    public List<int> friendIds;
    public List<SubscribedAuction> bidAuctions;
    public List<Achievement> achievements;
    public int experience;
    public PlanetData currentPlanet;
}

[Serializable]
public class Player
{
    public Vector3 lastPosition;
    public Vector3 lastEuler;
    public int lastPlanetId;

    public Player(Vector3 LastPos, Vector3 LastEuler, int lastId)
    {
        lastPosition = LastPos;
        lastEuler = LastEuler;
        lastPlanetId = lastId;
    }
}

[Serializable]
public class Achievement
{
    public int id;
    public string name;
    public float progress;
    public float maxProgress;
    public bool isCompleted;

    public Achievement(int Id, string Name, float Progress, float MaxProgress, bool IsCompleted)
    {
        id = Id;
        name = Name;
        progress = Progress;
        maxProgress = MaxProgress;
        isCompleted = IsCompleted;
    }
}

[Serializable]
public class SubscribedAuction
{
    public int id;
    public DateTimeOffset checkTime;

    public SubscribedAuction(int Id, DateTimeOffset CheckTime)
    {
        id = Id;
        checkTime = CheckTime;
    }
}

[Serializable]
public class PlanetData
{
    public List<ArtWork> NFTs;
    public int experience;

    public PlanetData(int Id, List<ArtWork> nfts, int exp)
    {
        NFTs = nfts;
        experience = exp;
    }
}

[Serializable]
public class ArtWork
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
