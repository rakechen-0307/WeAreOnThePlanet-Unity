using Realms;
using Realms.Sync;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using Newtonsoft.Json;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using System;
#if UNITY_EDITOR
using UnityEditor.VersionControl;
#endif
using Web3Unity.Scripts.Library.Web3Wallet;
using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using SysRandom = System.Random;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;

public class SushiManager : MonoBehaviour
{
    
    public GameObject sushiPrefab;
    public DialogButton dialogButton;

    public string viewMode = "none";
    public int viewNumber = 0;
    private bool isLeaving = false;
    private bool isComing = false;

    public List<NFTInfo> Displayed = new List<NFTInfo>();
    public List<NFTInfo> Mint = new List<NFTInfo>();
    public List<NFTInfo> Transfer = new List<NFTInfo>();
    public List<NFTInfo> Launch = new List<NFTInfo>();
    public List<NFTInfo> Attend = new List<NFTInfo>();
    public List<NFTInfo> Empty = new List<NFTInfo>() { };

    private List<GameObject> sushiInstances = new List<GameObject>();
    private float moveStartTime;

    //private Realm _realm;
    //private App _realmApp;
    //private User _realmUser;
    //private string _realmAppID = "weareontheplanet-ouawh";

    public static BigInteger price = 100;
    public static BigInteger fee = 5;
    public static int selected = 0;

    SysRandom rnd = new SysRandom(Guid.NewGuid().GetHashCode());

    [SerializeField]
    private NFTDisplayer nftDisplayer;

    public void SetDisplayed(int num, string mode)
    {
        Debug.Log(num.ToString() + mode);
        //if(isLeaving || isComing) return;

        Displayed.Clear();
        if(mode == "mint") {for(int i=num; i<num+4; i++) if(i<Mint.Count) Displayed.Add(Mint[i]);}
        else if(mode == "transfer") {for(int i=num; i<num+4; i++) if(i<Transfer.Count) Displayed.Add(Transfer[i]);}
        else if(mode == "launch") {for(int i=num; i<num+4; i++) if(i<Launch.Count) Displayed.Add(Launch[i]);}
        else if(mode == "attend") {for(int i=num; i<num+4; i++) if(i<Attend.Count) Displayed.Add(Attend[i]);}

        viewNumber = num;
        viewMode = mode;

        if(sushiInstances.Count > 0){
            say("SetDisplayed called, currently there are instances of sushi's.");
            isLeaving = true;
            moveStartTime = Time.time;
        }
        else{
            say("SetDisplayed called, currently there are no instances of sushi's.");
            ResetComingSushiInstance();            
        }
    }
    
    private float sushiMoveSpeed = 1f;
    private float sushiMoveTime = 1f;
    private Vector3 sushiMoveDirection = new Vector3(0f, 0f, 20f);
    void Update()
    {
        float elapsedTime = Time.time - moveStartTime;
        if(sushiInstances.Count > 0 && isLeaving){
            if(elapsedTime <= sushiMoveTime){
                foreach(GameObject sushiInstance in sushiInstances){
                    float sin = Mathf.Sin(elapsedTime * sushiMoveSpeed);
                    sushiInstance.transform.Translate(sushiMoveDirection * sin * Time.deltaTime);
                }
            }
            else {
                isLeaving = false;
                foreach (GameObject sushiInstance in sushiInstances) Destroy(sushiInstance);
                sushiInstances.Clear();
                ResetComingSushiInstance();
            }
        }
        else if(sushiInstances.Count > 0 && isComing){
            if(elapsedTime <= sushiMoveTime){
                foreach(GameObject sushiInstance in sushiInstances){
                    float sin = Mathf.Sin(elapsedTime * sushiMoveSpeed);
                    sushiInstance.transform.Translate(sushiMoveDirection * sin * Time.deltaTime);
                }
            }
            else {
                isComing = false;
            }
        }
    }

