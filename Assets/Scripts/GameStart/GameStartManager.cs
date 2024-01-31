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
using System;
using UnityEditor;
using Unity.Services.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using System.Threading.Tasks;

public class GameStartManager : Singleton<GameStartManager>
{
    [SerializeField] bool isHost;

    /* Start Page */
    public GameObject StartPage;
    public Button StartHostButton;
    public Button StartGameButton;
    public TMP_Text StartErrorText;

    /* Host Page */
    public GameObject HostPage;
    public TMP_Text JoinCodeText;

    /* Player Page */
    public GameObject PlayerPage;
    // Email Page
    public GameObject EmailPage;
    public Button CreateAccountButton;
    public Button SignInButton;
    public GameObject Email;
    public TMP_InputField EmailInput;
    public GameObject JoinCode;
    public TMP_InputField JoinCodeInput;
    public TMP_Text EmailErrorText;
    // Create Account Page
    public GameObject CreateAccountPage;
    public Button JoinButton;
    public GameObject CreateAccountUsername;
    public TMP_InputField CreateAccountUsernameInput;
    public GameObject CreateAccountPassword;
    public TMP_InputField CreateAccountPasswordInput;
    public TMP_Text CreateAccountErrorText;
    // Sign In Page
    public GameObject SigninPage;
    public Button StartPlayButton;
    public GameObject SignInPassword;
    public TMP_InputField SignInPasswordInput;
    public TMP_Text SignInErrorText;

    private string _email;
    private string _joinCode;
    private string _username;
    private string _password;
    private int _playerId;
    private PlayerData _player;
    public LoadedData _loadedData;

    ProjectConfigScriptableObject projectConfigSO = null;

    private async void Awake()
    {
        StartPage.SetActive(true);
        HostPage.SetActive(false);
        PlayerPage.SetActive(false);
        StartErrorText.gameObject.SetActive(true);
        EmailErrorText.gameObject.SetActive(false);
        CreateAccountErrorText.gameObject.SetActive(false);
        SignInErrorText.gameObject.SetActive(false);

        if (isHost)
        {
            StartHostButton.gameObject.SetActive(true);
            StartGameButton.gameObject.SetActive(false);
        }
        else
        {
            StartGameButton.gameObject.SetActive(true);
            StartHostButton.gameObject.SetActive(false);
        }

        ChainSafeSetup();  // setup ChainSafe
        bool success = await VivoxInitialize();  // initialize Vivox
    }

    // Start is called before the first frame update
    void Start()
    {
        StartHostButton.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.isRelayEnabled)
            {
                RelayHostData HostData = await RelayManager.Instance.SetupRelay();
                JoinCodeText.gameObject.SetActive(true);
                JoinCodeText.enabled = true;
                JoinCodeText.text = "Join Code : " + HostData.JoinCode;
            }

