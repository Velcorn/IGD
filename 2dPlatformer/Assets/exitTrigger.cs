using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitTrigger : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    
    void OnTriggerEnter2D (Collider2D other)
    {
        SceneManager.LoadScene("EndScene");
    }
}