using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //cached references
    private PlayerControls controls;
    private Rigidbody2D rb;
    private Animator animator;

    //state
    private float horizontalInput;
    private float verticalInput;
    private bool grounded;
    private float earlyJumpTimer;
    private float rememberGroundedTimer;
    private float initialJumpVelocity;
    private float maxJumpDuration;
    private float jumpTimer;
    private bool lowJump = false;
    private float runSpeedModifier = 1.0f;
    public bool isBuried = false;
    private float undergroundMovementTimer;

    //config
    [Header("Run Parameters")]
    [SerializeField]
    private float runSpeed = 200.0f;
    [SerializeField]
    private int undergroundJumpDistance = 3;

    [Header("Jump Parameters")]
    [SerializeField]
    private float jumpHeight = 3.0f;
    [SerializeField]
    private float lowJumpGravityScale = 3.0f;
    [SerializeField]
    private float fallingGravityScale = 2.0f;
    [SerializeField]
    private LayerMask walkableLayers;
    [SerializeField]
    private float timeToRememberEarlyJump = 0.2f;
    [SerializeField]
    private float timeToRemberGrounded = 0.125f;
    [SerializeField] [Range(0.0f, 1.0f)]
    private float relativeMinJumpDuration = 0.33f;
    private TileMapManager mapManager;

    [Header("References")]
    [SerializeField]
    private BoxCollider2D feetCollider;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.MoveHorizontal.performed += context => horizontalInput = context.ReadValue<float>();
        controls.Gameplay.MoveHorizontal.canceled += context => horizontalInput = 0.0f;

        controls.Gameplay.MoveVertical.performed += context => verticalInput = context.ReadValue<float>();
        controls.Gameplay.MoveVertical.canceled += context => verticalInput = 0.0f;

        controls.Gameplay.Jump.performed += context => SetEarlyJumpTimer();
        controls.Gameplay.Jump.canceled += context => CancelJump();

        controls.Gameplay.Bury.performed += context => Burry();

        mapManager = FindObjectOfType<TileMapManager>();

    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        CalculateJumpParameters();
        undergroundMovementTimer = 0;
    }
    private void FixedUpdate()
    {
        animator.SetBool("Burried", isBuried);
        CheckGrounded();
        ApplyFallingGravityScale();
        UpdateTimers();
        if (!isBuried)
        {
            Run();
        }
        else
        {
            MoveUnderGround();
        }        
        if (earlyJumpTimer > 0.0f && rememberGroundedTimer > 0.0f) Jump();

        Flip();
    }

    private void Run()
    {
        float horizontalVelocity = horizontalInput * runSpeed * runSpeedModifier * Time.fixedDeltaTime;
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        
        horizontalVelocity = horizontalInput * runSpeed * runSpeedModifier * Time.fixedDeltaTime * mapManager.getMovementMultiplikator(transform.position - new Vector3(0, 1, 0));
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        //animation
        animator.SetFloat("RunSpeed", Mathf.Abs(horizontalVelocity));
    }

    private void MoveUnderGround()
    {
        Vector2 targetPosition = transform.position;
        if (!(undergroundMovementTimer > 0f))
        {
            if (horizontalInput == 1)
            {
                targetPosition = getTargetPosition("right");
            } 
            else if (horizontalInput == -1)
            {
                targetPosition = getTargetPosition("left");
            }
            else if (verticalInput == -1)
            {
                targetPosition = getTargetPosition("down");
            }
            else if (verticalInput == 1)
            {
                targetPosition = getTargetPosition("up");
            }
            transform.position = targetPosition;
        }
    }

    private Vector2 getTargetPosition(string direction)
    {
        undergroundMovementTimer = 0.2f;
        var nextPosition = transform.position;

        switch (direction)
        {
            case "up":
                for (int i = 1; i <= undergroundJumpDistance + 1; i++)
                {
                    nextPosition = nextPosition + new Vector3(0 , 1);
                    if(mapManager.isMycelliumInPosition(nextPosition))
                    {
                    Debug.Log("Position: " + nextPosition+ "Current: " + transform.position);

                        if (!mapManager.isTileOnPosition(nextPosition) && i != 1)
                        {
                            return nextPosition;
                        }
                        else
                        {
                            return transform.position;
                        }
                    }
                }
                return transform.position;

            case "down":
                for (int i = 1; i <= undergroundJumpDistance + 1; i++)
                {
                    nextPosition += new Vector3(0 , -1);
                    if(!mapManager.isMycelliumInPosition(nextPosition))
                    {
                    Debug.Log("Position: " + nextPosition+ "Current: " + transform.position);

                        if (!mapManager.isTileOnPosition(nextPosition) && i != 1)
                        {
                            
                            return nextPosition;
                        }
                        else
                        {
                            return transform.position;
                        }
                    }
                }
                return transform.position;

            case "left":
                for (int i = 1; i < undergroundJumpDistance + 1; i++)
                {
                    nextPosition = nextPosition + new Vector3(-1 , 0);
                    print(i + " : " + mapManager.isMycelliumInPosition(nextPosition) + " on " + nextPosition);
                    if(!mapManager.isMycelliumInPosition(nextPosition))
                    {
                    Debug.Log("Position: " + nextPosition+ "Current: " + transform.position);

                        if (!mapManager.isTileOnPosition(nextPosition) && i != 1)
                        {
                            return nextPosition;
                        }
                        else
                        {
                            return transform.position;
                        }
                    }
                }
                return transform.position;

            case "right":
                for (int i = 1; i <= undergroundJumpDistance + 1; i++)
                {
                    nextPosition = nextPosition + new Vector3(1 , 0);
                    if(!mapManager.isMycelliumInPosition(nextPosition))
                    {
                    Debug.Log("Position: " + nextPosition+ "Current: " + transform.position);

                        if (!mapManager.isTileOnPosition(nextPosition) && i != 1)
                        {
                            return nextPosition;
                        }
                        else
                        {
                            return transform.position;
                        }
                    }
                }
                return transform.position;

            default:
                return transform.position;
        }
        
    }

    public void SetRunSpeedModifier(float modifier)
    {
        runSpeedModifier = modifier;
    }
    public void ResetRunSpeedModifier()
    {
        runSpeedModifier = 1.0f;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, initialJumpVelocity);
        jumpTimer = maxJumpDuration;

        earlyJumpTimer = 0.0f;
        rememberGroundedTimer = 0.0f;

        //animation
        animator.SetTrigger("Jump");
    }
    private void CancelJump()
    {
        if (jumpTimer > 0.0f) lowJump = true;
    }
    private void CheckGrounded()
    {
        grounded = feetCollider.IsTouchingLayers(walkableLayers);
        if (grounded)
        {
            rememberGroundedTimer = timeToRemberGrounded;
            rb.gravityScale = 1.0f;

            //animation
            animator.SetBool("Falling", false);
        }
        animator.SetBool("Grounded", grounded);
    }
    private void ApplyFallingGravityScale()
    {
        if (rb.velocity.y < 0.0f && !grounded) // Fix f�r Animation
        {
            rb.gravityScale = fallingGravityScale;

            //animation
            animator.SetBool("Falling", true);
        } 
    }
    private void SetEarlyJumpTimer()
    {
        earlyJumpTimer = timeToRememberEarlyJump;
    }
    private void CalculateJumpParameters()
    {
        maxJumpDuration = Mathf.Sqrt(-2.0f * jumpHeight / Physics2D.gravity.y);
        initialJumpVelocity = 2.0f * jumpHeight / maxJumpDuration;
    }
    private void UpdateTimers()
    {
        if (earlyJumpTimer > 0.0f) earlyJumpTimer -= Time.fixedDeltaTime;
        if (rememberGroundedTimer > 0.0f) rememberGroundedTimer -= Time.fixedDeltaTime;
        if (jumpTimer > 0.0f)
        {
            jumpTimer -= Time.fixedDeltaTime;
            if (lowJump && jumpTimer <= maxJumpDuration * (1.0f - relativeMinJumpDuration))
            {
                lowJump = false;
                rb.gravityScale = lowJumpGravityScale;
            }
        }
        if (undergroundMovementTimer > 0f) undergroundMovementTimer -= Time.fixedDeltaTime;
    }
    private void Flip()
    {
        if (rb.velocity.x > 0.0f) transform.eulerAngles = Vector3.zero;
        else if (rb.velocity.x < 0.0f) transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
    }

    public void Burry(){
        if (grounded && !isBuried)
        {
            rb.velocity = Vector2.zero;
            //animator.SetFloat("RunSpeed", 0);
            animator.SetBool("Bury", true);
            isBuried = true;
            rootGround(this.transform.position - new Vector3(0, 1, 0));
        }
        else
        {
            isBuried = false;
        }
        
    }

    private void rootGround(Vector3 position)
    {
        mapManager.rootGround(position);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}