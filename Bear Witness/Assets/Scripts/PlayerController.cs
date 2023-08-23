using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float m_CrouchSpeed = 1.5f;          // Amount of maxSpeed applied to rolling movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_AttackPoint; 
	[SerializeField] private Collider2D m_CrouchDisableCollider;     // A collider that will be disabled when rolling
	[SerializeField] private Animator animator;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	private GameManager gameManager;
	private float attackTime = 0f;
	public float attackRange = 0.4f;
	public LayerMask enemyLayers;
	private bool attacking = false;
	private bool specialing = false;
	private bool m_HasAirAttack = true;

	private bool m_wallClingState = false;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

    private void Start()
    {
		gameManager = FindFirstObjectByType<GameManager>();
    }

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
				{
					m_HasAirAttack = true;
					OnLandEvent.Invoke();
				}
			}
		}
	}

    private void Update()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
	}


    public void Move(float move, bool jump, bool run, bool roll)
	{
		animator.SetBool("run", run);
		// If rolling, check to see if the character can stand up
		if (!roll)
		{
			// If the character has a ceiling preventing them from standing up, keep them rolling
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				roll = true;
			}
		}

		animator.SetBool("roll", roll);

		//only control the player if grounded or airControl is turned on
		if ((m_Grounded || m_AirControl))
		{
			// If rolling
			if (roll)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the rollSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when rolling
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not rolling
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			Vector3 targetVelocity;

			if (attacking || specialing)
            {
				if (m_HasAirAttack)
                {
					// Move the character by finding the target velocity
					targetVelocity = Vector2.zero;
				} else
                {
					// Move the character by finding the target velocity
					targetVelocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
				}
			} else
            {
				// Move the character by finding the target velocity
				targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			}

			if (!m_wallClingState)
            {
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			} else if (m_Rigidbody2D.IsAwake())
            {
				m_Rigidbody2D.Sleep();
            } 

			animator.SetInteger("xSpeed", Mathf.RoundToInt(Mathf.Abs(m_Rigidbody2D.velocity.x)));
			animator.SetFloat("ySpeed", m_Rigidbody2D.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}


		if (attacking || specialing)
		{
			if (attackTime >= Time.time)
			{
				AttackHitboxes(specialing);
			}
			else
			{
				attacking = false;
				specialing = false;
				if (!m_Grounded)
                {
					m_HasAirAttack = false;
				}
			}
		}

		// If the player should jump...
		if (m_Grounded && jump)
		{
			if (roll)
			{
				// bound
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * 1.2f));
			} else if (run)
            {
				// leap
				m_Rigidbody2D.AddForce(new Vector2(10f * move, m_JumpForce / 1.5f));
			} else
            {
				// jump
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
			m_Grounded = false;
		} else if (m_wallClingState && jump)
        {
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
			m_Rigidbody2D.WakeUp();
			m_wallClingState = false;
		}
		animator.SetBool("grounded", m_Grounded);
		animator.SetBool("wallCling", m_wallClingState);
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		if (m_wallClingState)
		{
			m_Rigidbody2D.WakeUp();
			m_wallClingState = false;
		}

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Damage(int damage, float sourcePositionX)
	{
		FindObjectOfType<GameUI_Controller>().DecreaseHP(damage);
		if (gameManager.playerCurrentHealth <= 0)
		{
			Perish();
			return;
		}
		float impactSide = Mathf.Sign(sourcePositionX - transform.position.x);
		m_Rigidbody2D.AddForce(transform.up * 3f + transform.right * -10f * (impactSide), ForceMode2D.Impulse);
	}
	public void Perish()
	{
		Debug.Log("Perished");
	}
	public void Attack(bool useSpecial)
	{
		attackTime = Time.time + 0.2f;
		if (useSpecial) attackTime += 0.2f;
		animator.SetTrigger("attack");
		if (useSpecial)
        {
			if (!(gameManager.currentItem.name != "Ice Pick"))
            {
				animator.SetInteger("attackType", 2);
			} else
            {
				animator.SetInteger("attackType", 1);
			}
        } else
        {
			animator.SetInteger("attackType", 0);
        }
		attacking = !useSpecial;
		specialing = useSpecial;
	}

	private void AttackHitboxes(bool useSpecial)
    {
		if (useSpecial)
		{
			if (gameManager.currentItem.name == "Ice Pick")
			{
				Vector3 boxAttackPoint = m_AttackPoint.position + Vector3.up * 0.4f;
				Vector2 boxRange = Vector2.down * 0.7f + Vector2.right * attackRange;
				Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(boxAttackPoint, boxRange, enemyLayers);
				Collider2D[] hitWall = Physics2D.OverlapBoxAll(boxAttackPoint, boxRange, 0, m_WhatIsGround);
				foreach (Collider2D enemy in hitEnemies)
				{
					enemy.TryGetComponent(out Breakable_Wall wall);
					if (wall) wall.Damage(2);
				}
				if (hitWall.Length > 0 && !m_Grounded)
				{
					m_Rigidbody2D.Sleep();
					m_wallClingState = true;
					m_HasAirAttack = true;
					attackTime = 0f;
				}
			}
		}
		else
		{
			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_AttackPoint.position, attackRange, enemyLayers);
			foreach (Collider2D enemy in hitEnemies)
			{
				enemy.TryGetComponent(out Breakable_Wall wall);
				if (wall) wall.Damage(1);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (m_AttackPoint != null)
		{
			Gizmos.DrawWireSphere(m_AttackPoint.position, attackRange);
		}
	}
}