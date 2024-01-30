using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<NFTInfo> Launch = new List<NFTInfo>();
    public List<NFTInfo> Attend = new List<NFTInfo>();

    private List<GameObject> sushiInstances = new List<GameObject>();
    private float moveStartTime;

    public void SetDisplayed(int num, string mode)
    {
        if(isLeaving || isComing) return;

        Displayed.Clear();
        if(viewMode == "mint") {for(int i=num; i<num+4; i++) if(i<Mint.Count) Displayed.Add(Mint[i]);}
        else if(viewMode == "transfer") {for(int i=num; i<num+4; i++) if(i<Transfer.Count) Displayed.Add(Transfer[i]);}
        else if(viewMode == "launch") {for(int i=num; i<num+4; i++) if(i<Launch.Count) Displayed.Add(Launch[i]);}
        else if(viewMode == "attend") {for(int i=num; i<num+4; i++) if(i<Attend.Count) Displayed.Add(Attend[i]);}

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
    void Start(){
        // log function, delay time, repeat interval        
        // InvokeRepeating("rsay", 0.0f, 1.0f);
    }

}
