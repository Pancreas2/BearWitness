using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FindLoopNumber : MonoBehaviour
{
    [SerializeField] private bool findOnStart = true;
    [SerializeField] private int modulo = 0;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string parameterName;

    [Header("Thresholds")]
    [SerializeField] private int threshold = 1;
    public UnityEvent OnLoopsHitThreshold;

    private int loopNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (findOnStart)
        {
            if (animator)
            {
                animator.SetInteger(parameterName, GetNumber());
            } else
                GetNumber();
        }
    }

    public int GetNumber()
    {
        loopNumber = GameManager.instance.loopNumber;
        if (loopNumber >= threshold)
        {
            OnLoopsHitThreshold.Invoke();
        }

        if (modulo == 0)
            return loopNumber;
        else
        {
            return loopNumber % modulo;
        }
    }
}
