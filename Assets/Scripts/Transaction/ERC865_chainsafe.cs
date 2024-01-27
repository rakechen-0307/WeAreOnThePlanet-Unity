using System;
using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using Nethereum.Signer;
using Web3Unity.Scripts.Library.Web3Wallet;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using SysRandom = System.Random;
using Nethereum.Util;
using System.Text;
using Unity.Collections;
using UnityEditor.VersionControl;
//using ERC865.ContractDefinition;
//using ERC721.ContractDefinition;

public class ERC865_chainsafe : MonoBehaviour
{
    [SerializeField] private Button _Connect;
    //[SerializeField] private Button _Deploy;
    [SerializeField] private Button _Signature;
    [SerializeField] private Button _Transfer;
    //[SerializeField] private Button _NFTDeploy;
    [SerializeField] private Button _NFTMint;
    [SerializeField] private Button _NFTSign;
    [SerializeField] private Button _NFTTransfer;
    //[SerializeField] private TMP_InputField _RPC;
    [SerializeField] private TMP_InputField _Address;
    [SerializeField] private TMP_InputField _PrivateKey;
    //[SerializeField] private TMP_Text _AddressText;
    public string chain;
    public string chainId;
    public string network;
    //private string _selectedAccountAddress;
    //private bool _isMetamaskInitialized = false;
    //private BigInteger _currentChainId;
    //private string _currentContractAddress;
    private string _currentSignature;
    private string _currentHash;
    SysRandom rnd = new SysRandom(Guid.NewGuid().GetHashCode());
    private BigInteger _nonce = 1;
    //private string _NFTContractAddress;
    private string _NFTContractSignature;
    private string _NFTContractHash;
    private string _accountAddress;


    ProjectConfigScriptableObject projectConfigSO = null;
    // Start is called before the first frame update
    void Start()
    {
        //_RPC.text = "http://localhost:8545/";
        //_Address.text = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";
        //_PrivateKey.text = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
        //_AddressText.text = "Address";
        _nonce = rnd.Next();
        Web3Wallet.url = "https://chainsafe.github.io/game-web3wallet/";
        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        PlayerPrefs.SetString("ProjectID", projectConfigSO.projectID);
        PlayerPrefs.SetString("ChainID", projectConfigSO.chainID);
        PlayerPrefs.SetString("Chain", projectConfigSO.chain);
        PlayerPrefs.SetString("Network", projectConfigSO.network);
        PlayerPrefs.SetString("RPC", projectConfigSO.rpc);
        chain = projectConfigSO.chain;
        chainId = projectConfigSO.chainID;
        network = projectConfigSO.network;
        // if remember me is checked, set the account to the saved account
        /*
        if (PlayerPrefs.HasKey("RememberMe") && PlayerPrefs.HasKey("Account"))
        {
            if (PlayerPrefs.GetInt("RememberMe") == 1 && PlayerPrefs.GetString("Account") != "")
            {
                // move to next scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        */
        // Connect Metamask
        _Connect.onClick.AddListener(() =>
        {
            Connect();
        });
        
        // Signature
        _Signature.onClick.AddListener(() =>
        {
            Signature();
        });

        // Delegated Transfer
        _Transfer.onClick.AddListener(() =>
        {
            Transfer();
        });

        // Mint NFT
        _NFTMint.onClick.AddListener(() =>
        {
            NFTMint();
        });

        // NFT Transfer Signature
        _NFTSign.onClick.AddListener(() =>
        {
            NFTSign();
        });

        _NFTTransfer.onClick.AddListener(() =>
        {
            NFTTransfer();
        });
        /*
        // Deploy
        _Deploy.onClick.AddListener(() =>
        {
            StartCoroutine(Deploy());
        });

        

        

        // Service End Deploy NFT
        _NFTDeploy.onClick.AddListener(() =>
        {
            StartCoroutine(NFTDeploy());
        });

        

        

        
        */
    }
    public string SignVerifySignature(string signatureString, string originalMessage)
    {
        var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
        var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
        var signature = MessageSigner.ExtractEcdsaSignature(signatureString);
        var key = EthECKey.RecoverFromSignature(signature, msgHash);
        return key.GetPublicAddress();
    }
    async private void Connect()
    {
// get current timestamp
        var timestamp = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        // set expiration time
        var expirationTime = timestamp + 60;
        // set message
        var message = expirationTime.ToString();
        // sign message
        Debug.Log("Signing...");
        var signature = await Web3Wallet.Sign(message);
        // verify account
        Debug.Log("Verifying...");
        var account = SignVerifySignature(signature, message);
        var now = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        // validate
        if (account.Length == 42 && expirationTime >= now)
        {
            // save account
            
            PlayerPrefs.SetString("Account", account);
            _accountAddress = account;
            _Address.text = account;
            Debug.Log(account);
        }
        else
        {
            Debug.Log("Connect Wallet Failed...");
        }
    }
    
