using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionScript : MonoBehaviour
{
	
	public GameObject player;
	private GameObject frog;


	// Start is called before the first frame update
	void Awake(){
		frog = GameObject.Find("NinjaFrog");
	}
	

	public void react(int reaction_index, string reacting_to)
	{
		if(reaction_index == 0){
			if(reacting_to == "talked"){
				Debug.Log("reacting to talking");
				if(gameObject.name=="TalkingBat"){
					enableTextScriptWithIndexForCharacter(frog,1);
				}
				if(gameObject.name=="MaskGuy"){
					enableTextScriptWithIndexForCharacter(frog,3);
				}
			}
			if(reacting_to == "correct"){
				Debug.Log("no action");
			}
			else if (reacting_to == "wrong"){
			}
		}



		if(reaction_index == 1){
			if(reacting_to == "talked"){
				Debug.Log("no action");
			}
			if(reacting_to == "correct"){
				if(gameObject.name=="NinjaFrog"){
					enableTextScriptWithIndexForCharacter(frog,2);
				}

			}
			else if (reacting_to == "wrong"){
				if(gameObject.name=="NinjaFrog"){
					GameObject FrogTeleportTarget = GameObject.Find("FrogTeleportTarget");
					if(FrogTeleportTarget){
						player.transform.position = FrogTeleportTarget.transform.position;
					}
					else{
						Debug.Log("teleport target not found");
					}
				}
			}
		}



		if(reaction_index == 2){
			if(reacting_to == "talked"){
				Debug.Log("no action");
			}
			if(reacting_to == "correct"){
				Debug.Log("no action");
			}
			else if (reacting_to == "wrong"){
				Debug.Log("no action");
			}
		}



		if(reaction_index == 3){
			if(reacting_to == "talked"){
				Debug.Log("no action");
			}
			if(reacting_to == "correct"){
				if(gameObject.name=="NinjaFrog"){
					BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
					boxCollider.enabled=false;
				}
			}
			else if (reacting_to == "wrong"){
				if(gameObject.name=="NinjaFrog"){
					GameObject FrogTeleportTarget = GameObject.Find("FrogTeleportTarget");
					if(FrogTeleportTarget){
						player.transform.position = FrogTeleportTarget.transform.position;
					}
					else{
						Debug.Log("teleport target not found");
					}
				}
			}
		}


	}

	private void enableTextScriptWithIndexForCharacter(GameObject character, int index){
		Component[] text_scripts;
		text_scripts = character.GetComponents<TextScript>();
		foreach (TextScript ts in text_scripts){
			if(ts.TextScript_index == index){
				ts.enabled=true;
			}
			else{
				ts.enabled=false;
			}
		}
	}
	private void disableTextScriptWithIndexForCharacter(GameObject character, int index){
		Component[] text_scripts;
		text_scripts = character.GetComponents<TextScript>();
		foreach (TextScript ts in text_scripts){
			if(ts.TextScript_index == index){
				ts.enabled=false;
			}
		}
	}

}
