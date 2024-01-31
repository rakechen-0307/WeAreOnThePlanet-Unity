using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class Sushi : MonoBehaviour
{
    public string Author;
    public string CreateTime;
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
        SushiManager.selected = Id;
        if(viewMode == "mint"){
            UIManager.Instance.UpdateDialog("mint1", "Would you like to mint this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + Id + "\n" +
                "Author: " + Author + "\n" +
                "Time of creation: " + CreateTime + " (UTF+0)\n" +
                "This will cost 100 WATP-Ts."
            );
        }
        else if(viewMode == "transfer"){
            UIManager.Instance.UpdateDialog("transfer1", "Would you like to transfer this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + Id + "\n" +
                "Author: " + Author + "\n" +
                "Time of creation: " + CreateTime + " (UTF+0)\n" +
                "This will cost 5 WATP-Ts."
            );
        }
        else if(viewMode == "launch"){
            UIManager.Instance.UpdateDialog("launch1", "Would you like to launch an auction of this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + Id + "\n" +
                "Author: " + Author + "\n" +
                "Time of creation: " + CreateTime + " (UTF+0)\n" +
                "This will cost 5 WATP-Ts."
            );
        }
        else if(viewMode == "attend"){
            UIManager.Instance.UpdateDialog("attend1", "Would you like to attend the auction of this NFT?\n" +
                "Name: " + Name + "\n" +
                "ID: " + Id + "\n" +
                "Author: " + Author + "\n" +
                "Time of creation: " + CreateTime + " (UTF+0)\n" +
                "This will cost 5 dollars."
            );
        }

    }

    private void say(string s){
        bool debugging = false;
        if(debugging)Debug.Log(s);
    }
}
