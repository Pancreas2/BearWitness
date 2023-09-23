using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookForwardForPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float noticeDistance = 4f;
    [SerializeField] private LayerMask raycastLayers;
    [SerializeField] private Transform eyePoint;
    private bool seesPlayer = false;

    public UnityEvent OnSee;
    public UnityEvent OnNotSee;

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D sightline = Physics2D.Raycast(eyePoint.transform.position, new Vector2(eyePoint.transform.position.x + noticeDistance, eyePoint.transform.position.y), noticeDistance, raycastLayers);
        if ((sightline && sightline.collider.CompareTag("Player")) ^ seesPlayer)
        {
            if (seesPlayer)
            {
                OnNotSee.Invoke();
                animator.SetBool("playerInRange", false);
            }
            else
            {
                OnSee.Invoke();
                animator.SetBool("playerInRange", true);
            }
            seesPlayer = !seesPlayer;
        }
    }
}
