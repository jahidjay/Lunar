using DG.Tweening;

using UnityEngine;

public class Spike : MonoBehaviour
{
    public Transform Loc;
    public float delay;
    public float speed = 2;
    public bool up = true;

    private void Start()
    {
        if(up)
        {
            transform.DOMoveY(Loc.position.y, speed).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
        }
       
        else
        {
            transform.DOMove(Loc.position, speed).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("suriken"))
        Destroy(collision.gameObject);
    }
}
