using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public GameObject hero;
    public GameObject cloud;
    void Update()
    {
        Vector3 pos = transform.position;   
        Vector3 heroPos = hero.transform.position;
        //Vector3 cloudPos = cloud.transform.position;
        transform.position = new Vector3(heroPos.x, pos.y, pos.z);
        
    }
}