            if (NetworkManager.Singleton.StartHost())
            {
                HostConnect();
            }
            else
            {
                StartErrorText.enabled = true;
                StartErrorText.text = "Start Host Failed...";
            }
        });

        StartGameButton.onClick.AddListener(() =>
        {
            StartPage.SetActive(false);
            PlayerPage.SetActive(true);
            EmailPage.SetActive(true);
            EmailErrorText.gameObject.SetActive(true);
        });

        CreateAccountButton.onClick.AddListener(async () =>
        {
            _email = EmailInput.text;
            _joinCode = JoinCodeInput.text;
            if (string.IsNullOrEmpty(_email) || string.IsNullOrEmpty(_joinCode))
            {
                EmailErrorText.enabled = true;
                EmailErrorText.text = "Please Fill In Your Email & Join Code";
            }
            else
            {
                PlayerData findPlayer = BackendCommunicator.instance.FindOnePlayerByEmail(_email);
                if (findPlayer == null)
                {
                    EmailErrorText.enabled = true;
                    EmailErrorText.text = "Waiting For Correct Join...";
                    if (RelayManager.Instance.isRelayEnabled)
                    {
                        RelayJoinData JoinData = await RelayManager.Instance.JoinRelay(_joinCode);
                    }

                    EmailErrorText.text = "";
                    if (NetworkManager.Singleton.StartClient())
                    {
                        EmailPage.SetActive(false);
                        CreateAccountPage.SetActive(true);
                        CreateAccountErrorText.gameObject.SetActive(true);
                    }
                    else
                    {
                        EmailErrorText.text = "Unable to Join...";
                    }
                }
                else
                {
                    EmailErrorText.enabled = true;
                    EmailErrorText.text = "User Has Already Existed";
                }
            }
        });

        SignInButton.onClick.AddListener(async () =>
        {
            _email = EmailInput.text;
            _joinCode = JoinCodeInput.text;
            if (string.IsNullOrEmpty(_email) || string.IsNullOrEmpty(_joinCode))
            {
                EmailErrorText.enabled = true;
                EmailErrorText.text = "Please Fill In Your Email & Join Code";
            }
            else
            {
                PlayerData findPlayer = BackendCommunicator.instance.FindOnePlayerByEmail(_email);
                if (findPlayer != null)
                {
                    Debug.Log("found");
                    _player = findPlayer;
                    EmailErrorText.enabled = true;
                    EmailErrorText.text = "Waiting For Correct Join...";
                    if (RelayManager.Instance.isRelayEnabled)
                    {
                        RelayJoinData JoinData = await RelayManager.Instance.JoinRelay(_joinCode.Substring(0, 6));
                    }

                    EmailErrorText.text = "";
                    if (NetworkManager.Singleton.StartClient())
                    {
                        EmailPage.SetActive(false);
                        SigninPage.SetActive(true);
                        SignInErrorText.gameObject.SetActive(true);
                    }
                    else
                    {
                        EmailErrorText.text = "Unable to Join...";
                    }
                }
                else
                {
                    EmailErrorText.enabled = true;
                    EmailErrorText.text = "User Doesn't Exist";
                }
            }
        });

        JoinButton.onClick.AddListener(() =>
        {
            _username = CreateAccountUsernameInput.text;
            _password = CreateAccountPasswordInput.text;
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                CreateAccountErrorText.enabled = true;
                CreateAccountErrorText.text = "Please Fill In Your Username & Password";
            }
            else
            {
                PlayerConnect(true, _email, _username, _password);
            }
        });

        StartPlayButton.onClick.AddListener(() =>
        {
            _password = SignInPasswordInput.text;
            if (string.IsNullOrEmpty(_password))
            {
                SignInErrorText.enabled = true;
                SignInErrorText.text = "Please Fill In Your Password";
            }
            else
            {
                if (_password != _player.Password)
                {
                    SignInErrorText.enabled = true;
                    SignInErrorText.text = "Password Is Wrong";
                }
                else
                {
                    _playerId = _player.Id;
                    PlayerConnect(false, _email, _username, _password);
                }
            }
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

    private async void HostConnect()
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
            await VivoxSignIn("host");
            StartPage.SetActive(false);
            HostPage.SetActive(true);
        }
        else
        {
            StartErrorText.enabled = true;
            StartErrorText.text = "Wallet Connect Failed...";
        }
    }

    private async void PlayerConnect(bool isNew, string email, string username, string password)
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
            PlayerPrefs.SetString("Email", email);
            print("Account: " + account);
            
            if (isNew)
            {
                Debug.Log("isnew");
                PlayerData findPlayer = BackendCommunicator.instance.FindOnePlayerByAccount(account);
                Debug.Log("isnew2");
                
                if (findPlayer != null)
                {
                    Debug.Log("isnew3");
                    SignInErrorText.enabled = true;
                    SignInErrorText.text = "Account has been used";
                    return;
                }
                _playerId = await BackendCommunicator.instance.CreateOnePlayer(email, username, password, account);
                Debug.Log("playerID = " + _playerId);
            }
            else
            {
                Debug.Log("isn'tnew");
                PlayerData findPlayer = BackendCommunicator.instance.FindOnePlayerByEmail(email);
                Debug.Log("isn'tnew2");
                if (findPlayer.Account != account)
                {
                    SignInErrorText.enabled = true;
                    SignInErrorText.text = "Wrong account";
                    return;
                }
            }
            
            await VivoxSignIn(_playerId.ToString());
            int planetId = BackendManager.instance.loadMainPlayerData(_playerId, _loadedData);
            Debug.Log(planetId);
            if (planetId == -1)  // NFT Workshop 
            {
                SceneManager.LoadScene("CreateNFT");
            }
            else if (planetId == -2)  // Octopus
            {
                SceneManager.LoadScene("Octopus");
            }
            else
            {
                SceneManager.LoadScene("Octopus");
                //SceneManager.LoadScene("MainPlanet");
            }
        }
        else
        {
            SignInErrorText.enabled = true;
            SignInErrorText.text = "Wallet Connect Failed...";
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
            await VivoxService.Instance.LoginAsync(loginOption);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        Debug.Log("Log in");
        return true;
    }
}
