using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        private bool isFrozen = false;
        public string entersFromRoom;

        public float movingSpeed;
        public float jumpForce;
        public float leapForce;
        public float boundForce;
        private float moveInput;

        private bool facingRight = true;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private bool stayCrouched;
        public Transform crouchCheck;

        public Transform attackPoint;

        public BoxCollider2D standbox;
        public BoxCollider2D crouchbox;

        public int maxHealth = 5;
        private int currentHealth = 5;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private int setPlayerState = 0;
        private bool isRunning = false;
        private bool isRolling = false;
        private GameManager gameManager;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            maxHealth = gameManager.playerMaxHealth;
            currentHealth = gameManager.playerCurrentHealth;
        }

    private void Awake()
    {
        if (entersFromRoom != FindObjectOfType<GameManager>().previousLevel)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
        if (!isFrozen)
        {
            if (stayCrouched)
            {
                RollState();
            }
            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime * 2);
                    if (setPlayerState < 7 || setPlayerState == 6 && isGrounded)
                    {
                        setPlayerState = 2;
                    }
                    isRunning = true;
                }
                else if ((isRunning || isRolling) && isGrounded)
                {
                    RollState();
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime * 1.5f);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                    if (setPlayerState < 7 || setPlayerState == 6 && isGrounded)
                    {
                        setPlayerState = 1;
                    }
                }
            }
            else
            {
                if (isGrounded && !stayCrouched && setPlayerState < 7) setPlayerState = 0; // Turn on idle animation
                isRunning = false;
                isRolling = false;
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                if (isRolling)
                {
                    rigidbody.AddForce((transform.up + transform.right * moveInput / 1.5f) * boundForce, ForceMode2D.Impulse);
                    setPlayerState = 7;
                }
                else if (isRunning)
                {
                    rigidbody.AddForce(transform.up * leapForce / 2 + transform.right * moveInput * leapForce, ForceMode2D.Impulse);
                    setPlayerState = 8;
                }
                else
                {
                    rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            if (!isGrounded)
            {
                isRunning = false;
                isRolling = false;
                if (stayCrouched)
                {
                    CheckCeiling();
                }
                if (rigidbody.velocity.y > 0)
                {
                    if (setPlayerState < 7)
                    {
                        setPlayerState = 3; // Turn on jump animation
                    }
                }
                else
                {
                    if (setPlayerState == 6 || setPlayerState == 8)
                    {
                        setPlayerState = 6; // turn on leap fall animation
                    }
                    else
                    {
                        setPlayerState = 4; // Turn on fall animation
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                Debug.Log("attack");
                Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.transform.position, 0.5f, 6);
                Debug.Log("hit " + colliders[0].name);
            }
            if (facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInput < 0)
            {
                Flip();
            }
            if (!isRolling && !stayCrouched)
            {
                standbox.enabled = true;
                crouchbox.enabled = false;
            }
            animator.SetInteger("playerState", setPlayerState);
        }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void CheckCeiling()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(crouchCheck.transform.position, 0.2f);
            stayCrouched = colliders.Length > 1;
        }

        private void RollState()
        {
            crouchbox.enabled = true;
            standbox.enabled = false;
            isRolling = true;
            CheckCeiling();
            if (setPlayerState != 7)
            {
                setPlayerState = 5; // Turn on roll animation
            }
        }

        public void Freeze()
        {
            isFrozen = true;
            setPlayerState = 0;
            animator.SetBool("dialogueEscape", true);
            isRolling = false;
            isRunning = false;
        }
        public void Unfreeze()
        {
            animator.SetBool("dialogueEscape", false);
            isFrozen = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }
    public void Damage(int damage, float sourcePositionX)
    {
        Debug.Log("YEEEEEOOUUUUCHHHH!");
        FindObjectOfType<GameUI_Controller>().DecreaseHP(damage);
        if (currentHealth <= 0)
        {
            Perish();
            return;
        }
        float impactSide = Mathf.Sign(sourcePositionX - transform.position.x);
        rigidbody.AddForce(transform.up * 3 + transform.right * -1 * (impactSide), ForceMode2D.Impulse);
    }
    public void Perish()
    {
        Debug.Log("Perished");
    }
    }
