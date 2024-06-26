using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueButtons : MonoBehaviour
{
    public Button talkButton;
    public Button shopButton;
    public Button giveButton;
    public Button backButton;
    private bool leaveButton = true;
    private TextMeshProUGUI talkText;
    private TextMeshProUGUI shopText;
    private TextMeshProUGUI giveText;
    private TextMeshProUGUI backText;

    private void Start()
    {
        talkText = talkButton.GetComponentInChildren<TextMeshProUGUI>();
        shopText = shopButton.GetComponentInChildren<TextMeshProUGUI>();
        giveText = giveButton.GetComponentInChildren<TextMeshProUGUI>();
        backText = backButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public int GetButtonCommand()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null)
        {
            if (selected == talkButton.gameObject) return 0;
            else if (selected == shopButton.gameObject) return 1;
            else if (selected == giveButton.gameObject) return 2;
            else if (selected == backButton.gameObject) return 3;
        }
        return 0;
    }

    public void SetButtonsActive(bool value)
    {
        talkButton.interactable = value;
        shopButton.interactable = value;
        giveButton.interactable = value;
        backButton.interactable = value;
    }
}
