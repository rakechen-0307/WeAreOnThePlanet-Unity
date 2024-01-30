using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogButton : MonoBehaviour
{

    public TMP_InputField inputTransfer2;  
    
    public SushiManager sushiManager;

    void Awake()
    {
        deactivateAllInputFields();
    }

    public void deactivateAllInputFields(){
        inputTransfer2.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (UIManager.Instance.State == "mint1")
        {
            // try minting NFT
            Debug.Log(sushiManager.Mint[sushiManager.viewNumber].Id); // other properties can be found likewise
            // Handle errors


            // If succeeded
            UIManager.Instance.UpdateDialog("mint2", "Your NFT has been successfully minted!");
        }

        
        else if (UIManager.Instance.State == "transfer1")
        {
            inputTransfer2.gameObject.SetActive(true);
            inputTransfer2.interactable = true;
            inputTransfer2.ActivateInputField();
            UIManager.Instance.UpdateDialog("transfer2", "Who would you like to transfer this NFT to?\nPlease enter their ID:\n");
        }
        else if (UIManager.Instance.State == "transfer2")
        {
            // try transfering NFT
            Debug.Log(sushiManager.Transfer[sushiManager.viewNumber].Id);
            Debug.Log(inputTransfer2.text);
            // Handle errors


            // If succeeded            
            inputTransfer2.gameObject.SetActive(false);
            inputTransfer2.interactable = false;
            UIManager.Instance.UpdateDialog("transfer3", "Your NFT has been successfully transferred!");
        }

        else 
        {
            UIManager.Instance.UpdateDialog("none", "");
        }
    }

    private void say(string s){
        bool debugging = true;
        if(debugging)Debug.Log(s);
    }
}
