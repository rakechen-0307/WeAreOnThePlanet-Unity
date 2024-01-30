using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right : MonoBehaviour
{
    public SushiManager sushiManager;
    void OnMouseDown(){
        Debug.Log("Right page clicked");
        if(sushiManager.viewMode == "none") return;
        int num = sushiManager.viewNumber;
        if(num + 4 < sushiManager.Mint.Count) sushiManager.SetDisplayed(num + 4, sushiManager.viewMode);
        UIManager.Instance.UpdateDialog("none", "");
    }
}
