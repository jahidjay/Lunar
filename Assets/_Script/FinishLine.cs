using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject endWindow;
    public SOunds sounds;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("hero"))
        {
            sounds.PlaySuccess();
            endWindow.SetActive(true);
        }
    }
}
