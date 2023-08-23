using UnityEngine;
using UnityEngine.Events;

public class Breakable_Wall : MonoBehaviour
{
    public GameObject stateOne;
    public GameObject stateTwo;
    public GameObject stateThree;
    public Animator animator;
    private int wallState = 3;
    private float invTime = 0f;
    float maxHealth = 3f;
    float currentHealth;

    public UnityEvent OnBreakEvent;

    private void Awake()
    {
        if (OnBreakEvent == null)
            OnBreakEvent = new UnityEvent();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int damage, bool skipInvTime = false)
    {
        if (Time.time >= invTime || skipInvTime)
        {
            animator.SetTrigger("Hit");
            currentHealth -= damage;
            wallState = Mathf.CeilToInt(currentHealth * 3 / maxHealth);
            if (wallState == 2)
            {
                Destroy(stateOne);
            }
            else if (wallState == 1)
            {
                Destroy(stateTwo);
            }
            if (currentHealth <= 0)
            {
                Die();
            }
            invTime = Time.time + 0.5f;
        }
    }
    public void Die()
    {
        OnBreakEvent.Invoke();
        Destroy(gameObject);
    }
}
