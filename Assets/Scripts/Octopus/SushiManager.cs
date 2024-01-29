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
using UnityEditor.VersionControl;
using Web3Unity.Scripts.Library.Web3Wallet;
using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Contracts;

public class SushiManager : MonoBehaviour
{
    
    public GameObject sushiPrefab;

    public string viewMode = "none";
    public int viewNumber = 0;
    private bool isLeaving = false;
    private bool isComing = false;

    public List<NFTInfo> Displayed = new List<NFTInfo>();
    public List<NFTInfo> Mint = new List<NFTInfo>();
    public List<NFTInfo> Transfer = new List<NFTInfo>();

    private List<GameObject> sushiInstances = new List<GameObject>();
    private float moveStartTime;

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    private string _realmAppID = "weareontheplanet-ouawh";

    public static BigInteger price = 100;

    public void SetDisplayed(int num, string mode)
    {
        if(isLeaving || isComing) return;

        Displayed.Clear();
        if(viewMode == "mint") for(int i=num; i<num+4; i++) if(i<Mint.Count) Displayed.Add(Mint[i]);
        // TODO - other modes

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

    private void ResetComingSushiInstance()
    {
        foreach (GameObject sushiInstance in sushiInstances) Destroy(sushiInstance);
        sushiInstances.Clear();
        float sushiInterval = 1.5f;
        for(int i=0; i<Displayed.Count; i++){
            Vector3 spawnPosition = new Vector3(-0.3f, -0.75f, sushiInterval * (1.5f-i) - 2f - 9.25f);
            GameObject sushiInstance = Instantiate(sushiPrefab, spawnPosition, Quaternion.identity);
            sushiInstance.tag = "sushi";
            Sushi sushiComponent = sushiInstance.GetComponent<Sushi>();
            NFTInfo nftInfo = Displayed[i];
            sushiComponent.Id = nftInfo.Id;
            sushiComponent.Name = nftInfo.Name;
            sushiComponent.Author = nftInfo.Author;
            sushiComponent.CreateTime = nftInfo.CreateTime;
            sushiComponent.IsMinted = nftInfo.IsMinted;
            // sushiComponent.OwnerID = nftInfo.OwnerID;
            sushiComponent.viewMode = viewMode;

            sushiInstances.Add(sushiInstance);
        }
        isComing = true;
        moveStartTime = Time.time;
    }


    private void say(string s){
        bool debugging = false;
        if(debugging)Debug.Log(s);
    }
    
    private void rsay(){
            for(int i=0; i<Displayed.Count;i++)Debug.Log("Display " + i + " " + Displayed[i].Id);
            Debug.Log(sushiInstances.Count);
            Debug.Log(viewNumber);
        }

    public void PreviewNonMintedNFT()
    {
        string email = PlayerPrefs.GetString("Email");
        PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == email).FirstOrDefault();
        var nfts = findPlayer.NFTs.Where(nft => nft.IsMinted = false);
        if(nfts.Count() > 0)
        {
            SetMint(nfts.ToList()); 
        }
        else
        {
            Debug.LogError("You have no unminted NFTs!");
        }
    }
    public void PreviewMintedNFT()
    {
        string email = PlayerPrefs.GetString("Email");
        PlayerData findPlayer = _realm.All<PlayerData>().Where(user => user.Email == email).FirstOrDefault();
        var nfts = findPlayer.NFTs.Where(nft => nft.IsMinted = true);
        if (nfts.Count() > 0)
        {
            SetMint(nfts.ToList());
        }
        else
        {
            Debug.Log("You have no minted NFTs!");
        }
    }

    private async Task<bool> CheckBalanceAndMint(int _id)
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
            return false;
        }
        else
        {
            string[] preJsonData = { "mint", PlayerPrefs.GetString("Account"), _id.ToString() };
            string jsonData = JsonConvert.SerializeObject(preJsonData);
            string signature = await Web3Wallet.Sign(jsonData);
            string[] messageObj = { jsonData, signature };
            string message = JsonConvert.SerializeObject(messageObj);
            Debug.Log(message);
            // SendMessageRequest(message);
            return true;
        }
    }

    private async void RealmSetup()
    {
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
    }
    void Start(){
        // log function, delay time, repeat interval        
        // InvokeRepeating("rsay", 0.0f, 1.0f);
        ChainSafeSetup();
        RealmSetup();
    }

}
