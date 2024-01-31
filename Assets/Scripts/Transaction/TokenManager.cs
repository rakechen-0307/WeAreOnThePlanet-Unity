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

public class TokenManager : MonoBehaviour
{

    [SerializeField] private Button _freeToken;
    private string _chain;
    private string _chainId;
    private string _network;
    private string _account;
    ProjectConfigScriptableObject projectConfigSO = null;
    private SysRandom rnd = new SysRandom(Guid.NewGuid().GetHashCode());
    private BigInteger _nonce;

    private string _currentSignature;
    private string _currentHash;
    private string _privateKey = "a59c5a408aa6e27cb176eabc80dea85f68045cb2e5ce8fc28ef3dda0f6ac52c0";

    // Start is called before the first frame update
    void Start()
    {
        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        _chain = projectConfigSO.chain;
        _chainId = projectConfigSO.chainID;
        _network = projectConfigSO.network;
        _account = PlayerPrefs.GetString("Account");
        _nonce = rnd.Next();
        _freeToken.onClick.AddListener(() =>
        {
            AskForFreeToken();
        });
    }
    public string SignVerifySignature(string signatureString, string originalMessage)
    {
        var msg = "\x19" + "Ethereum Signed Message:\n" + originalMessage.Length + originalMessage;
        var msgHash = new Sha3Keccack().CalculateHash(Encoding.UTF8.GetBytes(msg));
        var signature = MessageSigner.ExtractEcdsaSignature(signatureString);
        var key = EthECKey.RecoverFromSignature(signature, msgHash);
        return key.GetPublicAddress();
    }
    private async void AskForFreeToken()
    {
        string method = "getTransferFromPreSignedHash";

        string host = "0x54eb55C16EEF4a9fc49f889535Dc7ff443364827";
        string receiver = _account;
        BigInteger value = 60000;
        BigInteger fee = 0;

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Debug.Log(_account);
        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);
        var data = await contract.Call(method, new object[]
        {
            host,
            host,
            receiver,
            value.ToString(),
            fee.ToString(),
            _nonce.ToString()
        });


        var result_hash = BitConverter.ToString((byte[])data[0]).Replace("-", string.Empty).ToLower();

        var signer = new EthereumMessageSigner();
        var signature1 = signer.EncodeUTF8AndSign(result_hash, new EthECKey(_privateKey));

        _currentSignature = signature1;
        _currentHash = result_hash;

        Debug.Log(_currentSignature);
        Debug.Log(_currentHash);
        _nonce = rnd.Next();
    }
}
