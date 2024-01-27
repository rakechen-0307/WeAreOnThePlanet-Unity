using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextChatManager : MonoBehaviour
{
    private IList<MessageObjectUI> _message = new List<MessageObjectUI>();

    public GameObject messageObj, ChatRoomObj;
    public Button sendButton;
    public TMP_InputField textInput;

    // Start is called before the first frame update
    void Start()
    {
        sendButton.onClick.AddListener(() =>
        {
            if (textInput.text != null)
            {
                Debug.Log(textInput.text);
                AddMessage(textInput.text);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void AddMessage(string message)
    {
        var newMes = Instantiate(messageObj, ChatRoomObj.transform);
        var newMessageTextObj = newMes.GetComponent<MessageObjectUI>();

        newMessageTextObj.MessageText.text = message;

        _message.Add(newMessageTextObj);
    }
}
