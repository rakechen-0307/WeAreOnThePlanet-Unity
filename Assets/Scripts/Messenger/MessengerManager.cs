using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using Unity.Services.Core;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MessengerManager : MonoBehaviour
{
    public GameObject MessengerUI;
    public Button ChatButton;
    public Button AddFriendButton;
    public Button PendingButton;
    public Button TravelButton;

    public GameObject ChatPage;
    public Button SendButton;
    public TMP_InputField TextInput;

    public GameObject AddFriendPage;
    public GameObject PlayerView;
    public TMP_Text AddFriendUsername;
    public TMP_Text AddFriendLevel;
    public TMP_Text AddFriendNFT;
    public TMP_Text AddFriendFriends;
    public TMP_Text AddFriendAuction;
    public Button SendRequestButton;

    public GameObject PendingPage;
    public GameObject PendingView;
    public TMP_Text PendingUsername;
    public TMP_Text PendingLevel;
    public TMP_Text PendingNFT;
    public TMP_Text PendingFriends;
    public TMP_Text PendingAuction;
    public Button AcceptButton;
    public Button DenyButton;

    public GameObject TravelPage;
    public GameObject TravelView;
    public TMP_Text TravelUsername;
    public TMP_Text TravelLevel;
    public TMP_Text TravelNFT;
    public TMP_Text TravelFriends;
    public TMP_Text TravelAuction;
    public Button GoButton;


    public GameObject playerListObj, chatChannelObj, addFriendChannelObj, pendingChannelObj, travelChannelObj;
    public GameObject myTextObj, comingTextObj, chatRoomObj;
    public LoadedData _loadedData;
    public MainPlanetTravel mainPlanetTravel;

    private bool _messengerIsOpened;
    private string _currentChannel;

    private IList<PlayerListObject> _player = new List<PlayerListObject>();
    private IList<MessageObject> _message = new List<MessageObject>();

    private async void Awake()
    {
        MessengerUI.SetActive(false);
        _messengerIsOpened = false;

        await VivoxInitialize();
        await VivoxSignIn(_loadedData.playerId.ToString());
    }

    void Start()
    {
        VivoxService.Instance.ChannelMessageReceived += ChannelMessageReceived;

        ChatButton.onClick.AddListener(() =>
        {
            ChatPage.SetActive(true);
            AddFriendPage.SetActive(false);
            PendingPage.SetActive(false);
            TravelPage.SetActive(false);
            ShowFriendList(_loadedData.playerId);
        });

        AddFriendButton.onClick.AddListener(() =>
        {
            AddFriendPage.SetActive(true);
            ChatPage.SetActive(false);
            PendingPage.SetActive(false);
            PlayerView.SetActive(false);
            TravelPage.SetActive(false);
            ShowNotFriendList(_loadedData.playerId);
        });

        PendingButton.onClick.AddListener(() =>
        {
            PendingPage.SetActive(true);
            ChatPage.SetActive(false);
            AddFriendPage.SetActive(false);
            PendingView.SetActive(false);
            TravelPage.SetActive(false);
            ShowPendingList(_loadedData.playerId);
        });

        TravelButton.onClick.AddListener(() =>
        {
            PendingPage.SetActive(false);
            ChatPage.SetActive(false);
            AddFriendPage.SetActive(false);
            TravelView.SetActive(false);
            TravelPage.SetActive(true);
            ShowTravelList(_loadedData.playerId, _loadedData.mainPlayer.lastPlanetId);
        });

        SendButton.onClick.AddListener(() =>
        {
            if (TextInput.text != string.Empty)
            {
                ChannelSendMessageAsync(_currentChannel, TextInput.text);
                AddMessage(TextInput.text, true);
                TextInput.text = string.Empty;
            }
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
                TravelPage.SetActive(false);
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
            AddFriendToList(friends[i].Username, playerId, friends[i].Id);
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
        IList<PendingFreiendInfo> pendings = BackendCommunicator.instance.FindAllPendingFriends(playerId);
        IList<PlayerData> pendingFriends = new List<PlayerData>();
        for (int i = 0; i < pendings.Count; i++)
        {
            pendingFriends.Add(pendings[i].Player);
        }
        PlayerData player = BackendCommunicator.instance.FindOnePlayerById(playerId);

        IList<PlayerData> notFriends = allPlayers.Except(friends).ToList().Except(pendingFriends).ToList();
        notFriends.Remove(player);

        for (int i = 0; i < notFriends.Count; i++)
        {
            AddNotFriendToList(notFriends[i].Username, playerId, notFriends[i].Id);
        }
    }

    private void ShowPendingList(int playerId)
    {
        _player = new List<PlayerListObject>();
        foreach (Transform child in pendingChannelObj.transform)
        {
            Destroy(child.gameObject);
        }

        IList<PendingFreiendInfo> pendings = BackendCommunicator.instance.FindAllPendingFriends(playerId);

        for (int i = 0; i < pendings.Count; i++)
        {
            AddPendingToList(pendings[i].Player.Username, playerId, pendings[i].Player.Id, pendings[i].IsSender);
        }
    }

    private void ShowTravelList(int playerId, int planetId)
    {
        _player = new List<PlayerListObject>();
        foreach (Transform child in travelChannelObj.transform)
        {
            Destroy(child.gameObject);
        }

        IList<PlayerData> friends = BackendCommunicator.instance.FindAllFriends(playerId);

        if (playerId != planetId)
        {
            AddTravelToList("Your Planet", playerId);
        }

        for (int i = 0; i < friends.Count; i++)
        {
            if (friends[i].Id != planetId)
            {
                AddTravelToList(friends[i].Username, friends[i].Id);
            }
        }
    }

    private void AddFriendToList(string friendName, int currentUserID, int friendID)
    {
        var newPlayer = Instantiate(playerListObj, chatChannelObj.transform);
        var newMessagePlayerObj = newPlayer.GetComponent<PlayerListObject>();

        newMessagePlayerObj.SelectButton.onClick.AddListener(() =>
        {
            ChannelSwitch(currentUserID, friendID);
        });

        newMessagePlayerObj.Username.text = friendName;
        _player.Add(newMessagePlayerObj);
    }

    private void AddNotFriendToList(string playerName, int currentUserID, int playerID)
    {
        var newPlayer = Instantiate(playerListObj, addFriendChannelObj.transform);
        var newMessagePlayerObj = newPlayer.GetComponent<PlayerListObject>();

        newMessagePlayerObj.SelectButton.onClick.AddListener(() =>
        {
            ShowPlayerInfo(currentUserID, playerID, newMessagePlayerObj.SelectButton);
        });

        newMessagePlayerObj.Username.text = playerName;
        _player.Add(newMessagePlayerObj);
    }

    private void AddPendingToList(string pendingName, int currentUserID, int pendingID, bool isSender)
    {
        var newPlayer = Instantiate(playerListObj, pendingChannelObj.transform);
        var newMessagePlayerObj = newPlayer.GetComponent<PlayerListObject>();

        newMessagePlayerObj.SelectButton.onClick.AddListener(() =>
        {
            ShowPendingInfo(currentUserID, pendingID, newMessagePlayerObj.SelectButton, isSender);
        });

        newMessagePlayerObj.Username.text = pendingName;
        _player.Add(newMessagePlayerObj);
    }

    private void AddTravelToList(string friendName, int friendID)
    {
        var newPlayer = Instantiate(playerListObj, travelChannelObj.transform);
        var newMessagePlayerObj = newPlayer.GetComponent<PlayerListObject>();

        newMessagePlayerObj.SelectButton.onClick.AddListener(() =>
        {
            ShowTravelInfo(friendID);
        });

        newMessagePlayerObj.Username.text = friendName;
        _player.Add(newMessagePlayerObj);
    }

    public void ShowPlayerInfo(int currentUserID, int playerID, Button button)
    {
        PlayerData playerData = BackendCommunicator.instance.FindOnePlayerById(playerID);
        int auctionCount = BackendCommunicator.instance.FindHeldAuctionByPlayerID(playerID).Count;

        AddFriendUsername.text = playerData.Username;
        AddFriendLevel.text = "Planet Level : " + playerData.Exp.ToString();
        AddFriendNFT.text = "Owned NFT : " + playerData.NFTs.Count.ToString();
        AddFriendFriends.text = "Total Friends : " + playerData.Friends.Count.ToString();
        AddFriendAuction.text = "Auctions Hold : " + auctionCount.ToString();
        PlayerView.SetActive(true);

        SendRequestButton.onClick.AddListener(() =>
        {
            SendRequestToPlayer(currentUserID, playerID, button);
        });
    }

    public void ShowPendingInfo(int currentUserID, int playerID, Button button, bool isSender)
    {
        PlayerData playerData = BackendCommunicator.instance.FindOnePlayerById(playerID);
        int auctionCount = BackendCommunicator.instance.FindHeldAuctionByPlayerID(playerID).Count;

        PendingUsername.text = playerData.Username;
        PendingLevel.text = "Planet Level : " + playerData.Exp.ToString();
        PendingNFT.text = "Owned NFT : " + playerData.NFTs.Count.ToString();
        PendingFriends.text = "Total Friends : " + playerData.Friends.Count.ToString();
        PendingAuction.text = "Auctions Hold : " + auctionCount.ToString();
        PendingView.SetActive(true);

        if (isSender)
        {
            AcceptButton.gameObject.SetActive(false);
            DenyButton.gameObject.SetActive(false);
        }

        AcceptButton.onClick.AddListener(() =>
        {
            AcceptRequest(currentUserID, playerID, button);
        });

        DenyButton.onClick.AddListener(() =>
        {
            DenyRequest(currentUserID, playerID, button);
        });
    }

    private void ShowTravelInfo(int friendID)
    {
        PlayerData playerData = BackendCommunicator.instance.FindOnePlayerById(friendID);
        int auctionCount = BackendCommunicator.instance.FindHeldAuctionByPlayerID(friendID).Count;

        TravelUsername.text = playerData.Username;
        TravelLevel.text = "Planet Level : " + playerData.Exp.ToString();
        TravelNFT.text = "Owned NFT : " + playerData.NFTs.Count.ToString();
        TravelFriends.text = "Total Friends : " + playerData.Friends.Count.ToString();
        TravelAuction.text = "Auctions Hold : " + auctionCount.ToString();
        TravelView.SetActive(true);

        GoButton.onClick.AddListener(() =>
        {
            mainPlanetTravel.TravelPlanet(friendID);
        });
    }

    public async void SendRequestToPlayer(int from, int to, Button button)
    {
        await BackendCommunicator.instance.AddPendingFriend(from, to);
        Destroy(button.gameObject);
        PlayerView.SetActive(false);
    }

    public async void AcceptRequest(int from, int to, Button button)
    {
        await BackendCommunicator.instance.AcceptFriend(from, to);
        Destroy(button.gameObject);
        PendingView.SetActive(false);
    }

    public async void DenyRequest(int from, int to, Button button)
    {
        await BackendCommunicator.instance.DenyFriend(from, to);
        Destroy(button.gameObject);
        PendingView.SetActive(false);
    }

    public void ChannelMessageReceived(VivoxMessage message)
    {
        var messageText = message.MessageText;
        var fromSelf = message.FromSelf;

        if (!fromSelf)
        {
            AddMessage(messageText, false);
        }
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
            if (chatRoomObj == null)
            {
                return;
            }

            var newMes = Instantiate(comingTextObj, chatRoomObj.transform);
            var newMessageTextObj = newMes.GetComponent<MessageObject>();

            newMessageTextObj.Message.text = message;

            _message.Add(newMessageTextObj);
        }
    }

    public async void ChannelSwitch(int from, int to)
    {
        string channelToJoin = (from < to) ? (from.ToString() + "_" + to.ToString()) : (to.ToString() + "_" + from.ToString());
        if (channelToJoin != _currentChannel)
        {
            await VivoxService.Instance.LeaveAllChannelsAsync();

            _message = new List<MessageObject>();
            foreach (Transform child in chatRoomObj.transform)
            {
                Destroy(child.gameObject);
            }

            JoinChannelAsync(channelToJoin);
            _currentChannel = channelToJoin;
        }
        Debug.Log(channelToJoin);
    }

    public async void JoinChannelAsync(string channelName)
    {
        await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.TextOnly);

        // get history messages
        var historyOptions = new ChatHistoryQueryOptions();
        historyOptions.TimeStart = DateTime.Now.AddHours(-1);
        historyOptions.TimeEnd = DateTime.Now;

        var historyMessages = await VivoxService.Instance.GetChannelTextMessageHistoryAsync(channelName, 50, historyOptions);
        for (int i = 0; i < historyMessages.Count; i++)
        {
            VivoxMessage vivoxMessage = historyMessages[i];
            AddMessage(vivoxMessage.MessageText, vivoxMessage.FromSelf);
        }
    }

    public async void ChannelSendMessageAsync(string channelName, string message)
    {
        await VivoxService.Instance.SendChannelTextMessageAsync(channelName, message);
    }

    private async Task<bool> VivoxInitialize()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            await VivoxService.Instance.InitializeAsync();
            Debug.Log("Vivox Initialized");
        }
        catch (Exception)
        {
            Debug.Log("Authencation SignIn Has Existed");
        }

        return true;
    }

    private async Task<bool> VivoxSignIn(string displayName)
    {
        var loginOption = new LoginOptions
        {
            DisplayName = displayName,
            EnableTTS = false
        };

        try
        {
            await VivoxService.Instance.LogoutAsync();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        try
        {
            await VivoxService.Instance.LoginAsync(loginOption);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        Debug.Log("Log in");
        return true;
    }
}

