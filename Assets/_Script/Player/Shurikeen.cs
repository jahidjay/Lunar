using UnityEngine;

public class Shurikeen : MonoBehaviour
{

    public float lifeTime = 3;
    public float damage = 10f;

    float currentTime = 0;

    private void Start()
    {
        currentTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - currentTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<LunarHealth>().TakeDamage(damage);        Destroy(gameObject);
        }
        Debug.Log("HERE");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<LunarHealth>().TakeDamage(damage);        Destroy(gameObject);
        }
        Debug.Log("HERE");
        Destroy(gameObject);
    }
}