    public void SetMint(List<NFTInfo> nfts)
    {
        viewMode = "mint";
        Mint = nfts;
    }
    public void SetTransfer(List<NFTInfo> nfts)
    {
        viewMode = "transfer";
        Transfer = nfts;
    }
    public void SetLaunch(List<NFTInfo> nfts)
    {
        viewMode = "launch";
        Launch = nfts;
    }
    public void SetAttend(List<NFTInfo> nfts)
    {
        viewMode = "attend";
        Attend = nfts;
    }

    private void ResetComingSushiInstance()
    {
        foreach (GameObject sushiInstance in sushiInstances) Destroy(sushiInstance);
        sushiInstances.Clear();
        float sushiInterval = 1.5f;
        for(int i=0; i<Displayed.Count; i++){
            Vector3 spawnPosition = new Vector3(-0.3f, -0.75f, sushiInterval * (1.5f-i) - 2f - 9.25f);
            GameObject sushiInstance = Instantiate(sushiPrefab, spawnPosition, Quaternion.identity);
            Sushi sushiComponent = sushiInstance.GetComponent<Sushi>();
            NFTInfo nftInfo = Displayed[i];
            sushiComponent.Id = nftInfo.Id;
            sushiComponent.Name = nftInfo.Name;
            sushiComponent.Author = nftInfo.Author;
            sushiComponent.CreateTime = nftInfo.CreateTime.ToString("yyyy/MM/dd, h:mm tt", new System.Globalization.CultureInfo("en-US"));
            sushiComponent.IsMinted = nftInfo.IsMinted;
            // sushiComponent.OwnerID = nftInfo.OwnerID;
            sushiComponent.viewMode = viewMode;

            sushiInstances.Add(sushiInstance);

            Transform ballSlot = sushiInstance.transform.Find("ball");
            ballSlot.GetComponent<Renderer>().enabled = false;
            ballSlot.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            nftDisplayer.ShowOneNFT(ballSlot, nftInfo.Contents);
        }
        isComing = true;
        moveStartTime = Time.time;
    }


    private void say(string s){
        bool debugging = true;
        if(debugging)Debug.Log(s);
    }
    
    private void rsay(){
            for(int i=0; i<Displayed.Count;i++)Debug.Log("Display " + i + " " + Displayed[i].Id);
            Debug.Log(sushiInstances.Count);
            Debug.Log(viewNumber);
        }

    public bool PreviewNonMintedNFT()
    {
        string email = PlayerPrefs.GetString("Email");
        Debug.Log(email);
        var nfts = BackendCommunicator.instance.FindOnePlayerByEmail(email).NFTs.Where(nft => (!nft.IsMinted && !nft.IsPending));
        if(nfts.Count() > 0)
        {
            SetMint(nfts.ToList());
            Debug.Log("Hello");
            foreach(var nft in nfts)
            {
                Debug.Log(nft.Owner);
            }
            return true;
        }
        else
        {
            Debug.LogError("You have no unminted NFTs!");
            return false;
        }
    }
    public bool PreviewMintedNFT(string method)
    {
        string email = PlayerPrefs.GetString("Email");
        var nfts = BackendCommunicator.instance.FindOnePlayerByEmail(email).NFTs.Where(nft => nft.IsMinted && !nft.IsPending);
        if (nfts.Count() > 0)
        {
            if(method == "transfer")
            {
                SetTransfer(nfts.ToList());
            }
            else if (method == "launch")
            {
                SetLaunch(nfts.ToList());
            }
            else
            {
                Debug.Log("Invalid command");
                return false;
            }
            return true;
        }
        else
        {
            Debug.Log("You have no minted NFTs!");
            return false;
        }
    }
    public bool PreviewActiveAuctions()
    {
        List<Auction> activeAuctions = BackendCommunicator.instance.FindActiveAuctions();
        if (activeAuctions.Count() > 0)
        {
            SetAttend(activeAuctions.Select(auction => auction.NFT).ToList());
            return true;
        }
        else
        {
            Debug.LogError("There are no active auctions!");
            return false;
        }
    }

