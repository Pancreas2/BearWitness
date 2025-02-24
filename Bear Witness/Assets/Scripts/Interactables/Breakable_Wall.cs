using UnityEngine;
using UnityEngine.Events;

public class Breakable_Wall : ReceiveDamage
{
    public GameObject stateOne;
    public GameObject stateTwo;
    public GameObject stateThree;
    public Animator animator;
    private int wallState = 3;
    private float invTime = 0f;
    readonly float maxHealth = 9f;
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

    public override void Damage(int damage, float sourceX = 0f)
    {
        if (Time.time >= invTime)
        {
            if (wallState <= 2)
            {
                Destroy(stateOne);
            }
            if (wallState <= 1)
            {
                Destroy(stateTwo);
            }
            if (currentHealth <= 0)
            {
                Die();
            }
            animator.SetTrigger("Hit");
            currentHealth -= damage;
            wallState = Mathf.CeilToInt(currentHealth * 3 / maxHealth);
            invTime = Time.time + 0.1f;
            Debug.Log(currentHealth);
        }
    }
    public void Die()
    {
        OnBreakEvent.Invoke();
        Destroy(gameObject);
    }
}
