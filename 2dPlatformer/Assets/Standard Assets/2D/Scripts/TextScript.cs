using UnityEngine;
using System.Collections;
using TMPro;
using System;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class TextScript : MonoBehaviour
{
	public int TextScript_index;
	public TMP_FontAsset character_font;
	public TMP_FontAsset standard_font;
	public List<int> idxs_of_sents_with_standard_font;
	public string[] sentences;
	public string[] answer_choices;
	public string[] conclusion_right;
	public string[] conclusion_wrong;
	public int correct_answer_index;
	public ReactionScript reaction_script;
	private UnityEngine.UI.Image bubbleBackgroundImage;       
	private TextMeshProUGUI bubbleTextMesh;
	private int interactionCounter;
	private List<TextMeshProUGUI> choices = new List<TextMeshProUGUI>();
	private int currentChoice = 0;
	private bool showingChoices = false;
	private GameObject character = null;
	private Animator anim;
	private GameObject selene;
	private bool explode = false;


	// Start is called before the first frame update
	void Start()
	{
		anim = GetComponent<Animator>();
		selene = GameObject.Find("Selene");
	}
	
	void Awake()
	{
		GameObject speechBubbleBackground = GameObject.Find("SpeechBubbleBackground");
		bubbleBackgroundImage = speechBubbleBackground.GetComponent<UnityEngine.UI.Image>();
		bubbleBackgroundImage.enabled = false;

		GameObject speechBubbleText = GameObject.Find("SpeechBubbleText");
		bubbleTextMesh = speechBubbleText.GetComponent<TextMeshProUGUI>();
		bubbleTextMesh.enabled = false;
	}

	//set an arrow in front of the selected choice with up down arrows or 'S' and 'W'
	void setSelector()
	{
		foreach(var s in choices) 
		{
			if (s.text.Length>=2 && s.text.Substring(0, 2)=="->")
			{
				s.text = s.text.Substring(2, s.text.Length-2);
			}
		}
		if (choices[currentChoice].text.Length > 1 && choices[currentChoice].text.Substring(0, 2) != "->")
		{
			choices[currentChoice].text = "->" + choices[currentChoice].text;
		}
	}
	
	void Update()
	{
		if(character != null && transform.name == character.name)
		{
			if (showingChoices)
			{
				if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
				{
					if (currentChoice<answer_choices.Length - 1)
					{
						currentChoice += 1;
						setSelector();
					} 
				}
				if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.W))
				{
					if(currentChoice > 0)
					{
						currentChoice -= 1;
						setSelector();
					}
				}
			}
			
			// Update bool so Selene can explode
			if (character.name == "Selene")
			{
				explode = true;
			}
		}
	}

	public void hideChoices(){
		showingChoices = false;
		for (int i = 0; i < 4; i++) 
		{
			GameObject s = GameObject.Find("Choice"+i);
			TextMeshProUGUI t = s.GetComponent<TextMeshProUGUI>();
			t.enabled = false;
		}
	}

	public void resetAndHideAll()
	{
		bubbleBackgroundImage.enabled = false;
		bubbleTextMesh.enabled = false;
		hideChoices();
		interactionCounter = 0;
		character = null;
		
		// Jan: Add explosion-handling on Selene
		if (selene != null)
		{
			if (explode)
			{
				anim.Play("seleneExplosionAnimation");
				Destroy(selene, 2.0f);	
			}
		}
	}

	public string interaction(GameObject overlapCharacter)
	{
		character=overlapCharacter;

		if (reaction_script == null)
		{
			Debug.Log("ReactionScript missing for TextScript on character: " + character.name);
		}
		
		Component[] text_scripts;
		text_scripts = character.GetComponents<TextScript>();
		TextScript ts = null;
		foreach (TextScript script in text_scripts){
			if (script.enabled == true)
			{
				ts = script;
			}
		}
		if (ts != null)
		{
			string[] sentences = ts.sentences;
			string[] answer_choices = ts.answer_choices;
			int correct_answer_index = ts.correct_answer_index;
		
			//show sentences if there are any
			if (sentences.Length > 0 && interactionCounter<sentences.Length)
			{
				if (idxs_of_sents_with_standard_font.Contains(interactionCounter))
				{
					bubbleTextMesh.font = standard_font;
				}
				else
				{
					bubbleTextMesh.font = character_font;
				}

				// Show speech bubble and question text
				bubbleBackgroundImage.enabled = true;
				bubbleTextMesh.enabled = true;
				bubbleTextMesh.text = sentences[interactionCounter];
				interactionCounter += 1;	
				
				return "disableMovement";
			}
			
			//show answer choices if there are any
			else if (answer_choices.Length > 0 && interactionCounter == sentences.Length)
			{
				currentChoice=0;
				bubbleBackgroundImage.enabled = true;
				bubbleTextMesh.enabled = false;//hide any previous text
				showingChoices = true;
				for (int i = 0; i < 4; i++) 
				{
					//fill choice objects with answer choices
					if (i<=answer_choices.Length - 1)
					{
						GameObject s = GameObject.Find("Choice"+i);
						TextMeshProUGUI t = s.GetComponent<TextMeshProUGUI>();
						t.enabled = true;
						t.text = answer_choices[i];
						choices.Add(t);
					}
					//hide unused choice objects
					else
					{
						GameObject s = GameObject.Find("Choice"+i);
						TextMeshProUGUI t = s.GetComponent<TextMeshProUGUI>();
						t.enabled = false;
					}
				}
				setSelector();
				interactionCounter+=1;
				return "disableMovement";
			}
			//show conclusion sentences if there are any
			else if (interactionCounter>=sentences.Length + 1) {
				hideChoices();
				bubbleTextMesh.enabled = true;
				int adjusted_counter_to_index = interactionCounter-(sentences.Length + 1);
				//player made the right choice
				if (currentChoice == correct_answer_index)
				{
					if(conclusion_right.Length > 0 && adjusted_counter_to_index < conclusion_right.Length){
						bubbleTextMesh.text = conclusion_right[adjusted_counter_to_index];
						interactionCounter += 1;
						return "disableMovement";
					}
					else
					{
						resetAndHideAll();
						if (reaction_script != null)
						{
							reaction_script.react(TextScript_index, "correct");
						}
						return "enableMovement";
					}
				}
				//player made the wrong choice
				else
				{
					if (conclusion_wrong.Length>0 && adjusted_counter_to_index<conclusion_wrong.Length)
					{
						bubbleTextMesh.text = conclusion_wrong[adjusted_counter_to_index];
						interactionCounter+=1;
						return "disableMovement";
					}
					else
					{
						if (reaction_script != null)
						{
							reaction_script.react(TextScript_index,"wrong");
						}
						resetAndHideAll();
						return "enableMovement";
					}
				}
			}
			//any reaction after just talking to the character
			if (reaction_script != null)
			{
				reaction_script.react(TextScript_index, "talked");
			}
			resetAndHideAll();
			return "enableMovement";
		}
		resetAndHideAll();
		return "enableMovement";
	}
}