    public async Task<NFTStatus> CheckBalanceAndMint(int _id)
    {
        try
        {
            string method = "balanceOf";

            var provider = new JsonRpcProvider(ContractManager.RPC);

            Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);

            var data = await contract.Call(method, new object[]
            {
                PlayerPrefs.GetString("Account")
            });


            BigInteger balanceOf = BigInteger.Parse(data[0].ToString());
            BigInteger realPrice = (BigInteger)1000000000000000000 * price;
            Debug.Log("Balance Of: " + balanceOf);
            Debug.Log("Price:" + realPrice);
            if (balanceOf < realPrice)
            {
                Debug.Log("Your balance is NOT enough!");
                return NFTStatus.Failure;
            }
            else
            {
                string[] preJsonData = { "mint", PlayerPrefs.GetString("Email"), _id.ToString() };
                string jsonData = JsonConvert.SerializeObject(preJsonData);
                string signature = await Web3Wallet.Sign(jsonData);
                string[] messageObj = { jsonData, signature };
                string message = JsonConvert.SerializeObject(messageObj);
                Debug.Log(message);
                // SendMessageRequest(message);
                return NFTStatus.Success;
            }
        }
        catch
        {
            return NFTStatus.ContractError;
        }
    }


    public async Task<NFTStatus> CheckBalanceAndTransfer(string toEmail, int _id)
    {
        string method = "balanceOf";

        var provider = new JsonRpcProvider(ContractManager.RPC);

        Contract contract = new Contract(ContractManager.TokenABI, ContractManager.TokenContract, provider);
        Contract NFTcontract = new Contract(ContractManager.NFTABI, ContractManager.NFTContract, provider);
        try
        {
            var data = await contract.Call(method, new object[]
            {
                PlayerPrefs.GetString("Account")
            });

            BigInteger nonce = rnd.Next();
            BigInteger balanceOf = BigInteger.Parse(data[0].ToString());
            BigInteger realFee = (BigInteger)1000000000000000000 * fee;
            Debug.Log("Balance Of: " + balanceOf);
            Debug.Log("Fee:" + realFee);
            if (balanceOf < realFee)
            {
                Debug.Log("Your balance is NOT enough!");
                return NFTStatus.Failure;
            }
            else
            {
                method = "getTransferPreSignedHash";
                string toAccount = BackendCommunicator.instance.FindOnePlayerByEmail(toEmail).Account; 
                string[] preJsonData = { "transfer", PlayerPrefs.GetString("Email"), toEmail, _id.ToString(), nonce.ToString() };
                string jsonData = JsonConvert.SerializeObject(preJsonData);
                data = await NFTcontract.Call(method, new object[]
                {
                    PlayerPrefs.GetString("Account"),
                    toAccount,
                    _id.ToString(),
                    nonce.ToString()
                });
                var result_hash = BitConverter.ToString((byte[])data[0]).Replace("-", string.Empty).ToLower();
                string signature = await Web3Wallet.Sign(result_hash);
                string[] messageObj = { jsonData, result_hash, signature };
                string message = JsonConvert.SerializeObject(messageObj);
                Debug.Log(message);
                // SendMessageRequest(message);
                return NFTStatus.Success;
            }
        }
        catch
        {
            return NFTStatus.ContractError;
        }
    }
    private void ChainSafeSetup()
    {
        // change this if you are implementing your own sign in page
        Web3Wallet.url = "https://chainsafe.github.io/game-web3wallet/";
        // loads the data saved from the editor config
        ProjectConfigScriptableObject projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
        PlayerPrefs.SetString("ChainID", projectConfigSO.ChainID);
        PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        PlayerPrefs.SetString("Network", projectConfigSO.Network);
        PlayerPrefs.SetString("RPC", projectConfigSO.RPC);
        PlayerPrefs.SetString("Account", "0xC79dbE9296E54e5C503Bd1820eE5dAC6376c98C5");
    }
    void Awake(){
        // log function, delay time, repeat interval        
        // InvokeRepeating("rsay", 0.0f, 1.0f);
        ChainSafeSetup();
        // PlayerPrefs.SetString("Email", "rakechen168@gmail.com");// For test
        dialogButton.deactivateAllInputFields();
    }

}
public enum NFTStatus
{
    Success,
    Failure,
    ContractError
};