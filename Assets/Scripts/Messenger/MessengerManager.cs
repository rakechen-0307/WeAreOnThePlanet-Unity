using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using Unity.Services.Core;
using TMPro;
using UnityEngine.UI;
using Realms;
using Realms.Sync;
using System.Linq;

public class MessengerManager : MonoBehaviour
{
    private IList<MessageObject> _message = new List<MessageObject>();
    private IList<FriendListObject> _friend = new List<FriendListObject>();

    public GameObject friendListObj, myTextObj, comingTextObj, channelObj, chatRoomObj;
    public Button sendButton;
    public TMP_InputField textInput;

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-hhbzr";

    private string _currentChannel;

    // Start is called before the first frame update
    async void Start()
    {
        var _email = "rakechen168@gmail.com";

        RealmSetup();

        // subscription
        var playerQuery = _realm.All<PlayerData>();
        await playerQuery.SubscribeAsync();

        // Check if user is already existed and store user information
        PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == _email).FirstOrDefault();

        if (findPlayer != null)
        {
            var _userID = findPlayer.Id;
            var friends = findPlayer.Friends.ToArray();
            Debug.Log(friends.Length);
            for (int i = 0; i < friends.Length; i++)
            {
                AddFriend(friends[i].Username, _userID, friends[i].Id);
                Debug.Log("add friend");
            }
        }

        // subscribe to receive coming channel message
        VivoxService.Instance.ChannelMessageReceived += ChannelMessageReceived;

        sendButton.onClick.AddListener(() =>
        {
            if (textInput.text != string.Empty)
            {
                ChannelSendMessageAsync(_currentChannel, textInput.text);
                AddMessage(textInput.text, true);
                textInput.text = string.Empty;
            }
        });
    }

    private void AddFriend(string friendName, int currentUserID, int friendID)
    {
        var newFriend = Instantiate(friendListObj, channelObj.transform);
        var newMessageFriendObj = newFriend.GetComponent<FriendListObject>();

        newMessageFriendObj.SelectButton.onClick.AddListener(() =>
        {
            ChannelSwitch(currentUserID, friendID);
        });

        newMessageFriendObj.Username.text = friendName;
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

    public async void ChannelSwitch(int from, int to)
    {
        await VivoxService.Instance.LeaveAllChannelsAsync();

        string channelToJoin = (from < to) ? (from.ToString() + "_" + to.ToString()) : (to.ToString() + "_" + from.ToString());

        if (channelToJoin != _currentChannel)
        {
            JoinChannelAsync(channelToJoin);
            _currentChannel = channelToJoin;
        }
        Debug.Log(channelToJoin);
    }

    public async void JoinChannelAsync(string channelName)
    {
        await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.TextOnly);
    }

    public async void ChannelSendMessageAsync(string channelName, string message)
    {
        await VivoxService.Instance.SendChannelTextMessageAsync(channelName, message);
    }

    private void AddMessage(string message, bool isFromSelf)
    {
        if (isFromSelf)
        {
            var newMes = Instantiate(myTextObj, chatRoomObj.transform);
            var newMessageTextObj = newMes.GetComponent<MessageObject>();

            newMessageTextObj.Message.text = message;

            _message.Add(newMessageTextObj);
        }
        else
        {
            var newMes = Instantiate(comingTextObj, chatRoomObj.transform);
            var newMessageTextObj = newMes.GetComponent<MessageObject>();

            newMessageTextObj.Message.text = message;

            _message.Add(newMessageTextObj);
        }
    }

    public void ChannelMessageReceived(VivoxMessage message)
    {
        var channelName = message.ChannelName;
        var senderName = message.SenderDisplayName;
        var senderId = message.SenderPlayerId;
        var messageText = message.MessageText;
        var fromSelf = message.FromSelf;

        if (!fromSelf)
        {
            AddMessage(messageText, false);
        }
    }
}

