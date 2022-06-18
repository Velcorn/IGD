using UnityEngine;
using System.Collections;
using TMPro;
using System;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class TextScript : MonoBehaviour
{

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
	public bool firstInteractionDone = false;



	// Start is called before the first frame update
	void Awake(){

		GameObject speechBubbleBackground = GameObject.Find("SpeechBubbleBackground");
		bubbleBackgroundImage = speechBubbleBackground.GetComponent<UnityEngine.UI.Image>();
		bubbleBackgroundImage.enabled = false;

		GameObject speechBubbleText = GameObject.Find("SpeechBubbleText");
		bubbleTextMesh = speechBubbleText.GetComponent<TextMeshProUGUI>();
		bubbleTextMesh.enabled = false;


	}

	//set an arrow in front of the selected choice with up down arrows or 'S' and 'W'
	void setSelector(){
		Debug.Log(currentChoice);
		foreach(var s in choices) {
			if(s.text.Length>=2 && s.text.Substring(0, 2)=="->"){
				s.text = s.text.Substring(2, s.text.Length-2);
			}
		}
		if(choices[currentChoice].text.Length>1 && choices[currentChoice].text.Substring(0, 2)!="->"){
			choices[currentChoice].text = "->"+choices[currentChoice].text;
		}
	}


	// Update is called once per frame
	void Update(){
		if(character!=null && transform.name==character.name){
			if(showingChoices){
				if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)){
					if(currentChoice<answer_choices.Length-1){
						currentChoice+=1;
						setSelector();
					} 
				}
				if(Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.W)){
					if(currentChoice>0){
						currentChoice-=1;
						setSelector();
					}
				}
			}
		}
	}



	public void hideChoices(){
		showingChoices = false;
		for (int i = 0; i < 4; i++) {
			GameObject s = GameObject.Find("Choice"+i);
			TextMeshProUGUI t = s.GetComponent<TextMeshProUGUI>();
			t.enabled = false;
		}
	}

	public void resetAndHideAll(){
		bubbleBackgroundImage.enabled = false;
		bubbleTextMesh.enabled = false;
		hideChoices();
		interactionCounter=0;
		character=null;
	}


	public string interaction(GameObject overlapCharacter){
		character=overlapCharacter;
		
		TextScript ts = character.GetComponent<TextScript>();
		if(!ts.firstInteractionDone){
			string[] sentences = ts.sentences;
			string[] answer_choices = ts.answer_choices;
			int correct_answer_index = ts.correct_answer_index;
		

			//show sentences if there are any
			if(sentences.Length>0 && interactionCounter<sentences.Length){
				//show speech bubble and question text
				bubbleBackgroundImage.enabled = true;
				bubbleTextMesh.enabled = true;
				bubbleTextMesh.text = sentences[interactionCounter];
				interactionCounter+=1;	
				return "disableMovement";
			}
			//show answer choices if there are any
			else if(answer_choices.Length>0 && interactionCounter==sentences.Length){
				currentChoice=0;
				bubbleBackgroundImage.enabled = true;
				bubbleTextMesh.enabled = false;//hide any previous text
				showingChoices = true;
				for (int i = 0; i < 4; i++) {
					//fill choice objects with answer choices
					if(i<=answer_choices.Length-1){
						GameObject s = GameObject.Find("Choice"+i);
						TextMeshProUGUI t = s.GetComponent<TextMeshProUGUI>();
						t.enabled = true;
						t.text = answer_choices[i];
						choices.Add(t);
					}
					//hide unused choice objects
					else{
						GameObject s = GameObject.Find("Choice"+i);
						TextMeshProUGUI t = s.GetComponent<TextMeshProUGUI>();
						t.enabled = false;
					}
				}
				setSelector();
				interactionCounter+=1;
				return "disableMovement";
			}
			//show conlusion sentences if there are any
			else if(interactionCounter>=sentences.Length+1){
				hideChoices();
				bubbleTextMesh.enabled = true;
				int adjusted_counter_to_index = interactionCounter-(sentences.Length+1);
				//player made the right choice
				Debug.Log(currentChoice);
				Debug.Log(correct_answer_index);
				if(currentChoice==correct_answer_index){
					Debug.Log("correct!");
					if(conclusion_right.Length>0 && adjusted_counter_to_index<conclusion_right.Length){
						bubbleTextMesh.text = conclusion_right[adjusted_counter_to_index];
						interactionCounter+=1;
						return "disableMovement";
					}
					else{
						resetAndHideAll();
						if(reaction_script != null)
						{
							reaction_script.react(true);
						}
						firstInteractionDone = true;
						return "enableMovement";
					}
				}
				//player made the wrong choice
				else{
					Debug.Log("incorrect!");
					if(conclusion_wrong.Length>0 && adjusted_counter_to_index<conclusion_wrong.Length){
						bubbleTextMesh.text = conclusion_wrong[adjusted_counter_to_index];
						interactionCounter+=1;
						return "disableMovement";
					}
					else{
						if(reaction_script != null)
						{
							reaction_script.react(false);
						}
						resetAndHideAll();
						return "enableMovement";
					}
				}
			}
			resetAndHideAll();
			return "enableMovement";
		}
		resetAndHideAll();
		return "enableMovement";
	}
	
}
