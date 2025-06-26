using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBounce : MonoBehaviour
{
    private bool active = true;

    public void SetActive(bool value)
    {
        active = value;
    }

    public bool GetActive()
    {
        return active;
    }
}
