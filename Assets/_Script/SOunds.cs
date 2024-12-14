using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOunds : MonoBehaviour
{
    public AudioClip success, button, checkPoint, slash, death;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySuccess()
    {
        audioSource.PlayOneShot(success);
    }
    public void PlayCheckPoint()
    {
        audioSource.PlayOneShot(checkPoint);
    }
    public void PlayButton() { 
        audioSource.PlayOneShot(button);
    }

    public void PlaySlash()
    {
        audioSource.PlayOneShot(slash);
    }

    public void PlayDeath()
    {
        audioSource.PlayOneShot(death);
    }
}
