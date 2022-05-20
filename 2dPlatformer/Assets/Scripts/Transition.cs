using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            LoadTimer();
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            LoadLevel();
        }
    }
    
    public void LoadTimer()
    {
        SceneManager.LoadScene("Timer0");
    }
    
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
