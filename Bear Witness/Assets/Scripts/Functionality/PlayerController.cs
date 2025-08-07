using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.Tilemaps;


/// <summary>
/// Script for calculating player movement in response to inputs from PlayerMovement
/// </summary>
public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_DefaultGravity = 1.75f;					// Default gravity
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_AttackPoint;							// A position marking the center of attacks
	[SerializeField] private Light2D lanternLight;								// A light which appears if the lantern is held
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when rolling
	[SerializeField] private Animator animator;									// Player sprite animator
	[SerializeField] private ParticleSystem damageParticles;                    // Particles that appear when player is damaged
    [SerializeField] private ParticleSystem deathParticles;                    // Particles that appear when player dies
    [SerializeField] private GameObject lantern;
	[SerializeField] private GameObject phaserBlast;
	[SerializeField] public Item brokenLantern;									// Lantern items
	[SerializeField] public Item normalLantern;
	[SerializeField] private Tilemap collisionTilemap;

	const float k_GroundedRadius = .1f;			// Radius of the overlap circle to determine if grounded
	public bool m_Grounded = true;				// Whether or not the player is grounded.
	const float k_CeilingRadius = .1f;			// Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;			// For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private int coyotePoints = 8;               // Number of forgiveness frames for jumps

	private bool jumping = false;

	private GameManager gameManager;
	private float attackTime = 0f;
	public float attackRange = 0.4f;
	public LayerMask enemyLayers;
	public LayerMask hammerLayers;
	private bool attacking = false;
	private int attackType = 0;
	private bool m_HasAirAttack = true;
	public float invTime = 0f;
	private bool shielded = false;

	private bool wedgeState = false;
	private bool doAttackHitbox = false;
	private float moveLeftDelay = 0f;
	private float moveRightDelay = 0f;

	private bool running = false;

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

		if (!collisionTilemap) collisionTilemap = FindFirstObjectByType<TilemapCollider2D>().GetComponent<Tilemap>();
	}

    private void Start()
    {
		gameManager = FindFirstObjectByType<GameManager>();
		CheckLantern();
    }

	public void CheckLantern()
	{
        // checks to see if the lantern is equipped
        for (int i = 0; i < gameManager.currentItems.Count; i++)
        {
            if (gameManager.currentItems[i] == "Lantern")
            {
                animator.SetBool("useLantern", true);
                lanternLight.enabled = true;
                break;
            }
            else if (i + 1 == gameManager.currentItems.Count)
            {
                animator.SetBool("useLantern", false);
                lanternLight.enabled = false;
            }
        }
    }

    private void FixedUpdate()
	{
		// increases gravity when falling
		if (m_Rigidbody2D.gravityScale != m_DefaultGravity && !inWater)
        {
			if (m_Rigidbody2D.velocity.y < -0.25f)
            {
				SetGravityFraction(1f);
            } 
        }

		// makes the damage flash decay over time
		if (whiteFlash != 0f)
        {
			whiteFlash = Mathf.Max(whiteFlash - 10 * Time.deltaTime, 0);
			GetComponent<SpriteRenderer>().material.SetFloat("_FlashBrightness", whiteFlash);
		}

		// checks to see if the player is grounded
		CheckGround();

		// counts the number of frames the player has spent in the air
		if (!m_Grounded && coyotePoints > 0)
        {
			coyotePoints--;
			m_Grounded = true;
        }

		// checks to see if the player is in water
		CheckWater();

		// ladders
		if (wasClimbing && !playerMovement.climbing)
        {
			wasClimbing = false;
			m_Rigidbody2D.gravityScale = m_DefaultGravity;
        }

		if (wedgeState) m_Rigidbody2D.velocity = Vector2.zero;
    }

    private void Update()
    {
        animator.SetBool("stayOnIcePick", wedgeState);

	}


	// MOVEMENT SCRIPT FOR SWIMMING
	public void Swim(float hmove, float vmove, bool jump)
    {
		// left/right/up/down movement
		bool atSurface = CheckAtSurface();
		float mod = gameManager.GetSpeedModifiers(this);
		hmove *= mod;
		vmove *= mod;

		Vector2 targetVelocity = new Vector2(hmove * 10f, vmove * 10f);

		if (atSurface)
        {
			// makes sure you can't swim past the surface
			targetVelocity.y = Mathf.Min(targetVelocity.y, 0f);
        }

		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


		// jumping
		if (jump && atSurface)
        {
			SetGravityFraction(2f / 3f);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * 1.2f));
			m_Grounded = false;
		}


		// animation
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

	// MOVEMENT SCRIPT FOR WALKING/RUNNING/ROLLING
    public void Move(float move, bool jump, bool run, bool roll)
	{
		// you can't move if shielded
		if (shielded)
        {
			move = 0f;
			jump = false;
			run = false;
			roll = false;
        }

		// If attempting to leave a roll, check to see if the character can stand up
		if (!roll && animator.GetCurrentAnimatorStateInfo(0).IsName("roll"))
		{
			// If the character has a ceiling preventing them from standing up, keep them rolling
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				roll = true;
			}
		}

		// animations
        animator.SetBool("run", run);
        animator.SetBool("roll", roll);

		running = run;

		// if move left/move right cooldowns are not refreshed, prevent player from moving in that direction
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
					// player just started rolling
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
					// player just stopped rolling
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			move *= gameManager.GetSpeedModifiers(this);
			Vector3 targetVelocity;

			if (attacking)
            {
				if (m_HasAirAttack)
                {
					// slows vertical speed while attacking
					targetVelocity = new Vector2(move, Mathf.Max(m_Rigidbody2D.velocity.y, 0f));
				} else
                {
					// normal movement, without momentum
					targetVelocity = new Vector2(move * 7f, m_Rigidbody2D.velocity.y);
				}
			} else
            {
                targetVelocity = new Vector2(move * 7f + m_Rigidbody2D.velocity.x / 2f, m_Rigidbody2D.velocity.y);

				// REMOVING THIS TO SEE IF IT'S NECESSARY
                // if (Mathf.Abs(move * 14f) < Mathf.Abs(m_Rigidbody2D.velocity.x) && Mathf.Sign(move) == Mathf.Sign(m_Rigidbody2D.velocity.x) && Mathf.Abs(move) > 0.1f)
				// {
					// if the player is moving extremely fast compared to their normal speed, slow them down gradually
					// maybe the /2f is too fast?
					// targetVelocity = new Vector2(m_Rigidbody2D.velocity.x / 2f, m_Rigidbody2D.velocity.y);
				// } else
                // {
                    // normal movement, with momentum
                    // targetVelocity = new Vector2(move * 7f + m_Rigidbody2D.velocity.x / 2f, m_Rigidbody2D.velocity.y);
                // }
			}

			// Smoothing it out and applying it to the player
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// animations
			animator.SetInteger("xSpeed", Mathf.RoundToInt(Mathf.Abs(m_Rigidbody2D.velocity.x)));
			animator.SetFloat("ySpeed", m_Rigidbody2D.velocity.y);

			// only flip player when not attacking
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

		if (doAttackHitbox)
		{
			AttackHitboxes();
			// check for wall-grabs (so it occurs during the duration of the swing)
			//Vector3 attackCenter = m_AttackPoint.position + Vector3.down * 0.1f;
			//if (Physics2D.OverlapCircleAll(attackCenter, attackRange / 2f, m_WhatIsGround).Length > 0)
			//{
			//	// cancel velocity and start wall jump
			//	m_Rigidbody2D.velocity = Vector3.zero;
			//	startWallClingHitbox = false;
			//	m_wallClingState = true;
			//	m_HasAirAttack = true;
			//	attacking = false;
			//	ForceWallJump();
			//}

			//if (attackTime + 0.2f <= Time.time) startWallClingHitbox = false;
		}

		// If the player should jump...
		if (jump)
        {
			// prevent further jumps
			coyotePoints = 0;
			jumping = true;

            if (wedgeState)
            {
                ForceWallJump();
                wedgeState = false;
            } else if (m_Grounded || wasClimbing)
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

                AudioManager.instance.Play("Jump", 0, 0, 0);

                m_Grounded = false;
			}
        }

		animator.SetBool("grounded", m_Grounded);
	}


	public void Flip()
	{
		if (!wedgeState)
		{
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
	}

	public void Damage(int damage, float sourcePositionX, bool doKnockback = true, bool bypassInv = false)
	{
		if (shielded && (transform.position.x - sourcePositionX > 0 ^ m_FacingRight))
        {
			if (doKnockback)
				Knockback(200f);
        } else if (invTime <= Time.time || bypassInv)
        {
			if (shielded)
            {
				shielded = false;
				animator.SetTrigger("attackEnd");
            }

            float impactSide = Mathf.Sign(sourcePositionX - transform.position.x);

            // pretty effects

            invTime = Time.time + 1.0f;
			whiteFlash = 2f;
			gameManager.DamagePlayer(damage);
			AudioManager.instance.TempMute(2f, 1f);

			// BADGE CODE: Flare
			if (gameManager.currentBadges.Contains("Flare"))
			{
				animator.Play("flare", 6);
			}


            damageParticles.Play();

			gameManager.GetComponent<FreezeFrame>().Freeze(Mathf.Min(0.1f * damage, 0.5f));
            CinemachineShake.instance.ShakeCamera(0.5f, 4f);

            playerMovement.PlayAnimation("hurt");

			if (doKnockback)
            {
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

	public void Kablooie()
	{
		deathParticles.Play();
	}

	public void Attack(int button)
	{
		animator.ResetTrigger("attackEnd");
		attackTime = Time.time + 0.15f;
		string tool = gameManager.currentItems[button];
		switch (tool)
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

			case "Lantern":
				attackType = 6;
				break;

			case "Fire Spear":
				attackType = 7;
				if (m_HasAirAttack)
				{
					SpearUp();
					m_HasAirAttack = false;
				}
				break;

			case "Phaser":
				attackType = 8;
				break;

			case "Seashell":
				gameManager.AddTempBuff(TemporaryBuff.BuffType.Resilience, 15f);
                EatItem("Seashell", button);
                break;

			case "Haddock":
				gameManager.HealPlayer(4f);
                EatItem("Haddock", button);
                break;

			case "friend":  // don't do it it's not worth it
                gameManager.HealPlayer(1f);
                EatItem("friend", button);
				gameManager.GrantAchievement("Curiosity Killed The Rat");
                break;

            default:
				if (button == 2 && Input.GetButton("Run"))
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

	private void EatItem(string item, int button)
	{
        attackType = -1;
        gameManager.currentItems[button] = "";
        gameManager.Consume(item);
        GameUI_Controller.instance.Reload();
		AudioManager.instance.Play("SnowStep", fadeTime:0);
    }

	public void AttackHitboxes()
    {
		Vector3 attackCenter;
		Collider2D[] hitEnemies;
		bool hitLiveEnemy = false;
		int atk = 0;
		switch (attackType)
        {
			case 0:
				hitEnemies = Physics2D.OverlapCircleAll(m_AttackPoint.position, attackRange, enemyLayers);
				atk = Mathf.RoundToInt(3f * gameManager.GetAttackModifiers());

                foreach (Collider2D enemy in hitEnemies)
				{
					enemy.TryGetComponent(out ReceiveDamage hitbox);
					if (hitbox) hitbox.Damage(atk, transform.position.x);
					enemy.TryGetComponent(out AttackBounce atkBounce);
					if (atkBounce && atkBounce.GetActive()) hitLiveEnemy = true;
                }
				if (hitLiveEnemy)
				{
					Knockback(100f);
                    CinemachineShake.instance.ShakeCamera(0.1f, 0.25f);
                }
				break;

				// attackType 1 is dab

			case 2:
				attackCenter = m_AttackPoint.position + Vector3.right * 0.05f + Vector3.down * 0.1f;
				hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange * 0.7f, enemyLayers + m_WhatIsGround);
                atk = Mathf.RoundToInt(5f * gameManager.GetAttackModifiers());

                foreach (Collider2D enemy in hitEnemies)
				{
                    enemy.TryGetComponent(out ReceiveDamage hitbox);
                    if (hitbox) hitbox.Damage(atk, transform.position.x);
                    enemy.TryGetComponent(out AttackBounce atkBounce);
                    if (atkBounce && atkBounce.GetActive()) hitLiveEnemy = true;
                }
				if (hitEnemies.Length > 0)
				{
                    CinemachineShake.instance.ShakeCamera(0.1f, 0.25f);

					if (!hitLiveEnemy && gameManager.currentBadges.Contains("Wedge"))
					{
                        m_Rigidbody2D.velocity = Vector3.zero;
                        SetAttackHitboxes(-1);
                        m_HasAirAttack = true;
                        attacking = false;

                        // gives you 3 frames for a superjump
                        coyotePoints = Mathf.Min(coyotePoints, 3);

						m_Grounded = false;
						SetGravityFraction(0);
						wedgeState = true;
                    } else
					{

                        // cancel velocity and start wall jump
                        m_Rigidbody2D.velocity = Vector3.zero;
                        animator.SetTrigger("wallCling");
                        SetAttackHitboxes(-1);

                        // gives you 3 frames for a superjump
                        coyotePoints = Mathf.Min(coyotePoints, 3);
                        m_HasAirAttack = true;
                        attacking = false;
                        ForceWallJump();
                    }

					AudioManager.instance.Play("IcePick", 0, 0, 0);
                }
				break;

			case 3:
				shielded = true;
				break;

			case 4:
				hitEnemies = Physics2D.OverlapCircleAll(m_AttackPoint.position, attackRange, hammerLayers);
                atk = Mathf.RoundToInt(10f * gameManager.GetAttackModifiers());
                foreach (Collider2D enemy in hitEnemies)
				{
                    enemy.TryGetComponent(out ReceiveDamage hitbox);
                    if (hitbox) hitbox.Damage(atk, transform.position.x);
                    enemy.TryGetComponent(out AttackBounce atkBounce);
                    if (atkBounce && atkBounce.GetActive()) hitLiveEnemy = true;
                }
				if (hitLiveEnemy)
				{
					Knockback(100f);
                    CinemachineShake.instance.ShakeCamera(0.2f, 2f);
                }
				break;

			case 5:
				attackCenter = transform.position + Vector3.right * 0.15f + Vector3.down * 0.4f;
				hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, hammerLayers);
                atk = Mathf.RoundToInt(10f * gameManager.GetAttackModifiers());
                foreach (Collider2D enemy in hitEnemies)
				{
                    enemy.TryGetComponent(out ReceiveDamage hitbox);
                    if (hitbox) hitbox.Damage(atk, transform.position.x);
                    enemy.TryGetComponent(out AttackBounce atkBounce);
                    if (atkBounce && atkBounce.GetActive()) hitLiveEnemy = true;
                }
				if (hitLiveEnemy)
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
                    CinemachineShake.instance.ShakeCamera(0.2f, 2f);
                }
				break;

			case 6:
				GameObject lanternProjectile = Instantiate(lantern);
				lanternProjectile.transform.position = transform.position;
				Vector2 vel = new(transform.localScale.x * 5f, -2f);
				lanternProjectile.GetComponent<Rigidbody2D>().velocity = vel;

				int index = -1;
				index = gameManager.tools.IndexOf("Lantern");
                if (index > -1) gameManager.tools[index] = "Broken Lantern";

                index = -1;
				index = gameManager.currentItems.IndexOf("Lantern");
				if (index > -1) gameManager.currentItems[index] = "Broken Lantern";

				GameUI_Controller.instance.Reload();
				CheckLantern();
				break;

			case 8:  // phaser, 7 is for fire spear

				if (gameManager.currentBadges.Contains("Phase Change"))
				{
					Flip();
				}

				Vector2 recoilForce = new(transform.localScale.x * -1000f, 80f);
				doAttackHitbox = false;
				m_HasAirAttack = true;  // allows spear-gun chaining

				m_Rigidbody2D.velocity = Vector2.zero;
                m_Rigidbody2D.AddForce(recoilForce);

                moveRightDelay = Time.time + 0.25f;
                moveLeftDelay = Time.time + 0.25f;

                GameObject phaserProjectile = Instantiate(phaserBlast);
                phaserProjectile.transform.position = transform.position;
                Vector2 ppVel = new(transform.localScale.x * 20f, 0f);
                phaserProjectile.GetComponent<Rigidbody2D>().velocity = ppVel;
                break;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (m_AttackPoint != null)
		{
			Gizmos.DrawWireSphere(m_AttackPoint.position + Vector3.right * 0.15f + Vector3.down * 0.1f, attackRange);
		}
	}

	public void PhaserPreemptiveFreeze()
	{
        moveRightDelay = Time.time + 0.25f;
        moveLeftDelay = Time.time + 0.25f;
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

	private void CheckGround()
	{
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                m_HasAirAttack = true;
                if (m_Rigidbody2D.velocity.y < 0)
                {
                    // NOT SURE IF THIS IS STILL NECESSARY
                    playerMovement.climbing = false;
                    if (!wasGrounded)
                    {
                        // player just landed!
                        coyotePoints = 8;
                        OnLandEvent.Invoke();
						jumping = false;
                    }
                }
            }
        }
    }

	public void SetAttackHitboxes(int value)
	{
		if (value == -1)
		{
			doAttackHitbox = false;
		} else
		{
			doAttackHitbox = true;
			attackType = value;
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

	private void ForceWallJump()
    {
        animator.SetTrigger("wallCling");
        jumping = false;
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

	private void SpearUp()
	{
		jumping = false;
		coyotePoints = 0;
        SetGravityFraction(2f / 3f);
		m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        playerMovement.frozen = false;
        moveRightDelay = Time.time + 0.25f;
        moveLeftDelay = Time.time + 0.25f;
		invTime = Time.time + 0.25f;
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
		playerMovement.Freeze("Controller");
		yield return new WaitForSeconds(time);
		playerMovement.Unfreeze("Controller");
    }

	public void ReduceUpwardMovement()
    {
		if (jumping)
		{
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Min(m_Rigidbody2D.velocity.y, m_Rigidbody2D.velocity.y / 3f));
			jumping = false;
        }
	}

	public void SetGravityFraction(float gravityFraction)
    {
		m_Rigidbody2D.gravityScale = gravityFraction * m_DefaultGravity;
    }

    private void LateUpdate()
    {
        if (m_Grounded && Mathf.Abs(m_Rigidbody2D.velocity.x) > 0.1f)
        {
            Vector3Int cellPos = collisionTilemap.WorldToCell(m_GroundCheck.position + new Vector3(0f, -0.2f, 0f));
            if (collisionTilemap.GetTile(cellPos))
            {
                string tileType = collisionTilemap.GetTile(cellPos).name;
                AudioManager.instance.Step(tileType, running);
            }
        }
    }
}