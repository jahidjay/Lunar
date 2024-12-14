
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour
{
    //Player Ref
    Transform player;
    Hero hero;

    public bool isDragon = true;

    [Header("Common Enemy Parameters")]
    public string AttackAnimationName = "Attack";
    public List<Transform> LeftRightMoveLimitsPos;
    public float attackInterval = 2;
    public float minDisToAttack = 2;
    float bodyScale;

    Animator animator;
    bool  attacking = false, dead = false;

    [Header("Dragon")]
    
    public GameObject fireThrowPos;
    public GameObject fireball;
    public Transform fireBallLoc;
    public GameObject jumpAttackhitBox;

    [Header("Skeleton")]
    public GameObject dashAttackhitBox;
    public GameObject punchAttackBox;
    public float punchAttackMoveTime;
    public float dashAttackMoveTime;


    [Header("Health")]
    RectTransform rectTransform;
    public float health=100;
    float currentHealth;
    public Slider healthSlider;
    public float damageFromPlayer = 10;
    public Transform healthbarLoc;

    public float dodgeDistance = 3;
    public float dodgeDistanceY = 3;



    private void Start()
    {
        bodyScale = transform.localScale.x;
        animator = GetComponent<Animator>();
        currentHealth = health;
        player = GameObject.FindGameObjectWithTag("hero").transform;
        hero = player.GetComponent<Hero>();

        //rectTransform = healthSlider.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!dead)
        {
            CheckIfPlayerWithinAttackRange();
        }

        HealthBar();

        
    }

    void CheckIfPlayerWithinAttackRange()
    {
        if (Vector2.Distance(transform.position, player.position) <= minDisToAttack)
        {
            RotateCharacter(player.position);
            //print("atck range");

            if (!attacking && !dead)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            //patrol = true;
        }
    }

    IEnumerator AttackPlayer()
    {
        attacking = true;
        //print("Attack");

        yield return new WaitForSeconds(attackInterval);
        RotateCharacter(player.position);

        int a = Random.Range(0, 3);

        if (a % 2 == 0)
        {
            if (isDragon)
            {
                ProjectileAttack();
            }              
            else
            {
                PunchAttack();
            }
        }
        else
        {
            if (isDragon)
            {
                JumpAttack();
            }
            else
            {
                DashAttack();
            }
        }
       

    }

    void DashAttack()
    {
        Vector2 playerPos = player.position;
        if (ChecktargetPositionValid(playerPos))
        {
            animator.Play("Skill");
        }
    }

    public void StartDash()
    {
        Vector2 playerPos = player.position;
        if (ChecktargetPositionValid(playerPos))
        {
            EnablePunchAttckBox();
            transform.DOMoveX(playerPos.x, dashAttackMoveTime).OnComplete(() =>
            {
                animator.Play("Idle");
            });
        }


        
    }
    void PunchAttack()
    {
        Vector2 playerPos = player.position;
        if(ChecktargetPositionValid(playerPos))
        {
            if (playerPos.x >= LeftRightMoveLimitsPos[0].position.x &&
           playerPos.x <= LeftRightMoveLimitsPos[1].position.x)
            {
                animator.Play("Walk");
                animator.SetBool("Walk", true);
                transform.DOMoveX(playerPos.x, punchAttackMoveTime).OnComplete(() =>
                {
                    animator.SetBool("Walk", false);
                    animator.Play("Attack_Skeleton_02");
                    attacking = false;
                });
            }

              
        }
       
    }



    public void EnablePunchAttckBox()
    {
        punchAttackBox.SetActive(true);
        StartCoroutine(DisablePunchAttckBox());
    }
    IEnumerator DisablePunchAttckBox()
    {
        yield return new WaitForSeconds(.2f);
        {
            punchAttackBox.SetActive(false);
            attacking = false;
        }
    }



    void ProjectileAttack()
    {
        animator.StopPlayback();
        animator.Play(AttackAnimationName);
    }

    void JumpAttack()
    {
        Vector2 playerPos = player.position;
        if (playerPos.x  >= LeftRightMoveLimitsPos[0].position.x &&
           playerPos.x  <= LeftRightMoveLimitsPos[1].position.x)
        {
            jumpAttackhitBox.SetActive(true);
            float prevy = transform.position.y;
            
            transform.DOMoveY(prevy + 5, 2f);
            transform.DOMoveX(playerPos.x, 2f).OnComplete(() => {
                attacking = false;
            });
        }
        
    }

    bool ChecktargetPositionValid(Vector2 targetPos)
    {
        return (targetPos.x >= LeftRightMoveLimitsPos[0].position.x &&
           targetPos.x <= LeftRightMoveLimitsPos[1].position.x);
    }

    IEnumerator DisableHitBox()
    {
        yield return new WaitForSeconds(.5f);
        
        jumpAttackhitBox.SetActive(false);
    
    }
    


    void RotateCharacter(Vector3 pos)
    {
        if (transform.position.x - pos.x < 0)
        {
            transform.localScale = new Vector3(-bodyScale, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(bodyScale, transform.localScale.y, transform.localScale.z);
        }
    }

    public void Dead()
    {
        dead = true;
        animator.SetBool("Dead", true);
        //healthSlider.enable = false;
        //Destroy(healthSlider);
        healthSlider.enabled = false;
        StartCoroutine(DestroyActor());
    }

    IEnumerator DestroyActor()
    {
        yield return new WaitForSeconds(3);
        
        Destroy(gameObject);
    }

    void GetAwayFromPlayer()
    {
        Vector2 playerPos = player.position;
        if(playerPos.x + dodgeDistance >= LeftRightMoveLimitsPos[0].position.x &&
            playerPos.x + dodgeDistance <= LeftRightMoveLimitsPos[1].position.x)
        {
            transform.DOMoveY(transform.position.y + dodgeDistanceY, .3f);
            transform.DOMoveX(playerPos.x + dodgeDistance, .5f).OnComplete(() => {
                attacking = false;
            });
        }
        else if(playerPos.x - dodgeDistance >= LeftRightMoveLimitsPos[0].position.x &&
            playerPos.x - dodgeDistance <= LeftRightMoveLimitsPos[1].position.x)
        {
            transform.DOMoveY(transform.position.y + dodgeDistanceY, .3f);
            transform.DOMoveX(playerPos.x - dodgeDistance, .5f).OnComplete(() => {
                attacking = false;
            });
        }
    }

    public void ThrowFireball()
    {
        //print("Throwing suriken");

        if (!dead)
        {
            if (transform.position.x - player.position.x < 0)
            {
                transform.localScale = new Vector3(-3, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(3, transform.localScale.y, transform.localScale.z);
            }
            Instantiate(fireball, fireThrowPos.transform.position, Quaternion.identity);
            attacking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attack"))
        {
            TakingDamage();
        }
        else if(collision.gameObject.CompareTag("suriken"))
        {
            TakingDamage();
        }
    }

    void TakingDamage()
    {
        print("Dragon Taking Damage");
        currentHealth -= damageFromPlayer;
        //StopAllCoroutines();
        int a = Random.Range(0, 3);

        if (a % 2 == 0)
        {
            GetAwayFromPlayer();
        }
        else
        {
            attacking = false;
        }

    }

    void HealthBar()
    {
        //Vector3 targetPositionScreenSpace = Camera.main.WorldToScreenPoint(healthbarLoc.position);
        //rectTransform.position = targetPositionScreenSpace;

        healthSlider.value = (currentHealth / health);

        if(currentHealth <= 0)
        {
            Dead();
        }
    }

}
