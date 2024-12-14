using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarHealthCollectibles : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
          //  SoundManager.instance.PlaySound(pickupSound);
            collision.GetComponent<LunarHealth>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
