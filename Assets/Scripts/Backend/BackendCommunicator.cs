using Realms.Sync;
using Realms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    /*
    public PlayerData CheckValid(string email)
    {

    }
    */

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
