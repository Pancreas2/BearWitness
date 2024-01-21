using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public bool active = true;
    [SerializeField] private Animator animator;
    public bool isFrontDoor = false;
    [SerializeField] private string destination;
    [SerializeField] private Animator interactText;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isFrontDoor && collision.collider.CompareTag("Player") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            FindObjectOfType<PlayerMovement>().frozen = true;
            FindObjectOfType<LevelLoader>().LoadNextLevel(destination);
        }

        if (active)
        {
            if (collision.collider.CompareTag("Player"))
            {
                animator.SetBool("open", true);
                if (isFrontDoor)
                {
                    interactText.SetBool("playerInRange", true);
                }

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            animator.SetBool("open", false);
            if (isFrontDoor)
                interactText.SetBool("playerInRange", false);
        }
    }
}
