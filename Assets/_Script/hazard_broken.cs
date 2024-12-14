using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class hazard_broken : MonoBehaviour
{
    public List<GameObject> block;
    Rigidbody2D rb;
    BoxCollider2D box;
    List<Vector3> previousPositions=new List<Vector3>();
    Vector3 mainPos;
    public float minFallDelay = 2.0f;  // Minimum delay before a block falls
    public float maxFallDelay = 5.0f;  // Maximum delay before a block falls
    private void Start()
    {
        box=GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        mainPos = transform.position;
        foreach(GameObject obj in block)
        {
            Transform a = obj.GetComponent<Transform>();
            previousPositions.Add(a.position);
        }
    }

    public void Respawn()
    {
        ChangeBodyType(RigidbodyType2D.Kinematic, true);
        transform.position = mainPos;
        int i = 0;
        foreach (GameObject obj in block)
        {
            obj.GetComponent<Rigidbody2D>().isKinematic = true;
            obj.GetComponent<Rigidbody2D>().gravityScale = 0;
            obj.GetComponent<Transform>().position = previousPositions[i];
            ChangeBodyType(RigidbodyType2D.Kinematic, true);
            i++;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
            if(collision.gameObject.CompareTag("hero") && minFallDelay == 3)
            {
                foreach(GameObject obj in block)
                {
                    Rigidbody2D r = obj.AddComponent<Rigidbody2D>();
                    r.isKinematic = true;
                    r.gravityScale = Random.Range(0.5f, 3.0f);

                }
                ChangeBodyType(RigidbodyType2D.Dynamic, false);
            }
       
    }

    private void ChangeBodyType(RigidbodyType2D newBodyType, bool a)
    {
        rb.bodyType = newBodyType;
        box.enabled = a;
    }
}
