using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class Sushi : MonoBehaviour
{
    public string Author;
    public DateTimeOffset CreateTime;
    public bool IsMinted;
    public int NFTID;
    public string Name;
    public int OwnerID;
    // Content

    public string viewMode;

    void OnMouseDown()
    {
        say("Sushi clicked!");
        say(NFTID.ToString());
        if(viewMode == "mint"){
            UIManager.Instance.UpdateText("Would you like to mint this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + NFTID + "\n" +
                "Author: " + Author + "\n" +
                "Time of creation: " + CreateTime + "\n"
            );
        }

    }

    private void say(string s){
        bool debugging = true;
        if(debugging)Debug.Log(s);
    }
}
