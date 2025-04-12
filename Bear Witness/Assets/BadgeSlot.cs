using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeSlot : MonoBehaviour
{
    [SerializeField] private Item heldBadge;
    [SerializeField] private Image image;
    private bool badgeOwned = false;

    public void OnClick()
    {
        
    }

    public void Reload()
    {
        if (heldBadge)
        {
            badgeOwned = GameManager.instance.foundBadges.Contains(heldBadge);
            image.sprite = heldBadge.image;
        }

        image.enabled = badgeOwned;
    }

    public Item GetBadge()
    {
        return heldBadge;
    }

    public bool BadgeIsOwned()
    {
        return badgeOwned;
    }
}
