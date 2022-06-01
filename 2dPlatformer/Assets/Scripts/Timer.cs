using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public int TimeLeft;
    public bool TimerOn = false;
    
    void Start()
    {
        textDisplay.text = TimeLeft.ToString();
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        while (TimerOn && TimeLeft > 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                TimeLeft--;
                yield return new WaitForSeconds(1);
                textDisplay.text = TimeLeft.ToString();
            }
            else
            {
                yield return new WaitForSeconds(.3f);
            }
        }
        TimerOn = false;
        StopCoroutine(Type());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
