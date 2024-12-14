using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireBall : MonoBehaviour
{
    Transform player;
    public float lifeTime = 3;
    public GameObject hitEffect;
    GameObject go;

    float currentTime = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("hero").GetComponent<Transform>();

        transform.DOMove(player.position, 2f);
        currentTime = Time.time;
    }

    private void Update()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (Time.time - currentTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        go = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        //StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(go);
    }
}
