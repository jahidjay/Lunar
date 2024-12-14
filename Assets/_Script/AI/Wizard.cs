using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using Unity.VisualScripting;


public class Wizard : MonoBehaviour
{
    public GameObject spell;
    public Transform startLocation, endLocation;
    public string animationToPlay;
    public float inteval = .5f;
    float currentTime =0;
    Animator animator;
        

    public void Start()
    {
        
        animator = gameObject.GetComponent<Animator>();
        currentTime = inteval;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            if(animator)
                animator.Play(animationToPlay);
            else
            {
                print("Null");
            }
            Shoot();
            currentTime = inteval;
        }
    }

    public void Shoot()
    {
        GameObject go = Instantiate(spell, startLocation.position, Quaternion.identity);
        go.transform.DOMove(endLocation.position, 2).OnComplete(() =>
        {
            Destroy(go);
        });
    }
}
