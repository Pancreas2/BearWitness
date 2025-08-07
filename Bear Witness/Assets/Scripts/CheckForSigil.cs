using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckForSigil : MonoBehaviour
{
    [SerializeField] private List<Gate.Gates> sigils;
    [SerializeField] private bool invert;

    public UnityEvent OnSigilMatch;

    void Start()
    {
        if (SigilsActive() ^ invert) OnSigilMatch.Invoke();
    }

    public void Refresh()
    {
        Start();
    }

    public void SetAnimBool(bool value)
    {
        GetComponent<Animator>().SetBool("SigilMatch", value);
    }

    private bool SigilsActive()
    {
        bool sigilsOpen = true;
        foreach (Gate.Gates sigil in sigils)
        {
            if (!GameManager.instance.doorStates[Gate.GateMatch[sigil]])
            {
                sigilsOpen = false;
                break;
            }
        }

        return sigilsOpen;
    }
}
