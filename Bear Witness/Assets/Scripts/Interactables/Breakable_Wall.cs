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

    public bool allowHitFromLeft = true;
    public bool allowHitFromRight = true;

    private void Awake()
    {
        if (OnBreakEvent == null)
            OnBreakEvent = new UnityEvent();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public override void Damage(int damage, float sourceX = 0f, bool bypassInv = false)
    {
        if (!allowHitFromLeft && sourceX < transform.position.x) return;
        if (!allowHitFromRight && sourceX > transform.position.x) return;

        if (Time.time >= invTime || bypassInv)
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
            AudioManager.instance.Play("Hit", fadeTime: 0);
        }
    }
    public void Die()
    {
        OnBreakEvent.Invoke();
        Destroy(gameObject);
    }
}
