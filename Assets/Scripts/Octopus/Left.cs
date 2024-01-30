using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour
{
    public SushiManager sushiManager;
    void OnMouseDown(){
        UIManager.Instance.DeleteAllInputFields();
        Debug.Log("Left page clicked");
        if(sushiManager.viewMode == "none") return;
        int num = sushiManager.viewNumber;
        if(num - 4 >= 0) sushiManager.SetDisplayed(num - 4, sushiManager.viewMode);
        UIManager.Instance.UpdateDialog("none", "");
    }
}
