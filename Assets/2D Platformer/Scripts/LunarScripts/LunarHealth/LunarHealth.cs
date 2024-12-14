using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LunarHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        GameStateManager.Instance.ChangeGameState(GameState.Game);
    }

    private void Start()
    {
                components = GetComponents<Behaviour>();
    }
    public void TakeDamage(float _damage)
    {
        if (currentHealth == 0f)
            return;
        if (hurtSound != null)
            AudioManager.Instance.PlayAudio(hurtSound, transform.position, 1f);
        //Debug.Log("Taking Damage");
        //if (invulnerable) return;
        //currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        //AudioManager.Instance.PlayAudio(hurtSound,transform.position,1f);

        //if (currentHealth > 0)
        //{
        //    anim.SetTrigger("hurt");
        //    StartCoroutine(Invunerability());
        //}
        //else
        //{
        //    if (!dead)
        //    {
        //        //Deactivate all attached component classes
        //        foreach (Behaviour component in components)
        //            component.enabled = false;

        //        anim.SetBool("grounded", true);
        //        anim.SetTrigger("die");

        //        dead = true;
        //        AudioManager.Instance.PlayAudio(deathSound,transform.position,1);
        //    }
        //}

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0f, startingHealth);
        //Debug.Log(currentHealth);
        if (currentHealth <= 0f)
        {

            if (gameObject.tag == "Player")
            {
                anim.SetTrigger("Death");
                AudioManager.Instance.PlayAudio(deathSound, transform.position, 1);
                GameStateManager.Instance.isWin = false;
                foreach (Behaviour component in components)
                    component.enabled = false;
                Invoke("LoadEndingScene", 1f);
                return;
            }
            else if (gameObject.tag == "EnemyBoss")
            {
                GameStateManager.Instance.isWin = true;
                foreach (Behaviour component in components)
                    component.enabled = false;
                Invoke("LoadEndingScene", 1f);
                return;
            }
            anim.SetTrigger("die");
            foreach (Behaviour component in components)
                component.enabled = false;

        }
    }

    void LoadEndingScene()
    {
        SceneManager.LoadScene(2);
    }
    public void goaway()
    {
        Destroy(gameObject);
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    //Respawn
    public void Respawn()
    {
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());
        dead = false;

        //Activate all attached component classes
        foreach (Behaviour component in components)
            component.enabled = true;
    }
}
