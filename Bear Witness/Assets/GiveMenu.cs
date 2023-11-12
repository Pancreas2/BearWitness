using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GiveMenu : MonoBehaviour
{
    public Button acceptButton;
    public Button declineButton;

    public GameObject itemArray;

    private GameObject memoryButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemChosen()
    {
        memoryButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(acceptButton.gameObject);
        acceptButton.interactable = true;
        declineButton.interactable = true;
    }

    public void AcceptGiveItem()
    {
        acceptButton.interactable = false;
        declineButton.interactable = false;
    }

    public void DeclineGiveItem()
    {
        EventSystem.current.SetSelectedGameObject(memoryButton);
        acceptButton.interactable = false;
        declineButton.interactable = false;
    }
}
