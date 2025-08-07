using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeTab : MonoBehaviour
{
    [SerializeField] private Selectable selectOnRight;
    [SerializeField] private Selectable thisSelectable;
    
    public void ValueChanged(bool value)
    {
        if (value)
        {
            Navigation customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnUp = thisSelectable.navigation.selectOnUp;
            customNav.selectOnDown = thisSelectable.navigation.selectOnDown;
            customNav.selectOnLeft = thisSelectable.navigation.selectOnLeft;

            customNav.selectOnRight = selectOnRight;
            thisSelectable.navigation = customNav;
        } else
        {
            Navigation customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnUp = thisSelectable.navigation.selectOnUp;
            customNav.selectOnDown = thisSelectable.navigation.selectOnDown;
            customNav.selectOnLeft = thisSelectable.navigation.selectOnLeft;

            thisSelectable.navigation = customNav;
        }
    }
}
