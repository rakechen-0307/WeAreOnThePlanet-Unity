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
using System.Threading.Tasks;

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
    public TMP_InputField UserNameInput;
    public TMP_InputField PasswordInput;

    public TMP_Text JoinCodeText;
    public TMP_Text HostErrorText;
    public TMP_Text JoinErrorText;
    public TMP_Text LogInErrorText;

    ProjectConfigScriptableObject projectConfigSO = null;

    private void Awake()
    {
        StartPage.SetActive(true);
        HostPage.SetActive(false);
        PlayerPage.SetActive(false);
        HostErrorText.enabled = false;
        JoinErrorText.enabled = false;
        LogInErrorText.enabled = false;

        // setup ChainSafe
        ChainSafeSetup();
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

        SignUpButton.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(UserNameInput.text) || string.IsNullOrEmpty(PasswordInput.text))
            {
                LogInErrorText.enabled = true;
                LogInErrorText.text = "Please Enter Your Username and Password";
            }
            else
            {
                // Check if user is already existed and store user information

                //

                // Player Connect Wallet
                PlayerConnectWallet();
            }
        });

        LogInButton.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(UserNameInput.text) || string.IsNullOrEmpty(PasswordInput.text))
            {
                LogInErrorText.enabled = true;
                LogInErrorText.text = "Please Enter Your Username and Password";
            }
            else
            {
                // Check if user information is correct

                //

                // Player Connect Wallet
                PlayerConnectWallet();
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
            print("Account: " + account);
            SceneManager.LoadScene("MainPlanet");
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
