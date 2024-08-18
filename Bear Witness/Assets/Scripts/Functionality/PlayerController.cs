using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_DefaultGravity = 1.75f;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_AttackPoint; 
	[SerializeField] private Collider2D m_CrouchDisableCollider;     // A collider that will be disabled when rolling
	[SerializeField] private Animator animator;

	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded = true;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .1f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private int coyotePoints = 8;

	private GameManager gameManager;
	private float attackTime = 0f;
	public float attackRange = 0.4f;
	public LayerMask enemyLayers;
	public LayerMask hammerLayers;
	private bool attacking = false;
	private int attackType = 0;
	private bool m_HasAirAttack = true;
	private float invTime = 0f;
	private bool shielded = false;

	private bool m_wallClingState = false;
	private bool startWallClingHitbox = false;
	private float moveLeftDelay = 0f;
	private float moveRightDelay = 0f;

	private bool wasClimbing = false;

	private PlayerMovement playerMovement;

	private float whiteFlash = 0f;

	public bool inWater = false;
	[SerializeField] private LayerMask m_WhatIsWater;

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
		m_Rigidbody2D.gravityScale = m_DefaultGravity;

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		playerMovement = GetComponent<PlayerMovement>();
	}

    private void Start()
    {
		gameManager = FindFirstObjectByType<GameManager>();
    }

    private void FixedUpdate()
	{
		if (m_Rigidbody2D.gravityScale != m_DefaultGravity)
        {
			if (m_Rigidbody2D.velocity.y < -0.25f)
            {
				SetGravityFraction(1f);
            } 
        }


		if (whiteFlash != 0f)
        {
			whiteFlash = Mathf.Max(whiteFlash - 10 * Time.deltaTime, 0);
			GetComponent<SpriteRenderer>().material.SetFloat("_FlashBrightness", whiteFlash);
		}

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
				if (m_Rigidbody2D.velocity.y < 0)
				{
					playerMovement.climbing = false;
					if (!wasGrounded)
                    {
						coyotePoints = 8;
						m_HasAirAttack = true;
						OnLandEvent.Invoke();
                    }
				}
			}
		}

		if (!m_Grounded && coyotePoints > 0)
        {
			coyotePoints--;
			m_Grounded = true;
        }

		CheckWater();

		if (wasClimbing && !playerMovement.climbing)
        {
			wasClimbing = false;
			m_Rigidbody2D.gravityScale = m_DefaultGravity;
        }
	}

    private void Update()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
	}

	public void Swim(float hmove, float vmove, bool jump)
    {
		bool atSurface = CheckAtSurface();
		Vector2 targetVelocity = new Vector2(hmove * 10f, vmove * 10f);
		if (atSurface)
        {
			targetVelocity.y = Mathf.Min(targetVelocity.y, 0f);
        }
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		if (jump && atSurface)
        {
			SetGravityFraction(2f / 3f);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * 1.2f));
			m_Grounded = false;
		}

		animator.SetInteger("swimSpeed", Mathf.RoundToInt(Mathf.Abs(m_Rigidbody2D.velocity.x) + Mathf.Abs(m_Rigidbody2D.velocity.y)));

		// If the input is moving the player right and the player is facing left...
		if (hmove > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (hmove < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
	}

    public void Move(float move, bool jump, bool run, bool roll)
	{
		if (shielded)
        {
			move = 0f;
			jump = false;
			run = false;
			roll = false;
        }
		animator.SetBool("run", run);
		// If rolling, check to see if the character can stand up
		if (!roll && animator.GetCurrentAnimatorStateInfo(0).IsName("roll"))
		{
			// If the character has a ceiling preventing them from standing up, keep them rolling
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				roll = true;
			}
		}

		animator.SetBool("roll", roll);

		if ((moveLeftDelay > Time.time && move < 0) || (moveRightDelay > Time.time && move > 0))
        {
			move = 0f;
        }

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// If rolling
			if (roll)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

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

			if (attacking)
            {
				if (m_HasAirAttack)
                {
					// Move the character by finding the target velocity
					targetVelocity = new Vector2(move, Mathf.Max(m_Rigidbody2D.velocity.y, 0f));
				} else
                {
					// Move the character by finding the target velocity
					targetVelocity = new Vector2(move * 7f, m_Rigidbody2D.velocity.y);
				}
			} else
            {
				// Move the character by finding the target velocity

				if (Mathf.Abs(move * 14f) < Mathf.Abs(m_Rigidbody2D.velocity.x) && Mathf.Sign(move) == Mathf.Sign(m_Rigidbody2D.velocity.x) && Mathf.Abs(move) > 0.1f)
				{
					targetVelocity = new Vector2(m_Rigidbody2D.velocity.x / 2f, m_Rigidbody2D.velocity.y);
				} else
                {
					targetVelocity = new Vector2(move * 7f + m_Rigidbody2D.velocity.x / 2f, m_Rigidbody2D.velocity.y);
				}
			}

			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			animator.SetInteger("xSpeed", Mathf.RoundToInt(Mathf.Abs(m_Rigidbody2D.velocity.x)));
			animator.SetFloat("ySpeed", m_Rigidbody2D.velocity.y);

			if (!attacking)
            {
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
		}


		if (attacking)
		{
			if (attackTime <= Time.time)
			{
				attacking = false;
				if (!m_Grounded)
                {
					m_HasAirAttack = false;
				}
			}
		}

		if (startWallClingHitbox)
		{
			// check for wall-grabs (so it occurs during the duration of the swing)
			Vector3 attackCenter = m_AttackPoint.position + Vector3.down * 0.1f;
			if (Physics2D.OverlapCircleAll(attackCenter, attackRange / 2f, m_WhatIsGround).Length > 0)
			{
				// cancel velocity and start wall jump
				m_Rigidbody2D.velocity = Vector3.zero;
				startWallClingHitbox = false;
				m_wallClingState = true;
				m_HasAirAttack = true;
				attacking = false;
				ForceWallJump();
			}

			if (attackTime + 0.2f <= Time.time) startWallClingHitbox = false;
		} else
        {
			m_wallClingState = false;
        }

		// If the player should jump...
		if (jump)
        {
			// prevent further jumps
			coyotePoints = 0;

			if (m_Grounded)
			{
				SetGravityFraction(2f / 3f);
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Max(0f, m_Rigidbody2D.velocity.y));
				if (roll)
				{
					// bound
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * 1.1f));
				}
				else if (run)
				{
					// leap
					m_Rigidbody2D.AddForce(new Vector2(10f * move, m_JumpForce / 1.3f));
				}
				else
				{
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				}

				m_Grounded = false;
			}
        }

		animator.SetBool("grounded", m_Grounded);
		animator.SetBool("wallCling", m_wallClingState);
	}


	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Damage(int damage, float sourcePositionX, bool doKnockback = true)
	{
		if (shielded && (transform.position.x - sourcePositionX > 0 ^ m_FacingRight))
        {
			if (doKnockback)
				Knockback(200f);
        } else if (invTime <= Time.time)
        {
			if (shielded)
            {
				shielded = false;
				animator.SetTrigger("attackEnd");
            }

			invTime = Time.time + 1.0f;
			whiteFlash = 2f;
			gameManager.DamagePlayer(damage);
			if (gameManager.playerCurrentHealth <= 0)
			{
				Perish();
				return;
			} else
            {
				playerMovement.PlayAnimation("hurt");
            }

			if (doKnockback)
            {
				float impactSide = Mathf.Sign(sourcePositionX - transform.position.x);
				Vector2 knockbackForce = transform.up * 100f + transform.right * -250f * (impactSide);
				m_Rigidbody2D.velocity = Vector3.zero;
				m_Rigidbody2D.AddForce(knockbackForce);
            }

		}
	}
	public void Perish()
	{
		playerMovement.PlayAnimation("die");
		playerMovement.frozen = true;
		Debug.Log("Perished");
	}

	public void Unperish()
    {
		playerMovement.PlayAnimation("unperish");
    }

	public void Attack(int button)
	{
		animator.ResetTrigger("attackEnd");
		attackTime = Time.time + 0.15f;
		Item tool = gameManager.currentItems[button];
		Debug.Log(tool.name);
		switch (tool.name)
		{
			case "Ice Pick":
				attackType = 2;
				break;

			case "Shield":
				attackType = 3;
				break;

			case "Hammer":
				if (Input.GetAxisRaw("Vertical") < -0.5f)
                {
					attackType = 5;
                } else
                {
					attackType = 4;
                }
				break;

			case "":
				if (button != 0)
				{
					attackType = 1;
					m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
					moveLeftDelay = Time.time + 0.3f;
					moveRightDelay = Time.time + 0.3f;
				}
				else
				{
					attackType = 0;
				}
				break;
		}

		animator.SetInteger("attackType", attackType);
		animator.SetTrigger("attack");
		attacking = true;
	}

	public void AttackHitboxes()
    {
		Vector3 attackCenter;
		Collider2D[] hitEnemies;
		switch (attackType)
        {
			case 0:
				hitEnemies = Physics2D.OverlapCircleAll(m_AttackPoint.position, attackRange, enemyLayers);
				foreach (Collider2D enemy in hitEnemies)
				{
					enemy.TryGetComponent(out ReceiveDamage hitbox);
					if (hitbox) hitbox.Damage(3, transform.position.x);
				}
				if (hitEnemies.Length > 0)
				{
					Knockback(100f);
				}
				break;

				// attackType 1 is dab

			case 2:
				attackCenter = m_AttackPoint.position + Vector3.right * 0.15f + Vector3.down * 0.1f;
				hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, enemyLayers);
				foreach (Collider2D enemy in hitEnemies)
				{
					enemy.TryGetComponent(out ReceiveDamage hitbox);
					if (hitbox) hitbox.Damage(5, transform.position.x);
				}
				if (hitEnemies.Length > 0)
				{
					Knockback(100f);
				}
				break;

			case 3:
				shielded = true;
				break;

			case 4:
				hitEnemies = Physics2D.OverlapCircleAll(m_AttackPoint.position, attackRange, hammerLayers);
				foreach (Collider2D enemy in hitEnemies)
				{
					enemy.TryGetComponent(out ReceiveDamage hitbox);
					if (hitbox) hitbox.Damage(10, transform.position.x);
				}
				if (hitEnemies.Length > 0)
				{
					Knockback(100f);
				}
				break;

			case 5:
				attackCenter = transform.position + Vector3.right * 0.15f + Vector3.down * 0.4f;
				hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, hammerLayers);
				foreach (Collider2D enemy in hitEnemies)
				{
					enemy.TryGetComponent(out ReceiveDamage hitbox);
					if (hitbox) hitbox.Damage(10, transform.position.x);
				}
				if (hitEnemies.Length > 0)
				{
					// land and bounce
					TryGetComponent<CancelHammerBounce>(out CancelHammerBounce chb);
					if (!chb)
                    {
						m_HasAirAttack = true;
						OnLandEvent.Invoke();
						m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
						m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.5f));
					}
				}
				break;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (m_AttackPoint != null)
		{
			Gizmos.DrawWireSphere(m_AttackPoint.position, attackRange);
		}
	}

	private void CheckWater()
    {
		bool wasInWater = inWater;
		inWater = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f, m_WhatIsWater);
		if (colliders.Length > 0)
        {
			inWater = true;
			if (!wasInWater)
            {
				animator.SetBool("inWater", true);
				m_Rigidbody2D.gravityScale = 0;
				m_Rigidbody2D.velocity = Vector3.zero;
			}
        }
		if (wasInWater && !inWater)
        {
			animator.SetBool("inWater", false);
			m_Rigidbody2D.gravityScale = m_DefaultGravity;
		}
    }

	public void AttackEnd() 
    {
		animator.SetTrigger("attackEnd");
		shielded = false;
    }
    private bool CheckAtSurface()
    {
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(0f, 0.05f, 0f), 0.01f, m_WhatIsWater);
		return colliders.Length == 0;
    }

	public void StartWallClingHitbox()
    {
		startWallClingHitbox = true;
    }

	private void ForceWallJump()
    {
		float jumpXKick = 200f;
		if (m_FacingRight) jumpXKick *= -1f;
		SetGravityFraction(2f / 3f);
		m_Rigidbody2D.AddForce(new Vector2(jumpXKick, m_JumpForce * 0.9f));
		playerMovement.frozen = false;

		if (m_FacingRight)
        {
			moveRightDelay = Time.time + 0.15f;
        } else
        {
			moveLeftDelay = Time.time + 0.15f;
        }
	}

	private void Knockback(float force)
    {
		Vector2 knockbackForce = new(force, 0f);
		if (m_FacingRight)
		{
			knockbackForce *= -1f;
		}
		m_Rigidbody2D.velocity = Vector3.zero;
		m_Rigidbody2D.AddForce(knockbackForce);
	}

	public void Climb(float vMove)
    {
		m_Rigidbody2D.gravityScale = 0f;
		Vector3 targetVelocity = new Vector2(0f, 7f * vMove);
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		wasClimbing = true;
		animator.SetFloat("ySpeed", m_Rigidbody2D.velocity.y);
	}

	public void FreezeCooldown(float time)
	{
		StartCoroutine(UnfreezeAfter(time));
	}

	IEnumerator UnfreezeAfter(float time)
    {
		playerMovement.frozen = true;
		yield return new WaitForSeconds(time);
		playerMovement.frozen = false;
    }

	public void ReduceUpwardMovement()
    {
		m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Min(m_Rigidbody2D.velocity.y, m_Rigidbody2D.velocity.y / 3f));
	}

	public void SetGravityFraction(float gravityFraction)
    {
		m_Rigidbody2D.gravityScale = gravityFraction * m_DefaultGravity;
    }
}