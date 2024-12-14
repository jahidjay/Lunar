using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Wall Climbing")]
    public float climbSpeed = 4f;
    public LayerMask wallLayer;

    [Header("Combat")]
    public Transform meleeAttackPoint;
    public float meleeRange = 1f;
    public float meleeDamage = 10;
    public LayerMask enemyLayer;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Physics")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isClimbing;

    private float moveInput;


    Animator animator;

    public AudioClip MeleeAttack;
    public AudioClip RangeAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        CheckEnvironment();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleWallClimbing();
    }

    private void HandleInput()
    {
        // Get movement input
        moveInput = Input.GetAxis("Horizontal");

        // Jump
        if (Input.GetButtonDown("Jump") && (isGrounded || isTouchingWall))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Slash
        if (Input.GetButtonDown("Fire1"))
        {
            Slash();
        }

        // Shoot
        if (Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }
    }
    private bool isFacingRight = true;
    private void HandleMovement()
    {
        if (!isClimbing)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            if(moveInput < 0f || moveInput > 0.01f) 
            {
                animator.SetBool("run", true); 
            }
            else animator.SetBool("run", false);
            //flip the character according to moveInput float

            if (moveInput > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && isFacingRight)
            {
                Flip();
            }

        }

    }

    void Flip()
    {
        // Toggle the facing direction
        isFacingRight = !isFacingRight;

        // Rotate the object 180 degrees on the Y axis
        Vector3 rotation = transform.eulerAngles;
        rotation.y += 180f;
        transform.eulerAngles = rotation;
    }

    private void HandleWallClimbing()
    {
        if (isTouchingWall && !isGrounded && Input.GetAxisRaw("Vertical") != 0)
        {
            isClimbing = true;
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * climbSpeed);
            rb.gravityScale = 0;
        }
        else
        {
            isClimbing = false;
            rb.gravityScale = 1;
        }
    }

    private void Slash()
    {
        animator.SetTrigger("slash 0");

        //slash sound
        AudioManager.Instance.PlayAudio(MeleeAttack,transform.position);

        // Detect enemies in melee range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeRange, enemyLayer);

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            // Implement enemy damage here
            enemy.GetComponent<LunarHealth>().TakeDamage(meleeDamage);
        }
    }

    private void Shoot()
    {

        AudioManager.Instance.PlayAudio(RangeAttack, transform.position);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = firePoint.right * bulletSpeed;
        Debug.Log("Shoot");
    }

    private void CheckEnvironment()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(transform.position, 0.5f, wallLayer);
    }



    private void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player is near a ladder
        //Debug.Log(other.gameObject.layer);
        if (other.gameObject.layer == wallLayer)
        {
            isTouchingWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player left the ladder area
        if (other.gameObject.layer == wallLayer)
        {
            isTouchingWall = false;
        }
    }
}
