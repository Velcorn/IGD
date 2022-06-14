using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    [TextArea(3, 20)]
    public string[] sentences;
    public float textSpeed;
    private int index;
    
    void Start()
    {
        textDisplay.text = "";
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textDisplay.text == sentences[index])
            {
                NextSentence();
            }
            else
            {
                StopAllCoroutines();
                textDisplay.text = sentences[index];
            }
        }
    }
    
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(Type());
    }
    
    IEnumerator Type()
    {
        foreach (char c in sentences[index].ToCharArray())
        {
            textDisplay.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    
    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            gameObject.SetActive(false);
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene("Timer0");
            }
        }
    }
}
