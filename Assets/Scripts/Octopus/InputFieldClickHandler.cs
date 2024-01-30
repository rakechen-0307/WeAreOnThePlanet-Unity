using UnityEngine;
using UnityEngine.EventSystems; // Required for Event Systems
using TMPro; // Required for TextMeshPro

public class InputFieldClickHandler : MonoBehaviour, IPointerClickHandler
{
    public TMP_InputField inputAuctionEndingTime; // Assign this in the Inspector

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        inputAuctionEndingTime.ActivateInputField();
    }
}
