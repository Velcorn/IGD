using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            SceneManager.LoadScene("Timer0");
        }
        else if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
