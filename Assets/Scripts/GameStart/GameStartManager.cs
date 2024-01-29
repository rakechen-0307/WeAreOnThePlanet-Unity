using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Core.Singletons;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Nethereum.Signer;
using Nethereum.Util;
using Web3Unity.Scripts.Library.Web3Wallet;
using System.Text;
using Realms;
using Realms.Sync;
using System.Linq;
using System;
using UnityEditor;
using Unity.Services.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;

public class GameStartManager : Singleton<GameStartManager>
{
    public GameObject StartPage;
    public GameObject HostPage;
    public GameObject HostMainPage;
    public GameObject PlayerPage;
    public GameObject PlayerJoin;
    public GameObject PlayerLogIn;

    public Button HostButton;
    public Button PlayerButton;
    public Button HostWalletDisconnect;
    public Button ViewRequest;
    public Button JoinButton;
    public Button SignUpButton;
    public Button LogInButton;

    public TMP_InputField JoinCodeInput;
    public TMP_InputField EmailInput;
    public TMP_InputField UserNameInput;
    public TMP_InputField PasswordInput;

    public TMP_Text JoinCodeText;
    public TMP_Text HostErrorText;
    public TMP_Text JoinErrorText;
    public TMP_Text LogInErrorText;

    ProjectConfigScriptableObject projectConfigSO = null;

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-hhbzr";
    private static int TASKNUM = 1;

    public string _email;
    public string _username;
    public string _password;


    private async void Awake()
    {
        StartPage.SetActive(true);
        HostPage.SetActive(false);
        PlayerPage.SetActive(false);
        HostErrorText.enabled = false;
        JoinErrorText.enabled = false;
        LogInErrorText.enabled = false;

        // setup ChainSafe
        ChainSafeSetup();

        // Initialize Vivox
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await VivoxService.Instance.InitializeAsync();
        Debug.Log("Vivox Initialized");
    }

    // Start is called before the first frame update
    void Start()
    {
        HostButton.onClick.AddListener(async() =>
        {
            // Start Host
            if (RelayManager.Instance.isRelayEnabled)
            {
                RelayHostData HostData = await RelayManager.Instance.SetupRelay();
                JoinCodeText.text = "Join Code : " + HostData.JoinCode;
            }

            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host Started...");

                // Vivox Sign In
                VivoxSignIn("host");

                // Host Connect Wallet
                HostConnectWallet();
            }
            else
            {
                HostErrorText.enabled = true;
                HostErrorText.text = "Start Host Failed...";
            }
        });

        PlayerButton.onClick.AddListener(() =>
        {
            RealmSetup();
            StartPage.SetActive(false);
            PlayerPage.SetActive(true);
            PlayerJoin.SetActive(true);
        });

