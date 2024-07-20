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
            ChangeRooms();
        }

        if (active)
        {
            if (collision.collider.CompareTag("Player"))
            {
                if (animator)
                    animator.SetBool("open", true);
                if (isFrontDoor && interactText)
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
            if (animator)
                animator.SetBool("open", false);
            if (isFrontDoor && interactText)
                interactText.SetBool("playerInRange", false);
        }
    }

    virtual public void ChangeRooms()
    {
        FindObjectOfType<PlayerMovement>().frozen = true;
        FindObjectOfType<LevelLoader>().LoadNextLevel(destination);
    }
}
