using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken2 : MonoBehaviour
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
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LunarHealth>().TakeDamage(damage);        Destroy(gameObject);
        }
        Debug.Log("HERE");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<LunarHealth>().TakeDamage(damage);        Destroy(gameObject);
        }
        Debug.Log("HERE");
        Destroy(gameObject);
    }
}
