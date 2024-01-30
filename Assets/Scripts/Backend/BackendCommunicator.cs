using Realms.Sync;
using Realms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;

public class BackendCommunicator : MonoBehaviour
{
    public static BackendCommunicator instance;
    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-ouawh";

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

    public async void UpdatePlayerPosition(int playerId, int planetId, Vector3 pos, Vector3 rot)
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
    }

    public IList<NFTInfo> GetNFTsById(int playerId)
    {
        IList<NFTInfo> nftInfos = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault().NFTs;
        return nftInfos;
    }

    public async void UpdateOneNFT(int nftId, string name, bool isShown, List<BlockData> blockData)
    {
        NFTInfo updateNFT = _realm.All<NFTInfo>().Where(nft => nft.Id == nftId).FirstOrDefault();

        await _realm.WriteAsync(() =>
        {
            updateNFT.Name = name;
            updateNFT.IsShown = isShown;

            while (updateNFT.Contents.Count >= 0)
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
    }

    public IList<PlayerData> FindAllFriends(int playerId)
    {
        IList<PlayerData> friends = _realm.All<PlayerData>().Where(user => user.Id == playerId).FirstOrDefault().Friends;
        return friends;
    }

    public async Task<int> CreateOneNFT(string name, int ownerId, string author, DateTimeOffset createTime, bool isMinted, bool isShown)
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
                IsShown = isShown
            });

            owner.NFTs.Add(newNFT);
        });

        return NFTsCount + 1;
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
}
