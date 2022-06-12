using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    
    void OnTriggerEnter2D (Collider2D other)
    {
        player.transform.position = target.transform.position;
    }
}
