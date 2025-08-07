using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipFade : MonoBehaviour
{
    public float timeDelay;
    public bool active = true;
    public string input;
    private float timeDestination;
    [SerializeField] private Animator animator;
    [SerializeField] private bool freezePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        if (freezePlayer) FindObjectOfType<PlayerMovement>().Freeze("tutorial");
        timeDestination = Time.time + timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Time.time >= timeDestination)
        {
            animator.SetBool("visible", true);
        }
        if (InputIs(input))
        {
            ForceFadeOut();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("(nothing)") && !active)
        {
            Destroy(gameObject);
        }
    }

    bool InputIs(string target)
    {
        if (target == "Horizontal" || target == "Vertical")
        {
            return Input.GetAxisRaw(target) != 0;
        } else
        {
            return Input.GetButton(target);
        }
    }

    public void ForceFadeOut()
    {
        if (freezePlayer) FindObjectOfType<PlayerMovement>().Unfreeze("tutorial");
        active = false;
        animator.SetBool("visible", false);
    }
}
