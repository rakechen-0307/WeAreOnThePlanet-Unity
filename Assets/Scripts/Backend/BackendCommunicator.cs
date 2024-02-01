using Realms.Sync;
using Realms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Serializers;
using UnityEngine.SceneManagement;

public class BackendCommunicator : MonoBehaviour
{
    public static BackendCommunicator instance;
    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-wkrim";

    [SerializeField] private float checkDelaySecond = 100f;
    private float lastSavedSeconds = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            RealmSetup();
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log(_realm);
        //SceneManager.LoadScene("octopus");// Delete this
    }

    public IList<PlayerData> FindAllPlayers()
    {
        IList<PlayerData> players = _realm.All<PlayerData>().ToList();
        return players;
    }

    public PlayerData FindOnePlayerByEmail(string email)
    {
        PlayerData playerData = _realm.All<PlayerData>().Where(user => user.Email == email).FirstOrDefault();
        return playerData;
    }

    public PlayerData FindOnePlayerById(int playerId)
    {
        PlayerData playerData = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault();
        return playerData;
    }

    public PlayerData FindOnePlayerByAccount(string account)
    {
        PlayerData playerData = _realm.All<PlayerData>().Where(user => user.Account == account).FirstOrDefault();
        return playerData;
    }

    public async Task<int> CreateOnePlayer(string email, string username, string password, string account)
    {
        int playerCount = _realm.All<PlayerData>().ToArray().Length;
        List<Task> tasks = _realm.All<Task>().ToList();

        await _realm.WriteAsync(() =>
        {
            PlayerData newPlayer = _realm.Add(new PlayerData()
            {
                Id = playerCount + 1,
                Email = email,
                Username = username,
                Password = password,
                Account = account,
                Exp = 0,
                Position = new PlayerPosition()
                {
                    PlanetID = playerCount + 1,
                    PosX = 0,
                    PosY = 50.7,
                    PosZ = 0,
                    RotX = 0,
                    RotY = 0,
                    RotZ = 0
                }
            });

            for (int i = 0; i < tasks.Count; i++)
            {
                newPlayer.TaskProgress.Add(new PlayerTask
                {
                    Task = tasks[i],
                    Progress = 0,
                    Achieved = false
                });
            }
        });

        return playerCount + 1;
    }

    public async Task<bool> UpdatePlayerPosition(int playerId, int planetId, Vector3 pos, Vector3 rot)
    {
        PlayerData player = FindOnePlayerById(playerId);

        await _realm.WriteAsync(() =>
        {
            player.Position.PlanetID = planetId;
            player.Position.PosX = (double)pos.x;
            player.Position.PosY = (double)pos.y;
            player.Position.PosZ = (double)pos.z;
            player.Position.RotX = (double)rot.x;
            player.Position.RotY = (double)rot.y;
            player.Position.RotZ = (double)rot.z;
        });

        return true;
    }

    public IList<PlayerData> FindPlanetUsers(int planetId)
    {
        IList<PlayerData> players = _realm.All<PlayerData>().ToList();
        IList<PlayerData> planetUsers = new List<PlayerData>();

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Position.PlanetID == planetId)
            {
                planetUsers.Add(players[i]);
            }
        }
        return planetUsers;
    }

    public IList<NFTInfo> GetNFTsById(int playerId)
    {
        try
        {
            IList<NFTInfo> nftInfos = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault().NFTs;
            return nftInfos;
        }
        catch (System.Exception)
        {
            Debug.LogError("failed to get NFT");
            throw;
        }
    }
    public NFTInfo FindOneNFTById(int NFTId)
    {
        NFTInfo nftData = _realm.All<NFTInfo>().Where(nft => nft.Id == NFTId).FirstOrDefault();
        return nftData;
    }

    public async Task<bool> UpdateOneNFT(int nftId, string name, bool isShown, bool isPending, List<BlockData> blockData)
    {
        NFTInfo updateNFT = _realm.All<NFTInfo>().Where(nft => nft.Id == nftId).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            updateNFT.Name = name;
            updateNFT.IsShown = isShown;
            updateNFT.IsPending = isPending;

            while (updateNFT.Contents.Count > 0)
            {
                _realm.Remove(updateNFT.Contents[0]);
            }

            for (int i = 0; i < blockData.Count; i++)
            {
                Vector3 pos = blockData[i].position;
                Color color = blockData[i].color;
                RGBColor rgbColor = new RGBColor()
                {
                    R = color.r,
                    G = color.g,
                    B = color.b,
                };
                NFTContent nftContent = new NFTContent()
                {
                    PosX = pos.x,
                    PosY = pos.y,
                    PosZ = pos.z,
                    Color = rgbColor
                };
                updateNFT.Contents.Add(nftContent);
            }
        });

        return true;
    }

    public async void UpdateNFTStatus(int nftId, bool status)
    {
        NFTInfo updateNFT = _realm.All<NFTInfo>().Where(nft => nft.Id == nftId).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            updateNFT.IsPending = status;
        });
    }
    public async void UpdateNFTMintStatus(int nftId, bool status)
    {
        NFTInfo updateNFT = _realm.All<NFTInfo>().Where(nft => nft.Id == nftId).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            updateNFT.IsMinted = status;
        });
    }
    public async void UpdateNFTOwner(int nftId, string email)
    {
        NFTInfo updateNFT = _realm.All<NFTInfo>().Where(nft => nft.Id == nftId).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            updateNFT.Owner = FindOnePlayerByEmail(email);
        });
    }

    public IList<PlayerData> FindAllFriends(int playerId)
    {
        IList<PlayerData> friends = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault().Friends;
        return friends;
    }


    public IList<Auction> FindEndedAuctionsByEmail(string email)
    {
        IList<Auction> auctions = _realm.All<Auction>().Where(auction => auction.Owner.Email == email && auction.EndTime < DateTimeOffset.UtcNow && auction.NFT.Owner.Email == email).ToList();
        return auctions;
    }
    public List<Auction> FindActiveAuctions()
    {
        DateTimeOffset now = System.DateTime.UtcNow;
        List<Auction> activeAuctions = _realm.All<Auction>().Where(auction => (
            auction.EndTime >= now && auction.StartTime < now
        )).ToList();
        return activeAuctions;
    }
    public Auction FindAuctionByNFTId(int NFTId)
    {
        NFTInfo NFT = _realm.All<NFTInfo>().Where(nft => nft.Id == NFTId).FirstOrDefault();
        Auction auction = _realm.All<Auction>().Where(auction => auction.NFT == NFT).FirstOrDefault();
        return auction;
    }
    public async Task<int> CreateAuction(NFTInfo nft, int startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        try
        {
            PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == PlayerPrefs.GetString("Email")).FirstOrDefault();
            var auctionsCount = _realm.All<Auction>().ToArray().Length;
            await _realm.WriteAsync(() =>
            {
                var auction = _realm.Add(new Auction()
                {
                    Id = auctionsCount + 1,
                    Owner = findPlayer,
                    NFT = nft,
                    StartTime = startTime,
                    EndTime = endTime,
                    StartPrice = startPrice,
                    BidPrice = startPrice
                });
            });
            return auctionsCount + 1;
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
            return -1;
        }
    }
    public async void Bid(int id, int bidPrice)
    {
        Auction auction = FindAuctionByNFTId(id);
        PlayerData player = _realm.All<PlayerData>().Where(user => user.Email == PlayerPrefs.GetString("Email")).FirstOrDefault();
        await _realm.WriteAsync(() =>
        {
            auction.BidPlayer = player;
            auction.BidPrice = bidPrice;
            Debug.Log(player + bidPrice.ToString());
        });
    }
    public IList<PendingFreiendInfo> FindAllPendingFriends(int playerId)
    {
        IList<PendingFreiendInfo> pendings = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault().PendingFriends;
        return pendings;
    }

    public async Task<bool> AddPendingFriend(int from, int to)
    {
        PlayerData sender = _realm.All<PlayerData>().Where(user => user.Id == from).FirstOrDefault();
        PlayerData receiver = _realm.All<PlayerData>().Where(user => user.Id == to).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            sender.PendingFriends.Add(new PendingFreiendInfo()
            {
                Player = receiver,
                IsSender = true
            });

            receiver.PendingFriends.Add(new PendingFreiendInfo()
            {
                Player = sender,
                IsSender = false
            });
        });

        return true;
    }

    public async Task<bool> AcceptFriend(int from, int to)
    {
        PlayerData sender = _realm.All<PlayerData>().Where(user => user.Id == from).FirstOrDefault();
        PlayerData receiver = _realm.All<PlayerData>().Where(user => user.Id == to).FirstOrDefault();

        IList<PendingFreiendInfo> senderPendings = sender.PendingFriends;
        IList<PendingFreiendInfo> receiverPendings = receiver.PendingFriends;

        PendingFreiendInfo senderPending = null;
        PendingFreiendInfo receiverPending = null;

        for (int i = 0; i < senderPendings.Count; i++)
        {
            if (senderPendings[i].Player.Id == receiver.Id)
            {
                senderPending = senderPendings[i];
            }
        }
        
        for (int i = 0; i < receiverPendings.Count; i++)
        {
            if (receiverPendings[i].Player.Id == sender.Id)
            {
                receiverPending = receiverPendings[i];
            }
        }

        if (senderPending == null || receiverPending == null)
        {
            Debug.Log("pending not found");
            return false;
        }

        await _realm.WriteAsync(() =>
        {
            sender.PendingFriends.Remove(senderPending);
            receiver.PendingFriends.Remove(receiverPending);
            sender.Friends.Add(receiver);
            receiver.Friends.Add(sender);
        });

        return true;
    }

    public async Task<bool> DenyFriend(int from, int to)
    {
        PlayerData sender = _realm.All<PlayerData>().Where(user => user.Id == from).FirstOrDefault();
        PlayerData receiver = _realm.All<PlayerData>().Where(user => user.Id == to).FirstOrDefault();

        IList<PendingFreiendInfo> senderPendings = sender.PendingFriends;
        IList<PendingFreiendInfo> receiverPendings = receiver.PendingFriends;

        PendingFreiendInfo senderPending = null;
        PendingFreiendInfo receiverPending = null;

        for (int i = 0; i < senderPendings.Count; i++)
        {
            if (senderPendings[i].Player.Id == receiver.Id)
            {
                senderPending = senderPendings[i];
            }
        }

        for (int i = 0; i < receiverPendings.Count; i++)
        {
            if (receiverPendings[i].Player.Id == sender.Id)
            {
                receiverPending = receiverPendings[i];
            }
        }

        if (senderPending == null || receiverPending == null)
        {
            Debug.Log("pending not found");
            return false;
        }

        await _realm.WriteAsync(() =>
        {
            sender.PendingFriends.Remove(senderPending);
            receiver.PendingFriends.Remove(receiverPending);
        });

        return true;
    } 

    public async Task<int> CreateOneNFT(string name, int ownerId, string author, DateTimeOffset createTime, bool isMinted, bool isShown, bool isPending)
    {
        int NFTsCount = _realm.All<NFTInfo>().ToArray().Length;
        PlayerData owner = _realm.All<PlayerData>().Where(user => user.Id == ownerId).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            NFTInfo newNFT = _realm.Add(new NFTInfo()
            {
                Id = NFTsCount + 1,
                Owner = owner,
                Name = name,
                Author = author,
                CreateTime = createTime,
                IsMinted = isMinted,
                IsShown = isShown,
                IsPending = isPending
            });

            owner.NFTs.Add(newNFT);
        });

        return NFTsCount + 1;
    }

    public void ChangeNFTOwner(string initOwnerEmail, string newOwnerEmail, int NFTId)
    {
        FindOnePlayerByEmail(newOwnerEmail).NFTs.Add(FindOneNFTById(NFTId));
        FindOnePlayerByEmail(newOwnerEmail).NFTs.Remove(FindOneNFTById(NFTId));
    }

    public IList<Auction> FindHeldAuctionByPlayerID(int playerId)
    {
        PlayerData player = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault();
        IList<Auction> heldAuctions = _realm.All<Auction>().Where(auction => auction.Owner == player).ToList();
        return heldAuctions;
    }

    public async Task<bool> FlowerAddExp(int playerId, int addedExp)
    {
        PlayerData player = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            player.Exp += addedExp;
        });

        return true;
    }

    public async Task<int> ProgressUpdate(int playerId, int taskId)
    {
        bool _isAwarded = false;
        PlayerData player = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault();

        if (!player.TaskProgress[taskId].Achieved)
        {
            await _realm.WriteAsync(() =>
            {
                player.TaskProgress[taskId].Progress += 1;
            });

            if (player.TaskProgress[taskId].Task.MaxProgress == player.TaskProgress[taskId].Progress)
            {
                await _realm.WriteAsync(() =>
                {
                    player.TaskProgress[taskId].Achieved = true;
                });
            }

            if (player.TaskProgress[taskId].Achieved)
            {
                await _realm.WriteAsync(() =>
                {
                    player.Exp += player.TaskProgress[taskId].Task.Exp;
                });
                _isAwarded = true;
            }
        }
        if (_isAwarded)
        {
            return player.TaskProgress[taskId].Task.Prize;
        }
        else
        {
            return -1;
        }
    }

    private async void RealmSetup()
    {
        if (_realm == null)
        {
            _realmApp = App.Create(new AppConfiguration(_realmAppID));
            if (_realmApp.CurrentUser == null)
            {
                _realmUser = await _realmApp.LogInAsync(Credentials.Anonymous());
                Debug.Log("user created");
                _realm = await Realm.GetInstanceAsync(new FlexibleSyncConfiguration(_realmUser));
            }
            else
            {
                _realmUser = _realmApp.CurrentUser;
                Debug.Log("user remain");
                _realm = Realm.GetInstance(new FlexibleSyncConfiguration(_realmUser));
            }
        }

        // Subscription
        var playerQuery = _realm.All<PlayerData>();
        await playerQuery.SubscribeAsync();

        var NFTQuery = _realm.All<NFTInfo>();
        await NFTQuery.SubscribeAsync();

        var auctionQuery = _realm.All<Auction>();
        await auctionQuery.SubscribeAsync();

        var taskQuery = _realm.All<Task>();
        await taskQuery.SubscribeAsync();
    }

    private void Start()
    {
        lastSavedSeconds = Time.time;
    }
    
    private async void Update()
    {
        if(PlayerPrefs.HasKey("Email"))
        {
            float currentTimeSecond = Time.time;
            if (currentTimeSecond - lastSavedSeconds > checkDelaySecond)
            {
                // TODO: check or save
                Debug.Log("UPDATE!" + currentTimeSecond.ToString());
                bool success = await AuctionManager.instance.CheckFinishedAuction(PlayerPrefs.GetString("Email"));
                lastSavedSeconds = currentTimeSecond;
            }
        }
    }
}
