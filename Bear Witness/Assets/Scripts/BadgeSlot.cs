using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeSlot : MonoBehaviour
{
    public Item.ItemType badgeType = Item.ItemType.CircleBadge;
    [SerializeField] private Item heldBadge;
    [SerializeField] private BadgePlacer badgePlacer;
    [SerializeField] private Image image;
    private bool badgeOwned = false;

    public void AcceptBadge()
    {
        badgePlacer.AttemptPlace(this);
        badgePlacer.VoidBadge();
    }

    public void StoreBadge()
    {
        if (badgeOwned)
        {

            if (GameManager.instance.currentBadges.Contains(heldBadge.name))
            {
                badgePlacer.VoidBadge();
                SetGreyOut(false);
            } else
            {
                badgePlacer.HoldBadge(this);
            }

        }
    }

    public void Reload()
    {
        if (heldBadge)
        {
            badgeOwned = GameManager.instance.foundBadges.Contains(heldBadge.name);
            image.sprite = heldBadge.image;
        }

        image.enabled = badgeOwned;
    }

    public Item GetBadge()
    {
        return heldBadge;
    }

    public string GetBadgeName()
    {
        if (heldBadge) return heldBadge.name;
        else return "";
    }

    public bool BadgeIsOwned()
    {
        return badgeOwned;
    }

    public void SetBadge(Item badge)
    {
        heldBadge = badge;
        badgeOwned = GameManager.instance.foundBadges.Contains(heldBadge.name);
        image.sprite = heldBadge.image;
    }

    public void VoidBadge()
    {
        heldBadge = null;
        image.sprite = Resources.Load<Sprite>("null_image");
    }

    public bool NoBadgeSet()
    {
        return heldBadge == null;
    }

    public void SetGreyOut(bool value)
    {
        if (value)
        {
            image.color = Color.gray;
        } else
        {
            image.color = Color.white;
        }
    }
}
