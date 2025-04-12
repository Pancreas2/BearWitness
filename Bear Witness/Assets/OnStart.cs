using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    public UnityEvent OnStartEvent;

    // Start is called before the first frame update
    void Start()
    {
        OnStartEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
