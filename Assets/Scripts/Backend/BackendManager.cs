using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    public static BackendManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int loadMainPlayerData(int playerId, LoadedData loadedData)
    {
        PlayerData playerData = BackendCommunicator.instance.FindOnePlayerById(playerId);

        // load general data
        loadedData.playerId = playerData.Id;
        loadedData.playerName = playerData.Username;
        loadedData.experience = playerData.Exp;

        loadedData.friendIds = new List<int>();
        for (int i = 0; i < playerData.Friends.Count; i++)
        {
            loadedData.friendIds.Add(playerData.Friends[i].Id);
        }

        loadedData.pendingIds = new List<int>();
        for (int i = 0; i < playerData.Friends.Count; i++)
        {
            loadedData.pendingIds.Add(playerData.PendingFriends[i].Id);
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
        loadedData.achievements = new List<Achievement>();
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
        loadedData.bidAuctions = new List<SubscribedAuction>();
        foreach (PlayerBidAuction playerBidAuction in playerData.BidAuction)
        {
            int auctionID = playerBidAuction.Auction.Id;
            DateTimeOffset checkTime = playerBidAuction.CheckTime;
            loadedData.bidAuctions.Add(new SubscribedAuction(auctionID, checkTime));
        }

        return planetId;
    }
    public void saveMainPlayerData(int playerId, Player player)
    {
        Vector3 pos = player.lastPosition;
        Vector3 rot = player.lastEuler;
        int planetId = player.lastPlanetId;
        BackendCommunicator.instance.UpdatePlayerPosition(playerId, planetId, pos, rot);
    }

    public void loadPlanetData(int playerId, LoadedData loadedData)
    {
        PlayerData playerData = BackendCommunicator.instance.FindOnePlayerById(playerId);
        if (playerData == null)
        {
            Debug.LogError("didn't get player data");
            return;
        }

        loadedData.currentPlanet.experience = playerData.Exp;
        // loadNFTs
        loadedData.currentPlanet.NFTs = new List<ArtWork>();
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
            nft.isPending = nftInfo.IsPending;
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
            loadedData.currentPlanet.NFTs.Add(nft);
        }
    }

    public void loadOwnedNFT(int playerId, LoadedData loadedData)
    {
        IList<NFTInfo> nftInfos = BackendCommunicator.instance.GetNFTsById(playerId);

        loadedData.NFTs = new List<ArtWork>();
        foreach (NFTInfo nftInfo in nftInfos)
        {
            ArtWork nft = new ArtWork();
            nft.id = nftInfo.Id;
            nft.artName = nftInfo.Name;
            nft.author = nftInfo.Author;
            nft.createdTime = nftInfo.CreateTime;
            nft.ownerID = nftInfo.Owner.Id;
            nft.isMinted = nftInfo.IsMinted;
            nft.isShown = nftInfo.IsShown;
            nft.isPending = nftInfo.IsPending;
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

    public void saveNFT(ArtWork NFT)
    {
        int nftID = NFT.id;
        string NFTName = NFT.artName;
        bool isShown = NFT.isShown;
        bool isPending = NFT.isPending;
        List<BlockData> blockDatas = NFT.blockDatas;
        BackendCommunicator.instance.UpdateOneNFT(nftID, NFTName, isShown, blockDatas);
    }

    public async Task<int> newNFT(ArtWork NFT)
    {
        string NFTName = NFT.artName;
        int ownerId = NFT.ownerID;
        string author = NFT.author;
        DateTimeOffset createTime = NFT.createdTime;
        bool isMinted = NFT.isMinted;
        bool isShown = NFT.isShown;
        bool isPending = NFT.isPending;
        NFT.id = await BackendCommunicator.instance.CreateOneNFT(NFTName, ownerId, author, createTime, isMinted, isShown);
        return NFT.id;
    }
}
