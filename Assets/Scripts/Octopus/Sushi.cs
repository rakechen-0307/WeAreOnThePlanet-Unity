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
        UIManager.Instance.DeleteAllInputFields();
        say("Sushi clicked!");
        say(Id.ToString());
        if(viewMode == "mint"){
            UIManager.Instance.UpdateDialog("mint1", "Would you like to mint this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + Id + "\n" +
                "Author: " + Author + "\n" +
                "Time of creation: " + CreateTime + "\n" +
                "This will cost 5 dollars."
            );
        }
        else if(viewMode == "transfer"){
            UIManager.Instance.UpdateDialog("transfer1", "Would you like to transfer this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + Id + "\n" +
                "Author: " + Author + "\n" +
                "Time of creation: " + CreateTime + "\n" +
                "This will cost 5 dollars."
            );
        }

    }

    private void say(string s){
        bool debugging = false;
        if(debugging)Debug.Log(s);
    }
}
