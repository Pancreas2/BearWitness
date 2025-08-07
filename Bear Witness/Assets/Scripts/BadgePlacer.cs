using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BadgePlacer : MonoBehaviour
{
    [SerializeField] private BadgeSlot circleSlot;
    [SerializeField] private BadgeSlot squarePrioritySlot;
    [SerializeField] private BadgeSlot trianglePrioritySlot;
    [SerializeField] private BadgeSlot squareAltSlot;
    [SerializeField] private BadgeSlot triangleAltSlot;

    [SerializeField] private BadgeSlot currentlyHeldBadge;
    private bool blockBadgePlacement = true;

    private List<BadgeSlot> appliedBadges = new();

    public void Place(Item badge)
    {
        if (badge.type == Item.ItemType.CircleBadge)
        {
            circleSlot.SetBadge(badge);
            GameManager.instance.currentBadges[0] = badge.name;
        } else if (badge.type == Item.ItemType.SquareBadge)
        {
            if (squarePrioritySlot.NoBadgeSet())
            {
                squarePrioritySlot.SetBadge(badge);
            } else if (squareAltSlot.NoBadgeSet())
            {
                squareAltSlot.SetBadge(badge);
            } else
            {
                // allow choice of badge removal?
                // actually I should always allow this
            }
        }
    }

    public void HoldBadge(BadgeSlot badgeSlot)
    {
        blockBadgePlacement = false;
        currentlyHeldBadge = badgeSlot;
    }

    public BadgeSlot GetHeldBadge()
    {
        return currentlyHeldBadge;
    }

    public void VoidBadge()
    {
        blockBadgePlacement = true;
        currentlyHeldBadge = null;
    }

    public void AttemptPlace(BadgeSlot badgeSlot)
    {
        if (blockBadgePlacement || currentlyHeldBadge.badgeType == badgeSlot.badgeType)
        {
            Item replacedBadge = badgeSlot.GetBadge();
            BadgeSlot replacedSlot = appliedBadges.Find(target => target.GetBadge() == replacedBadge);
            if (replacedSlot)
            {
                appliedBadges.Remove(replacedSlot);
                replacedSlot.SetGreyOut(false);
            }

            if (!blockBadgePlacement) {

                badgeSlot.SetBadge(currentlyHeldBadge.GetBadge());
                appliedBadges.Add(currentlyHeldBadge);
                currentlyHeldBadge.SetGreyOut(true);
            } else
            {
                badgeSlot.VoidBadge();
            }
        }
    }

    public void SetBadgeConfiguration()
    {
        List<string> newBadges = new();

        newBadges.Add(circleSlot.GetBadgeName());
        newBadges.Add(squarePrioritySlot.GetBadgeName());
        newBadges.Add(squareAltSlot.GetBadgeName());
        newBadges.Add(trianglePrioritySlot.GetBadgeName());
        newBadges.Add(triangleAltSlot.GetBadgeName());

        GameManager.instance.currentBadges = newBadges;

        bool allSlotsFilled = true;
        foreach (string badge in newBadges)
        {
            if (badge == "") allSlotsFilled = false;
        }

        if (allSlotsFilled) GameManager.instance.GrantAchievement("Fully Charged");
    }
}
