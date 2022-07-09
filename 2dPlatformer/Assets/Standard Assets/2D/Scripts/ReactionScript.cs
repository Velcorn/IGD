using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionScript : MonoBehaviour
{
	
	public GameObject player;
	private GameObject frog;
	private GameObject shaman;
	private GameObject bat;
	private GameObject selene;
	private GameObject ghost;

	private Animator anim;
	public static Dictionary<GameObject, int> current_state;
	
	void Awake(){
		frog = GameObject.Find("NinjaFrog");
		shaman = GameObject.Find("MaskGuy");
		bat = GameObject.Find("TalkingBat");
		selene = GameObject.Find("Selene");
		ghost = GameObject.Find("Ghost");

		current_state = new Dictionary<GameObject, int>(){
			{frog, 0},
			{shaman, 0},
			{bat, 0},
			{ghost, 0},
		};

		anim = GetComponent<Animator>();
	}

	public void react(int reaction_index, string reacting_to)
	{
		if (reaction_index == 0)
		{
			if (reacting_to == "talked")
			{
				if (gameObject == bat)
				{
					if (current_state[bat] == 1)
					{
						enableTextScriptWithIndexForCharacter(bat, 2);
					}
					enableTextScriptWithIndexForCharacter(frog, 1);
					current_state[gameObject] += 1;
				}
				else if (gameObject == shaman)
				{
					if (current_state[shaman] == 1)
					{
						enableTextScriptWithIndexForCharacter(shaman, 2);
					}
					if (current_state[frog]>=1)
					{
						enableTextScriptWithIndexForCharacter(frog, 3);
					} 
					current_state[gameObject] += 1;
				}
				else if (gameObject == selene)
				{
					Destroy(selene.GetComponent<TextScript>());
					anim.Play("seleneExplosionAnimation");
					Destroy(selene, 1.0f);	
				}
				else if (gameObject == ghost)
				{
					enableTextScriptWithIndexForCharacter(ghost, 1);
				}
			}
			if (reacting_to == "correct"){
			}
			else if (reacting_to == "wrong"){
			}
		}

		if (reaction_index == 1)
		{
			if (reacting_to == "talked")
			{
			}
			if (reacting_to == "correct")
			{
				if (gameObject == frog)
				{
					if (current_state[shaman]>=1)
					{
						enableTextScriptWithIndexForCharacter(frog,3);
					}
					else
					{
						enableTextScriptWithIndexForCharacter(frog,2);
					}
				}
			}
			else if (reacting_to == "wrong")
			{
				if (gameObject == frog)
				{
					GameObject FrogTeleportTarget = GameObject.Find("FrogTeleportTarget");
					if (FrogTeleportTarget)
					{
						player.transform.position = FrogTeleportTarget.transform.position;
					}
					else
					{
						Debug.Log("teleport target not found");
					}
				}
			}
		}
		
		if (reaction_index == 2)
		{
			if (reacting_to == "talked")
			{
			}
			if (reacting_to == "correct")
			{
			}
			else if (reacting_to == "wrong")
			{
			}
		}
		
		if (reaction_index == 3)
		{
			if (reacting_to == "talked")
			{
			}
			if (reacting_to == "correct")
			{
				if(gameObject == frog){
					BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
					boxCollider.enabled = false;
				}
			}
			else if (reacting_to == "wrong")
			{
				if (gameObject == frog)
				{
					GameObject FrogTeleportTarget = GameObject.Find("FrogTeleportTarget");
					if (FrogTeleportTarget)
					{
						player.transform.position = FrogTeleportTarget.transform.position;
					}
					else
					{
						Debug.Log("teleport target not found");
					}
				}
			}
		}

		foreach (var item in current_state)
		{
		    //Debug.Log(item.Key.name + " state: " + item.Value);
		}

	}

	private void enableTextScriptWithIndexForCharacter(GameObject character, int index)
	{
		Component[] text_scripts;
		text_scripts = character.GetComponents<TextScript>();
		foreach (TextScript ts in text_scripts)
		{
			if (ts.TextScript_index == index)
			{
				ts.enabled = true;
			}
			else
			{
				ts.enabled = false;
			}
		}
		if (current_state.ContainsKey(character)) 
		{
			current_state[character] = index;
		}
	}
}
