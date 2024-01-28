using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Realms;
using Realms.Sync;
using System.Linq;

public class TextChatManager : MonoBehaviour
{
    private IList<MessageObject> _message = new List<MessageObject>();
    private IList<FriendListObject> _friend = new List<FriendListObject>();

    public GameObject friendListObj, myTextObj, comingTextObj, channelObj, chatRoomObj;
    public Button sendButton;
    public TMP_InputField textInput;

    private int count = 0;

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-hhbzr";

    // Start is called before the first frame update
    async void Start()
    {
        // show friend list
        var _email = "rakechen168@gmail.com";

        RealmSetup();

        // subscription
        var playerQuery = _realm.All<PlayerData>();
        await playerQuery.SubscribeAsync();

        // Check if user is already existed and store user information
        PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == _email).FirstOrDefault();
        
        if (findPlayer != null)
        {
            var friends = findPlayer.Friends.ToArray();
            Debug.Log(friends.Length);
            for (int i = 0; i<friends.Length; i++)
            {
                AddFriend(friends[i].Username);
                Debug.Log("add friend");
            }
        }

        sendButton.onClick.AddListener(() =>
        {
            if (textInput.text != null)
            {
                Debug.Log(textInput.text);
                AddMessage(textInput.text);
                count++;
            }
        });
    }

    private void AddMessage(string message)
    {
        if (count % 2 == 0)
        {
            var newMes = Instantiate(comingTextObj, chatRoomObj.transform);
            var newMessageTextObj = newMes.GetComponent<MessageObject>();

            newMessageTextObj.Message.text = message;

            _message.Add(newMessageTextObj);
        }
        else
        {
            var newMes = Instantiate(myTextObj, chatRoomObj.transform);
            var newMessageTextObj = newMes.GetComponent<MessageObject>();

            newMessageTextObj.Message.text = message;

            _message.Add(newMessageTextObj);
        }
    }

    private void AddFriend(string username)
    {
        var newFriend = Instantiate(friendListObj, channelObj.transform);
        var newMessageFriendObj = newFriend.GetComponent<FriendListObject>();

        newMessageFriendObj.Username.text = username;

        _friend.Add(newMessageFriendObj);
    }

    public async void RealmSetup()
    {
        Debug.Log(_realm == null);
        // setup Realm
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
    }
}
