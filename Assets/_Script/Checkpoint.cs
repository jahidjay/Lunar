using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public partial class Checkpoint : MonoBehaviour
{
    public List<Transform> hazards;
    public GameObject hazardPrefab;
    Vector3[] positions;
    Vector3[] sizes;
    
    public void Start()
    {
        positions = new Vector3[hazards.Count]; sizes = new Vector3[hazards.Count];
        for (int i = 0; i < hazards.Count; i++)
        {
            positions[i] = hazards[i].position;
            sizes[i] = hazards[i].localScale;
        }
    }

    public void RespawnHazard()
    {
        for(int i=0;i<positions.Length;i++)
        {
            GameObject go = Instantiate(hazardPrefab, positions[i], Quaternion.identity);
            go.transform.localScale = sizes[i];
        }
        
    }
}
