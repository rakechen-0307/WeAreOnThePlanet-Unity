using System;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    BackendCommunicator backendCommunicator;

    private void Awake()
    {
        backendCommunicator = GameObject.Find("BackendCommunicator").GetComponent<BackendCommunicator>();
    }

    public int loadMainPlayerData(int playerId)
    {
        PlayerData playerData = backendCommunicator.FindOnePlayerById(playerId);

        // load general data
        loadedData.playerId = playerData.Id;
        loadedData.playerName = playerData.Username;
        loadedData.experience = playerData.Exp;
        for (int i = 0; i < playerData.Friends.Count; i++)
        {
            loadedData.friendIds.Add(playerData.Friends[i].Id);
        }

        // load player data
        float posX = (float) playerData.Position.PosX;
        float posY = (float) playerData.Position.PosY;
        float posZ = (float) playerData.Position.PosZ;
        float rotX = (float) playerData.Position.RotX;
        float rotY = (float) playerData.Position.RotY;
        float rotZ = (float) playerData.Position.RotZ;
        Vector3 pos = new Vector3(posX, posY, posZ);
        Vector3 euler = new Vector3(rotX, rotY, rotZ);
        int planetId = playerData.Position.PlanetID;
        loadedData.mainPlayer = new Player(pos, euler, planetId);

        // achivements
        foreach (PlayerTask playerTask in playerData.TaskProgress)
        {
            int taskId= playerTask.Task.Id;
            string taskName = playerTask.Task.Name;
            float progress = (float) playerTask.Progress;
            float maxProgress = (float) playerTask.Task.MaxProgress;
            bool isCompleted = playerTask.Achieved;
            loadedData.achievements.Add(new Achievement(taskId, taskName, progress, maxProgress, isCompleted));  
        }

        // bid auctions
        foreach (PlayerBidAuction playerBidAuction in playerData.BidAuction)
        {
            int auctionID = playerBidAuction.Auction.Id;
            DateTimeOffset checkTime = playerBidAuction.CheckTime;
            loadedData.bidAuctions.Add(new SubscribedAuction(auctionID, checkTime));
        }

        return planetId;
    }
    public void loadPlanetData(int playerId)
    {
        PlayerData playerData = backendCommunicator.FindOnePlayerById(playerId);

        loadedData.currentPlanet.ownerId = playerId;
        loadedData.currentPlanet.experience = playerData.Exp;
        // loadNFTs
        foreach (NFTInfo nftInfo in playerData.NFTs)
        {
            ArtWork nft = new ArtWork();
            nft.id = nftInfo.Id;
            nft.artName = nftInfo.Name;
            nft.author = nftInfo.Author;
            nft.createdTime = nftInfo.CreateTime;
            nft.ownerID = nftInfo.Owner.Id;
            nft.isMinted = nftInfo.IsMinted;
            nft.isShown = nftInfo.IsShown;
            foreach (NFTContent nftContent in nftInfo.Contents)
            {
                float posX = (float) nftContent.PosX;
                float posY = (float) nftContent.PosY;
                float posZ = (float) nftContent.PosZ;
                float r = (float) nftContent.Color.R;
                float g = (float) nftContent.Color.G;
                float b = (float) nftContent.Color.B;
                
                Vector3 pos = new Vector3(posX, posY, posZ);
                Color color = new Color(r, g, b);
                nft.blockDatas.Add(new BlockData(pos, color));
            }
            loadedData.NFTs.Add(nft);
        }
    }
}
