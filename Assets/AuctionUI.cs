using System;
using TMPro;
using UnityEngine;
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
    private TMP_Text remainingTime;

    [SerializeField]
    private TMP_InputField bidInput;

    [SerializeField]
    private Button bidButton;

    [SerializeField]
    private Button backButton;

    private void getAuctionInfo()
    {
        Auction auction = BackendCommunicator.instance.FindAuctionByNFTId(loadedData.attendingAuctionNFTId);
        string startPrice = auction.StartPrice.ToString();
        // string startTime = auction.StartTime.ToString("yyyy/MM/dd, h:mm tt", new System.Globalization.CultureInfo("en-US")) + " (UTF+0)";
        // string endTime = auction.EndTime.ToString("yyyy/MM/dd, h:mm tt", new System.Globalization.CultureInfo("en-US")) + " (UTF+0)";
        DateTimeOffset currentTime = DateTimeOffset.Now;
        DateTimeOffset endTime = auction.EndTime;
        TimeSpan timeInterval = endTime - currentTime;
        string ownerId = auction.Owner.Id.ToString();
        string nftID = loadedData.attendingAuctionNFTId.ToString();
        string createdTime = auction.NFT.CreateTime.ToString("yyyy/MM/dd, h:mm tt", new System.Globalization.CultureInfo("en-US")) + " (UTF+0)";
        string creator = auction.NFT.Author;
        string highestPrice = auction.BidPrice.ToString();
        string highestBidderName = auction.BidPlayer.Username;

        nameTitle.text = auction.Owner.Username;
        infoText.text = $"ID: {nftID}\nOwner: {ownerId}\nCreator: {creator}\nCreated Time: {createdTime}";
        highestBid.text = highestPrice;
        highestBidder.text = highestBidderName;
        // remainingTime.text = 
    }
}
