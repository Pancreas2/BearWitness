using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookForPlayer : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private Animator animator;
    [SerializeField] private float noticeDistance = 4f;
    [SerializeField] private LayerMask raycastLayers;
    [SerializeField] private Transform eyePoint;
    private bool seesPlayer = false;
    Vector3 visibleDirection;

    [SerializeField] private float limitingAngle = 90f;

    public UnityEvent OnSee;
    public UnityEvent OnNotSee;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        visibleDirection = player.transform.position - eyePoint.transform.position;
        float angle = Vector2.Angle(visibleDirection, Vector2.right);
        if (angle > limitingAngle && 180 - angle > limitingAngle)
        {
            // limit angle to limitingAngle
            visibleDirection.y = Mathf.Tan(limitingAngle * Mathf.Deg2Rad) * Mathf.Sign(visibleDirection.y) * Mathf.Abs(visibleDirection.x);
        }
        RaycastHit2D sightline = Physics2D.Raycast(eyePoint.transform.position, visibleDirection, noticeDistance, raycastLayers);
        if ((sightline && sightline.collider.CompareTag("Player")) ^ seesPlayer)
        {
            if (seesPlayer)
            {
                OnNotSee.Invoke();
                animator.SetBool("playerInRange", false);
            } else
            {
                OnSee.Invoke();
                animator.SetBool("playerInRange", true);
            }
            seesPlayer = !seesPlayer;
        }
    }

    private void OnDrawGizmosSelected()
    {
        float angle = limitingAngle * Mathf.Deg2Rad;
        Gizmos.DrawLine(eyePoint.transform.position, eyePoint.transform.position + noticeDistance * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)));
        Gizmos.DrawLine(eyePoint.transform.position, eyePoint.transform.position + noticeDistance * new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle)));
    }
}
