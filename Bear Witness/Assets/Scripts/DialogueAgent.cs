using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueAgent : MonoBehaviour
{
    [SerializeField] private List<string> flags;
    [SerializeField] private float activationDelay = 0;
    public UnityEvent OnActivate;

    public void Activate(string receivedFlag)
    {
        if (flags.Contains(receivedFlag))
        {
            if (activationDelay <= 0)
            {
                OnActivate.Invoke();
            } else
            {
                StartCoroutine(WaitToActivate(activationDelay));
            }
        }
    }

    private IEnumerator WaitToActivate(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        OnActivate.Invoke();
    }
}
