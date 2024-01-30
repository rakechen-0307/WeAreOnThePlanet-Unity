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
    public GameObject MessengerUI;
    public Button ChatButton;
    public Button AddFriendButton;
    public Button PendingButton;
    public GameObject ChatPage;
    public GameObject AddFriendPage;
    public GameObject PendingPage;

    public GameObject playerListObj, chatChannelObj, addFriendChannelObj;
    public LoadedData _loadedData;

    private bool _messengerIsOpened;
    private string _currentChannel;

    private IList<PlayerListObject> _player = new List<PlayerListObject>();

    private void Awake()
    {
        MessengerUI.SetActive(false);
        _messengerIsOpened = false;
    }

    void Start()
    {
        ChatButton.onClick.AddListener(() =>
        {
            ChatPage.SetActive(true);
            AddFriendPage.SetActive(false);
            PendingPage.SetActive(false);
            ShowFriendList(_loadedData.playerId);
        });

        AddFriendButton.onClick.AddListener(() =>
        {
            AddFriendPage.SetActive(true);
            ChatPage.SetActive(false);
            PendingPage.SetActive(false);
            ShowNotFriendList(_loadedData.playerId);
        });

        PendingButton.onClick.AddListener(() =>
        {
            PendingPage.SetActive(true);
            ChatPage.SetActive(false);
            AddFriendPage.SetActive(false);
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) 
        {
            if (_messengerIsOpened)
            {
                Debug.Log("Messenger Closed");
                MessengerUI.SetActive(false);
                _messengerIsOpened = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Debug.Log("Messenger Opened");
                MessengerUI.SetActive(true);
                ChatPage.SetActive(true);
                AddFriendPage.SetActive(false);
                PendingPage.SetActive(false);
                Debug.Log(ChatButton.colors);
                _messengerIsOpened = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                ShowFriendList(_loadedData.playerId);
            }
        }
    }

    private void ShowFriendList(int playerId)
    {
        _player = new List<PlayerListObject>();
        foreach(Transform child in chatChannelObj.transform)
        {
            Destroy(child.gameObject);
        }
        
        IList<PlayerData> friends = BackendCommunicator.instance.FindAllFriends(playerId);

        for (int i = 0; i < friends.Count; i++)
        {
            AddPlayer(friends[i].Username, playerId, friends[i].Id, chatChannelObj);
            Debug.Log("add friends");
        }
    }

    private void ShowNotFriendList(int playerId)
    {
        _player = new List<PlayerListObject>();
        foreach (Transform child in addFriendChannelObj.transform)
        {
            Destroy(child.gameObject);
        }

        IList<PlayerData> allPlayers = BackendCommunicator.instance.FindAllPlayers();
        IList<PlayerData> friends = BackendCommunicator.instance.FindAllFriends(playerId);
        PlayerData player = BackendCommunicator.instance.FindOnePlayerById(playerId);

        IList<PlayerData> notFriends = allPlayers.Except(friends).ToList();
        notFriends.Remove(player);

        for (int i = 0; i < notFriends.Count; i++)
        {
            AddPlayer(notFriends[i].Username, playerId, notFriends[i].Id, addFriendChannelObj);
            Debug.Log("show player");
        }
    }

    private void AddPlayer(string playerName, int currentUserID, int playerID, GameObject Channel)
    {
        var newPlayer = Instantiate(playerListObj, Channel.transform);
        var newMessagePlayerObj = newPlayer.GetComponent<PlayerListObject>();

        newMessagePlayerObj.SelectButton.onClick.AddListener(() =>
        {
            ChannelSwitch(currentUserID, playerID);
        });

        newMessagePlayerObj.Username.text = playerName;
        _player.Add(newMessagePlayerObj);
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

    /*
    private IList<MessageObject> _message = new List<MessageObject>();
    private IList<FriendListObject> _friend = new List<FriendListObject>();

    public GameObject friendListObj, myTextObj, comingTextObj, channelObj, chatRoomObj;
    public Button sendButton;
    public TMP_InputField textInput;

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-ouawh";

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
    */
}

