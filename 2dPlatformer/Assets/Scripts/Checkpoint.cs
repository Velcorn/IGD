using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}