    private async void Signature()
    {
        
        string method = "getTransferFromPreSignedHash";

        string receiver = "0xaE3CdFe3C638288E4CA5Db25A08133C36229ddB3";
        BigInteger value = 10000;
        BigInteger fee = 500;

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Debug.Log(_accountAddress);
        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider); 
        var data = await contract.Call(method, new object[]
        {
            _accountAddress,
            _accountAddress,
            receiver,
            value.ToString(),
            fee.ToString(),
            _nonce.ToString()
        });


        var result_hash = BitConverter.ToString((byte[])data[0]).Replace("-", string.Empty).ToLower();

        var signer = new EthereumMessageSigner();
        var signature1 = signer.EncodeUTF8AndSign(result_hash, new EthECKey(_PrivateKey.text));

        _currentSignature = signature1;
        _currentHash = result_hash;
        
        Debug.Log(_currentSignature);
        Debug.Log(_currentHash);
        _nonce = rnd.Next();

    }

    private async void Transfer()
    {
        var method = "transferPreSigned";

        string receiver = "0xaE3CdFe3C638288E4CA5Db25A08133C36229ddB3";
        BigInteger value = 10000;
        BigInteger fee = 500;

        var signer = new EthereumMessageSigner();
        var from = signer.EncodeUTF8AndEcRecover(_currentHash, _currentSignature);
        Debug.Log(_accountAddress);
        Debug.Log(from);

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);
            var data = contract.Calldata(method, new object[]
            {
                from,
                receiver,
                value.ToString(),
                fee.ToString(),
                _nonce.ToString()
            });
            // send transaction
            string response = await Web3Wallet.SendTransaction(chainId, ContractManager.TokenContract, "0", data, "", "");
            // display response in game
            print(response);
            print("Transaction successful!");
        }
        catch
        {
            print("Error with the transaction");
        }

        _nonce = rnd.Next();

    }

    private async void NFTMint()
    {
        var method = "safeMint";

        var to = _accountAddress;
        var uri = "ipfs://QmTQqtfx15vKbs5dKdhxgiTuY7eBATcJpFy9XQEhKTpxTU/0";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
            var data = contract.Calldata(method, new object[]
            {
                to,
                uri
            });
            // send transaction
            string response = await Web3Wallet.SendTransaction(chainId, ContractManager.NFTContract, "0", data, "", "");
            // display response in game
            print(response);
            print("Transaction successful!");
        }
        catch
        {
            print("Error with the transaction");
        }
    }
   
    private async void NFTSign()
    {
        string method = "getTransferPreSignedHash";

        string receiver = "0xaE3CdFe3C638288E4CA5Db25A08133C36229ddB3";
        BigInteger tokenId = 8;
        Debug.Log(_accountAddress);

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
            var data = await contract.Call(method, new object[]
            {
                _accountAddress,
                receiver,
                tokenId.ToString(),
                _nonce.ToString()
            });

            var result_hash = BitConverter.ToString((byte[])data[0]).Replace("-", string.Empty).ToLower();

            var signer = new EthereumMessageSigner();
            var signature1 = signer.EncodeUTF8AndSign(result_hash, new EthECKey(_PrivateKey.text));

            _NFTContractSignature = signature1;
            _NFTContractHash = result_hash;


            Debug.Log("Signature: " + signature1);
            Debug.Log("Hash: " + result_hash);
        }
        catch
        {
            Debug.Log("Fail to fetch hash.");
        }

        _nonce = rnd.Next();
    }

    private async void NFTTransfer()
    {
        var method = "transferPreSigned";

        var receiver = "0xaE3CdFe3C638288E4CA5Db25A08133C36229ddB3";
        BigInteger tokenId = 8;

        var signer = new EthereumMessageSigner();
        var from = signer.EncodeUTF8AndEcRecover(_NFTContractHash, _NFTContractSignature);
        Debug.Log("Correct address: " + _accountAddress);
        Debug.Log("Solved address: " + from);

        var provider = new JsonRpcProvider(ContractManager.RPC);

        try
        {
            Contract contract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
            var data = contract.Calldata(method, new object[]
            {
                from,
                receiver,
                tokenId.ToString(),
                _nonce.ToString(),
            });
            // send transaction
            string response = await Web3Wallet.SendTransaction(chainId, ContractManager.NFTContract, "0", data, "", "");
            // display response in game
            print(response);
            print("Transaction successful!");
        }
        catch
        {
            print("Error with the transaction");
        }

        _nonce = rnd.Next();

    }
}
    