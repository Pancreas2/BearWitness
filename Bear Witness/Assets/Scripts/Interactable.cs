using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI text;
    public string interactText = "LOOK";

    private float cooldown;

    private void Start()
    {
        text.text = interactText;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) 
        {
            animator.SetBool("playerInRange", true);
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && Time.time >= cooldown)
            {
                cooldown = Time.time + 2f;
                OnInteract();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            animator.SetBool("playerInRange", false);
        }
    }

    virtual public void OnInteract() {
        Debug.Log("No interaction declared");
    }
}
