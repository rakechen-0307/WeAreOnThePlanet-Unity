using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogButton : MonoBehaviour
{

    public TMP_InputField inputTransfer2;  
    public TMP_InputField inputAuctionStartingPrice;
    public TMP_InputField inputAuctionStartingTime;
    public TMP_InputField inputAuctionEndingTime;
    
    public SushiManager sushiManager;

    void Awake()
    {
        deactivateAllInputFields();
    }

    public void deactivateAllInputFields(){
        inputTransfer2.gameObject.SetActive(false);
        inputAuctionStartingPrice.gameObject.SetActive(false);
        inputAuctionStartingTime.gameObject.SetActive(false);
        inputAuctionEndingTime.gameObject.SetActive(false);
    }

    async void OnMouseDown()
    {
        if (UIManager.Instance.State == "mint1")
        {
            // try minting NFT
            Debug.Log(SushiManager.selected); // other properties can be found likewise
            NFTStatus result = await sushiManager.CheckBalanceAndMint(SushiManager.selected);
            // Handle errors
            if (result == NFTStatus.Success)
            {
                // If succeeded
                UIManager.Instance.UpdateDialog("mint2", "\n\nYour NFT has been successfully minted!");
                BackendCommunicator.instance.UpdateNFTStatus(SushiManager.selected, true);
            }
            else if (result == NFTStatus.Failure)
            {
                UIManager.Instance.UpdateDialog("mint2", "\n\nYou don't have enough tokens!");
            }
            else
            {
                UIManager.Instance.UpdateDialog("mint2", "\n\nSome errors occurred. Please try again.");
            }
        }

        
        else if (UIManager.Instance.State == "transfer1")
        {
            inputTransfer2.gameObject.SetActive(true);
            inputTransfer2.interactable = true;
            inputTransfer2.ActivateInputField();
            UIManager.Instance.UpdateDialog("transfer2", "Who would you like to transfer this NFT to?\nPlease enter their email:\n");
        }
        else if (UIManager.Instance.State == "transfer2")
        {
            // try transfering NFT
            Debug.Log(SushiManager.selected);
            Debug.Log(inputTransfer2.text);
            // Handle errors
            NFTStatus result = await sushiManager.CheckBalanceAndTransfer(inputTransfer2.text, SushiManager.selected);
            // Handle errors
            if (result == NFTStatus.Success)
            {
                // If succeeded
                UIManager.Instance.UpdateDialog("transfer3", "\n\nYour NFT has been successfully transferred!");
                BackendCommunicator.instance.UpdateNFTStatus(SushiManager.selected, true);
            }
            else if (result == NFTStatus.Failure)
            {
                UIManager.Instance.UpdateDialog("transfer3", "\n\nYou don't have enough tokens!");
            }
            else
            {
                UIManager.Instance.UpdateDialog("transfer3", "\n\nSome errors occurred. Please try again.");
            }

            // If succeeded            
            deactivateAllInputFields();
        }


        else if (UIManager.Instance.State == "launch1")
        {       
            inputAuctionStartingPrice.gameObject.SetActive(true);
            inputAuctionStartingTime.gameObject.SetActive(true);
            inputAuctionEndingTime.gameObject.SetActive(true);
            inputAuctionStartingPrice.interactable = true;
            inputAuctionStartingTime.interactable = true;
            inputAuctionEndingTime.interactable = true;
            inputAuctionStartingPrice.ActivateInputField();
            UIManager.Instance.UpdateDialog("launch2", "Please enter the information of the auction.\n\nStarting Price:\n\n" +
                "From(UTF+0):\n\nTo(UTF+0):"
            );
        }
        else if (UIManager.Instance.State == "launch2")
        {
            // try launching NFT
            // Handle errors


            // If succeeded            
            deactivateAllInputFields();
            UIManager.Instance.UpdateDialog("launch3", "\n\nThe auction has been successfully launched!");
        }


        else if (UIManager.Instance.State == "attend1")
        {       
            // try entering auction
            // Handle errors
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
