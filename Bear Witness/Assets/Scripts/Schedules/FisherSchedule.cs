using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FisherSchedule : NPCSchedule
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private float walkSpeed = 1.3f;
    private Vector3 referenceVel = Vector3.zero;

    private NPCActivity currentActivity;
    private GameManager gameManager;


    private IEnumerator WalkToLocation(Vector3 location)
    {
        Vector3 diff = location - spriteTransform.position;
        Debug.Log("walking");
        while (Mathf.Abs(diff.x) > 0.1f)
        {
            Debug.Log("walking");
            diff = location - spriteTransform.position;
            Vector3 targetVelocity = new(walkSpeed, m_Rigidbody2D.velocity.y, 0f);
            targetVelocity.x *= Mathf.Sign(diff.x);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref referenceVel, 0.05f);
            yield return new WaitForFixedUpdate();
        }
        spriteTransform.position = location;
        animator.SetBool("walking", false);
        m_Rigidbody2D.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {

    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void WalkToDock()
    {
        Vector3 location = new(-33.75f, -2f, 0f);
        StartCoroutine(WalkToLocation(location));
        animator.SetBool("walking", true);
    }
}
