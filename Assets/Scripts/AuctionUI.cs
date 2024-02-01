using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuctionUI : MonoBehaviour
{
    [SerializeField]
    private LoadedData loadedData;

    [SerializeField]
    private TMP_Text nameTitle;

    [SerializeField]
    private TMP_Text infoText;

    [SerializeField]
    private TMP_Text highestBid;

    [SerializeField]
    private TMP_Text highestBidder;

    [SerializeField]
    private TMP_InputField bidInput;

    [SerializeField]
    private Button bidButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private AuctionManager auctionManager;

    private void Start()
    {
        getAuctionInfo();
        bidButton.onClick.AddListener(bidButtonOnClick);
        backButton.onClick.AddListener(backButtonOnClick);
    }
    private void getAuctionInfo()
    {
        Auction auction = BackendCommunicator.instance.FindAuctionByNFTId(loadedData.attendingAuctionNFTId);
        DateTimeOffset currentTime = DateTimeOffset.Now;
        DateTimeOffset endTime = auction.EndTime;
        TimeSpan timeInterval = endTime - currentTime;
        string timeIntervalText = $"{timeInterval.Days} days, {timeInterval.Hours}:{timeInterval.Minutes}";
        string ownerId = auction.Owner.Id.ToString();
        string nftID = loadedData.attendingAuctionNFTId.ToString();
        string createdTime = auction.NFT.CreateTime.ToString("yyyy/MM/dd, h:mm tt", new System.Globalization.CultureInfo("en-US")) + " (UTF+0)";
        string creator = auction.NFT.Author;
        string highestPriceText = auction.BidPrice.ToString();
        string highestBidderName = auction.BidPlayer != null ? auction.BidPlayer.Username : "Start Price";

        nameTitle.text = $"Auction Of {auction.Owner.Username}";
        infoText.text = $"ID: {nftID}\nOwner: {ownerId}\nCreator: {creator}\nCreated Time: {createdTime}\nAuction ends in {timeIntervalText}";
        highestBid.text = highestPriceText;
        highestBidder.text = highestBidderName;

        int highestPriceNumber = int.Parse(highestPriceText);
        bidInput.text = (highestPriceNumber + 1).ToString();
    }
    
    private async void bidButtonOnClick()
    {
        bool legit = int.TryParse(bidInput.text, out int bidPrice);
        Debug.Log("bidprice: " + bidPrice.ToString());
        int highestPrice = int.Parse(highestBid.text);
        if (!legit || bidPrice < highestPrice + 1)
        {
            bidInput.text = (highestPrice + 1).ToString();
            return;
        }
        bool result = await auctionManager.AuctionBid(loadedData.attendingAuctionNFTId, bidPrice);
        if(result)
        {
            getAuctionInfo();
            highestBid.text = bidInput.text;
            highestBidder.text = BackendCommunicator.instance.FindOnePlayerByEmail(PlayerPrefs.GetString("Email")).Username;
        }
    }

    private void backButtonOnClick()
    {
        SceneManager.LoadScene("Octopus");
    }
}
