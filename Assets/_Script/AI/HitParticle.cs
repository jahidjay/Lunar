using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
    public float lifeTime = .3f;

    float currentTime = 0;

    void Start()
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
}