        JoinButton.onClick.AddListener(async() =>
        {
            // Start Client
            JoinErrorText.enabled = true;
            JoinErrorText.text = "Waiting For Correct Join...";
            if (RelayManager.Instance.isRelayEnabled && !string.IsNullOrEmpty(JoinCodeInput.text))
            {
                RelayJoinData JoinData = await RelayManager.Instance.JoinRelay(JoinCodeInput.text);
            }

            JoinErrorText.enabled = false;
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client Started...");
                PlayerJoin.SetActive(false);
                PlayerLogIn.SetActive(true);
            }
            else
            {
                JoinErrorText.enabled = true;
                JoinErrorText.text = "Unable to Join...";
            }
        });

        SignUpButton.onClick.AddListener(async () =>
        {
            if (string.IsNullOrEmpty(UserNameInput.text) || string.IsNullOrEmpty(PasswordInput.text))
            {
                LogInErrorText.enabled = true;
                LogInErrorText.text = "Please Enter Your Username and Password";
                return;
            }

            // subscription
            var playerQuery = _realm.All<PlayerData>();
            await playerQuery.SubscribeAsync();

            var taskQuery = _realm.All<Task>();
            await taskQuery.SubscribeAsync();

            // Check if user is already existed and store user information
            PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == EmailInput.text).FirstOrDefault();

            if (findPlayer != null)
            {
                LogInErrorText.enabled = true;
                LogInErrorText.text = "User Has Already Existed";
                return;
            }

            var totalPlayer = playerQuery.ToArray().Length;
            await _realm.WriteAsync(() =>
            {
                findPlayer = _realm.Add(new PlayerData()
                {
                    Id = totalPlayer + 1,
                    Email = EmailInput.text,
                    Username = UserNameInput.text,
                    Password = PasswordInput.text,
                    Position = new PlayerPosition()
                    {
                        PosX = 0,
                        PosY = 0,
                        PosZ = 0,
                        RotX = 0,
                        RotY = 0,
                        RotZ = 0,
                    }
                });
            });

            if (totalPlayer >= 1)
            {
                var players = playerQuery.ToArray();
                for (int i=0;  i < players.Length-1; i++)
                {
                    await _realm.WriteAsync(() =>
                    {
                        players[i].Friends.Add(findPlayer);
                        findPlayer.Friends.Add(players[i]);
                    });
                }
            }

            _email = EmailInput.text;
            _username = UserNameInput.text;
            _password = PasswordInput.text;

            // Vivox Sign In
            VivoxSignIn(findPlayer.Id.ToString());

            // Player Connect Wallet
            PlayerConnectWallet();
        });

        LogInButton.onClick.AddListener(async () =>
        {
            if (string.IsNullOrEmpty(UserNameInput.text) || string.IsNullOrEmpty(PasswordInput.text))
            {
                LogInErrorText.enabled = true;
                LogInErrorText.text = "Please Enter Your Username and Password";
                return;
            }

            // subscription
            var playerQuery = _realm.All<PlayerData>();
            await playerQuery.SubscribeAsync();

            var taskQuery = _realm.All<Task>();
            await taskQuery.SubscribeAsync();

            // Check if user information is correct
            PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == EmailInput.text).FirstOrDefault();

            if (findPlayer == null)
            {
                LogInErrorText.enabled = true;
                LogInErrorText.text = "User Does Not Exist";
                return;
            }

            if (findPlayer.Password !=  PasswordInput.text)
            {
                LogInErrorText.enabled = true;
                LogInErrorText.text = "Password Is Wrong";
                return;
            }

            if (findPlayer.Username != UserNameInput.text)
            {
                await _realm.WriteAsync(() =>
                {
                    findPlayer.Username = UserNameInput.text;
                });
            }

            _email = EmailInput.text;
            _username = UserNameInput.text;
            _password = PasswordInput.text;

            // Vivox Sign In
            VivoxSignIn(findPlayer.Id.ToString());

            // Player Connect Wallet
            PlayerConnectWallet();
        });
    }

    private void ChainSafeSetup()
    {
        // change this if you are implementing your own sign in page
        Web3Wallet.url = "https://chainsafe.github.io/game-web3wallet/";
        // loads the data saved from the editor config
        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
        PlayerPrefs.SetString("ChainID", projectConfigSO.ChainID);
        PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        PlayerPrefs.SetString("Network", projectConfigSO.Network);
        PlayerPrefs.SetString("RPC", projectConfigSO.RPC);
    } 

    private async void HostConnectWallet()
    {
        // get current timestamp
        var timestamp = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        // set expiration time
        var expirationTime = timestamp + 60;
        // set message
        var message = expirationTime.ToString();
        // sign message
        var signature = await Web3Wallet.Sign(message);
        // verify account
        var account = SignVerifySignature(signature, message);
        var now = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        // validate
        if (account.Length == 42 && expirationTime >= now)
        {
            PlayerPrefs.SetString("Account", account);
            print("Account: " + account);
            StartPage.SetActive(false);
            HostPage.SetActive(true);
            HostMainPage.SetActive(true);
        }
        else
        {
            HostErrorText.enabled = true;
            HostErrorText.text = "Connect Wallet Failed...";
        }
    }

    private async void PlayerConnectWallet()
    {
        var timestamp = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        var expirationTime = timestamp + 60;
        var message = expirationTime.ToString();
        var signature = await Web3Wallet.Sign(message);
        var account = SignVerifySignature(signature, message);
        var now = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        // validate
        if (account.Length == 42 && expirationTime >= now)
        {
            PlayerPrefs.SetString("Account", account);
            print("Account: " + account);
            SceneManager.LoadScene("CrystalMessengerTest");
        }
        else
        {
            LogInErrorText.enabled = true;
            LogInErrorText.text = "Connect Wallet Failed...";
        }
    }

    public string SignVerifySignature(string signatureString, string originalMessage)
    {
        var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
        var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
        var signature = MessageSigner.ExtractEcdsaSignature(signatureString);
        var key = EthECKey.RecoverFromSignature(signature, msgHash);
        return key.GetPublicAddress();
    }

    public async void RealmSetup()
    {
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
        await _realm.WriteAsync(() =>
        {
            var taskQuery = _realm.All<Task>();
            var totalTask = taskQuery.ToArray().Length;
            Debug.Log("Initial task number:");
            Debug.Log(totalTask);
            if(totalTask != TASKNUM)
            {
                Task findTask = _realm.All<Task>().Where(task => task.Id == 0).FirstOrDefault();
                if (findTask == null)
                {
                    findTask = _realm.Add(new Task()
                    {
                        Id = 0,
                        Description = "Click the button",
                        Prize = 30000,
                    });
                }
            }
        });
    }

    async void VivoxSignIn(string displayName)
    {
        var loginOption = new LoginOptions
        {
            DisplayName = displayName,
            EnableTTS = false
        };
        await VivoxService.Instance.LoginAsync(loginOption);
        Debug.Log("Log in");
    }
}
