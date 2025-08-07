using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BadgeDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Sprite defaultBadge;

    private void Update()
    {
        EventSystem.current.currentSelectedGameObject.TryGetComponent<BadgeSlot>(out BadgeSlot badgeSlot);
        if (badgeSlot)
        {
            if (badgeSlot.BadgeIsOwned())
            {
                Item badge = badgeSlot.GetBadge();
                image.sprite = badge.image;
                nameText.text = badge.name;
                descriptionText.text = badge.description;
            } else
            {
                image.sprite = defaultBadge;
                nameText.text = "Badge Name";
                descriptionText.text = "Badge Description";
            }
        }
    }
}
