using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class Hero : MonoBehaviour
{
    
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    
    Rigidbody2D rb;
    Animator animator;
    public float speed = 5;
    public float jumpForce = 10;
    public Vector3 lastCheckpoint = Vector3.zero;
    RespawnPoint checkpoint;
    private BoxCollider2D boxCollider;
    
    private float wallJumpCooldown;
    private float horizontalInput;
    [SerializeField] private float jumpPower;

    int life = 5;
    public GameObject gameoverWindow, deathEffect;
    bool bGameOver = false;
    public List<GameObject> lifeIcon = new List<GameObject>();
    public GameObject attackBox;

    bool move = false, jump = false, slash=false, projectile = false, shoot = false;

    public GameObject suriken, surikenSpawnPos;
    public float surikenCooldown;
    float currentSurikenCooldown;
    public int maxSurikenCount = 3;
    int currentSurikenCount = 3;
    public TMP_Text surikenCountText, surikenCooldownText;
    bool playerFacingLeft = false;
    public float surikenSize = 1, surikenSpeed = .16f;

    float jumpCheckTime = 1, currentJumpCheckTime;


    public SOunds sounds;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentSurikenCooldown = surikenCooldown;
        sounds = GetComponent<SOunds>();
    }


    void Update()
    {
        KeyBindings();
        //rb.AddForce(new Vector2(0, -1));
        Movement();
        SurikenManager();
        
        horizontalInput = Input.GetAxis("Horizontal");
        //        body.position+= new Vector2(horizontalInput, 0) * speed * Time.deltaTime;
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one * 1.5f;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);

        
      
        //Set animator parameters
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetMouseButtonDown(0) && !slash)
        {
            //Attack Impliment
            slash = true;
            animator.SetTrigger("slash 0");
            Debug.Log("GG");
        }
        if (onWall())
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 7;
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = extraJumps; //Reset jump counter to extra jump value
            }
            else
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        print(raycastHit);
        return raycastHit.collider != null;
        
    }

    void SurikenManager()
    {
        if(currentSurikenCooldown <= 0)
        {
            if(!(currentSurikenCount >= maxSurikenCount))
            {
                currentSurikenCount++;
                surikenCountText.text = currentSurikenCount.ToString();

            }
            currentSurikenCooldown = surikenCooldown;
            surikenCooldownText.text = "";

        }
        else
        {
            currentSurikenCooldown -= Time.deltaTime;
            if(currentSurikenCount < maxSurikenCount)
            {
                surikenCooldownText.text = currentSurikenCooldown.ToString("F2");
            }
        }
    }


    void KeyBindings()
    {
        if (Input.GetKey(KeyCode.Space) && !bGameOver)
        {
            if (!jump)
            {
                Jump();
                jump = true;
                
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("jump", false);
        }

        if (Input.GetMouseButtonDown(0) && !bGameOver)
        {
       //     sounds.PlaySlash();
            //animator.SetBool("slash", true);
            animator.Play("slash");
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("slash", false);
        }

        if (Input.GetMouseButtonDown(1) && !bGameOver)
        {
            animator.SetBool("shoot", true);
            Shoot();
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("shoot", false);
        }
    }

    void Shoot()
    {
        if(currentSurikenCount > 0)
        {
            sounds.PlaySlash();
            Vector3 pos = new Vector3(transform.forward.x + 5, transform.position.y, transform.position.z);
            GameObject go = Instantiate(suriken, surikenSpawnPos.transform.position, Quaternion.identity);
            go.transform.localScale = new Vector2(surikenSize,surikenSize);
            Shurikeen _suriken = go.GetComponent<Shurikeen>();
            currentSurikenCount--;
            surikenCountText.text = currentSurikenCount.ToString();
        }
       
    }

    void Movement()
    {

        //Move right
        if (Input.GetKey(KeyCode.D) && !bGameOver)
        {
            move = true;
            if(!jump && !slash)
            {
                animator.SetBool("move", true);
            }
            
            transform.rotation = new Quaternion(0, 0, 0, 0); playerFacingLeft = false;
            transform.position = new Vector3((transform.position.x + speed * Time.deltaTime), transform.position.y);
        }

        //applying opposite force so character dont slide when finished giving input
        if (Input.GetKeyUp(KeyCode.D))
        {
            move = false;
            animator.SetBool("move", false);
            rb.AddForce(new Vector2(-10f, 0));
        }


        //Moving left
        if (Input.GetKey(KeyCode.A) && !bGameOver)
        {
            move = true;
            if (!jump && !slash)
            {
                animator.SetBool("move", true);
            }
            transform.rotation = new Quaternion(0, 180, 0, 0); playerFacingLeft = true;
            transform.position = new Vector3((transform.position.x - speed* Time.deltaTime), transform.position.y);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            move = false;
            animator.SetBool("move", false);
            rb.AddForce(new Vector2(10f, 0));
            
        }
        

    }

    void Jump()
    {    
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return; 
        //If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything

        // SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            //  body.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            else
            {
                //If not on the ground and coyote counter bigger than 0 do a normal jump
                if (coyoteCounter > 0)
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) //If we have extra jumps then jump and decrease the jump counter
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }
    private void WallJump()
    {
        rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY), ForceMode2D.Force);
        wallJumpCooldown = 0;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
        // return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            jump = false;
        }
       
        else if(collision.gameObject.CompareTag("trap"))
        {
            Dead();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            //print("New Checkpoint");
            checkpoint = collision.gameObject.GetComponent<RespawnPoint>();
            lastCheckpoint = collision.gameObject.transform.position;
            sounds.PlayCheckPoint();
        }

        else if (collision.gameObject.CompareTag("suriken"))
        {
            print("hero died of suriken");
            Dead();
        }
        else if (collision.gameObject.CompareTag("bossAttack")) 
        {
            print("hero died of boss attack");
            Dead();
        }
    }

    bool alreadyDead = false;
    public void Dead()
    {
        if (alreadyDead)
            return;

        alreadyDead = true;
        animator.Play("dead");
        sounds.PlayDeath();
        life--;

        if(lifeIcon[life])
        lifeIcon[life].SetActive(false);

        deathEffect.SetActive(true);

        if (life <= 0)
        {
            bGameOver = true;
            gameoverWindow.SetActive(true);
        }
        else
        {
            StartCoroutine(RemoveDeathEffect());
        }
       
    }

    IEnumerator RemoveDeathEffect()
    {
        yield return new WaitForSeconds(1);
        deathEffect.SetActive(false);
        gameObject.transform.position = lastCheckpoint;
        alreadyDead = false;
        
        
    }

    public void EnableAttackBox()
    {
        attackBox.SetActive(true);
    }
    public void DisableAttackBox()
    {
        attackBox.SetActive(false);
    }


}
