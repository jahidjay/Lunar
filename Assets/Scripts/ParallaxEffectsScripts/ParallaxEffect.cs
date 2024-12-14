using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private Vector3 lastCameraPosition;
    // Start is called before the first frame update
    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main.transform;

        lastCameraPosition = mainCamera.position;
    }
    
    /*
    Foreground Layer:  like (0.5, 0.5), for a stronger parallax effect.
    Midground Layer:  (0.2, 0.2).
    Background Layer: (0.05, 0.05), for subtle movement.
*/

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 deltaMovement = mainCamera.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = mainCamera.position;
    }
}
