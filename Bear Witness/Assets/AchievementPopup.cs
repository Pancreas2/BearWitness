using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPopup : MonoBehaviour
{
    private Queue<Achievement> achs = new();

    private float delay = 2.5f;
    private float activeTimer = 0f;

    [SerializeField] private AchievementDisplay display;
    [SerializeField] private Animator animator;

    public void QueueAchievement(Achievement ach)
    {
        achs.Enqueue(ach);
    }

    private void Update()
    {
        if (activeTimer <= Time.time && achs.Count > 0)
        {
            activeTimer = Time.time + delay;
            Achievement targetAch = achs.Dequeue();
            display.ChangeHeldAchievement(targetAch);
            animator.Play("popup");
        }
    }
}
