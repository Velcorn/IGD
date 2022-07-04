using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.GetComponent<Animator>().enabled = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        anim.GetComponent<Animator>().enabled = true;
    }
}
