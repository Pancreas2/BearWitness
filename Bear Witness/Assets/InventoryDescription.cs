using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryDescription : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    private GameObject selected;

    private void Start()
    {
        icon.enabled = false;
    }
    private void Update()
    {
        if (selected != EventSystem.current.currentSelectedGameObject)
        {
            selected = EventSystem.current.currentSelectedGameObject;
            if (selected)
            {
                selected.TryGetComponent(out InventorySlot slot);
                if (slot && slot.name != null)
                {
                    Debug.Log(slot);
                    nameText.color = Color.white;
                    descText.color = Color.white;
                    if (icon.sprite || slot.heldItem.icon) icon.enabled = true;
                    icon.sprite = slot.heldItem.icon;
                    nameText.text = slot.heldItem.name;
                    descText.text = slot.heldItem.desc;
                } else if (!icon.sprite)
                {
                    icon.enabled = false;
                }
            } else if (!icon.sprite)
            {
                icon.enabled = false;
            }
        }
    }
}
