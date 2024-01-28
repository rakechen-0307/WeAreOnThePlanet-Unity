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
    public int Id;
    public string Name;
    // OwnerID
    // Content

    public string viewMode;

    void OnMouseDown()
    {
        say("Sushi clicked!");
        say(Id.ToString());
        if(viewMode == "mint"){
            UIManager.Instance.UpdateText("Would you like to mint this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + Id + "\n" +
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
