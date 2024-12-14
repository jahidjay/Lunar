using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    //Player Ref
    Transform player;
    Hero hero;

    [Header("Common Enemy Parameters")]
    public float speed = 4f;
    public string AttackAnimationName = "Attack";
    public List<Transform> LeftRightMoveLimitsPos;
    public float attackInterval = 2;
    public float waitinPatrol = 2;
    public float minDisToAttack = 2;
    public float minDisToDealDamage = 1;
    public float minDisToChessPlayer = 5;
    public GameObject exclamation; //to indicate player sencing
    
    [Header("Fly Enemy")]
    public GameObject fireThrowPos;
    public GameObject fireball;
    public Transform fireBallLoc;


    [Header("Health")]
    public Slider healthBarSlider;
    public float health =1;
    public float damageAmount = .5f;

    Animator animator;
    int currentPosIndex = 0;
    bool patrol = true, chessPlayer = false, attacking = false, dead = false;
    bool waitPetrolCoroutineOnce = false;

    [Header("Ninja")]
    public bool ninja = false;
    public GameObject surikenThrowPos;
    public GameObject suriken;

    private void Start()
    {
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("hero").transform;
        hero = player.GetComponent<Hero>();

       
    }




    private void Update()
    {
        if(!dead)
        {
            if(!ninja)
            {
                CheckIfPlayerWithinChessRange();
                MoveToCheckPoint();
            }
            
            CheckIfPlayerWithinAttackRange();

//            healthBarSlider.value = health;

            //RectTransform rt = healthBarSlider.GetComponent<RectTransform>();
            //rt.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }
    
    void CheckIfPlayerWithinChessRange()
    {
        Debug.Log(player);
        if (Vector2.Distance(transform.position, player.position) <= minDisToChessPlayer && 
            player.position.x >= LeftRightMoveLimitsPos[0].position.x && 
            player.position.x <= LeftRightMoveLimitsPos[1].position.x)
        {
            patrol = false;
            chessPlayer = true;

            if(exclamation)
            exclamation.SetActive(true);
        }
        else
        {
            patrol = true;
            chessPlayer = false;

            if (exclamation)
                exclamation.SetActive(false);
        }
    }

    void CheckIfPlayerWithinAttackRange()
    {
        if (Vector2.Distance(transform.position, player.position) <= minDisToAttack)
        {
            RotateCharacter(player.position);
            //print("atck range");
            patrol = false;
            chessPlayer = false;
            
            if(!attacking && !dead)
            {
               // StartCoroutine(AttackPlayer());
            } 
        }
        else
        {
            //patrol = true;
        }
    }

    void MoveToCheckPoint()
    {
        if (chessPlayer && !patrol)
        {
            RotateCharacter(player.position);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
        else
        {
            if (patrol)
            {
                if (transform.position.x != LeftRightMoveLimitsPos[currentPosIndex].position.x)
                {
                    RotateCharacter(LeftRightMoveLimitsPos[currentPosIndex].position);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(LeftRightMoveLimitsPos[currentPosIndex].position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
                }
                else
                {
                    if (!waitPetrolCoroutineOnce)
                    {
                        animator.Play("Idle");
                        waitPetrolCoroutineOnce = true;
                        StartCoroutine(WaitInPetrol());
                    }
                }
            }
        }
    }


    IEnumerator AttackPlayer()
    {
        attacking = true;
        //print("Attack");

        yield return new WaitForSeconds(attackInterval);

        RotateCharacter(player.position);
        animator.StopPlayback();
        animator.Play(AttackAnimationName);
        attacking = false;
        CheckIfPlayerWithinAttackRange();

    }

    IEnumerator WaitInPetrol()
    {
        attacking = false;
        yield return new WaitForSeconds(waitinPatrol);
        animator.StopPlayback();
        animator.Play("Walk");

        if(currentPosIndex < LeftRightMoveLimitsPos.Count - 1)
        {
            currentPosIndex++;
        }
        else
        {
            currentPosIndex = 0;
        }
        RotateCharacter(LeftRightMoveLimitsPos[currentPosIndex].position);
        waitPetrolCoroutineOnce = false;
    }

    void RotateCharacter(Vector3 pos)
    {

        if (transform.position.x - pos.x < 0)
        {
            transform.localScale =new Vector3(-2, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
        }

        
    }

    public void AttackingFalse()
    {
        attacking = false;
    }

    public void DealDamage()
    {
        if (Vector2.Distance(transform.position, player.position) <= minDisToDealDamage)
        {
            hero.Dead();
        }
    }

 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attack"))
        {
            TakingDamage();
            
        }
        if (collision.gameObject.CompareTag("suriken"))
        {
            TakingDamage();
        }
    }

    void TakingDamage()
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        dead = true;
        healthBarSlider.value = 0;
        animator.SetBool("Dead", true);
        StartCoroutine(DestroyActor());
    }

    IEnumerator DestroyActor()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public float surikenScale;
    public void ThrowSuriken()
    {
        //print("Throwing suriken");

        if(!dead)
        {
            bool l = false;

            if (transform.position.x - player.position.x < 0)
            {
                transform.localScale = new Vector3(-2, transform.localScale.y, transform.localScale.z);
                l = true;
                print("R");
            }
            else
            {
                transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z); l = true;
                print("L");
            }

            GameObject sk = Instantiate(suriken, surikenThrowPos.transform.position, Quaternion.identity);
            sk.transform.localScale = new Vector2(surikenScale, surikenScale);
        }
       
    }
}
