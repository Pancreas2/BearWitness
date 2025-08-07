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

    [Header("Thresholds (Inclusive)")]
    [SerializeField] private int min = 1;
    [SerializeField] private int max = 1;
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

        if (max < min)
        {
            max = min;
            Debug.LogError("FindLoopNumber bounds improperly set on object " + gameObject);
        }
    }

    public int GetNumber()
    {
        loopNumber = GameManager.instance.loopNumber;
        if (loopNumber >= min && loopNumber <= max)
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
