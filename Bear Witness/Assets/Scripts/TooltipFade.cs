using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipFade : MonoBehaviour
{
    public float timeDelay;
    public bool active = true;
    public List<KeyCode> keyCodes;
    private float timeDestination;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        timeDestination = Time.time + timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Time.time >= timeDestination)
        {
            animator.SetBool("visible", true);
        }
        if (AnyKeyPressed(keyCodes))
        {
            ForceFadeOut();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("(nothing)") && !active)
        {
            Destroy(gameObject);
        }
    }

    bool AnyKeyPressed(List<KeyCode> keys)
    {
        foreach (KeyCode key in keys) {
            if (Input.GetKey(key)) return true;
        }
        return false;
    }

    public void ForceFadeOut()
    {
        animator.SetBool("visible", false);
        active = false;
    }
}
