using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactionScript : MonoBehaviour
{
	
	public GameObject player;
	private GameObject frog;
	private GameObject shaman;
	private GameObject bat;
	private GameObject selene;
	private GameObject ghost;
	private GameObject turtle;
	private GameObject talisman;
	private GameObject frog_2;
	private GameObject pig;

	private Animator anim;
	public static Dictionary<GameObject, int> current_state;
	
	void Awake(){
		frog = GameObject.Find("NinjaFrog");
		shaman = GameObject.Find("MaskGuy");
		bat = GameObject.Find("TalkingBat");
		selene = GameObject.Find("Selene");
		ghost = GameObject.Find("Ghost");
		turtle = GameObject.Find("Turtle");
		talisman = GameObject.Find("Gegenstand");
		frog_2 = GameObject.Find("NinjaFrog_2");
		pig = GameObject.Find("GreenPig");

		current_state = new Dictionary<GameObject, int>(){
			{frog, 0},
			{shaman, 0},
			{bat, 0},
			{ghost, 0},
			{turtle, 0},
			{talisman, 0},
			{pig,0},
			{frog_2,0}
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
					if (SceneManager.GetActiveScene().name == "EndScene")
					{
			            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
				}
				else if (gameObject == ghost)
				{
					enableTextScriptWithIndexForCharacter(ghost, 1);
					enableTextScriptWithIndexForCharacter(frog_2, 1);
				}
				else if (gameObject == turtle)
				{
					enableTextScriptWithIndexForCharacter(turtle, 1);
					enableTextScriptWithIndexForCharacter(talisman, 1);
				}
				else if (gameObject == pig)
				{
					enableTextScriptWithIndexForCharacter(pig,1);
					if(current_state[frog_2]==4)
					{
						enableTextScriptWithIndexForCharacter(frog_2, 5);
					}
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
				if (gameObject == talisman)
				{
					if (current_state[turtle] == 1)
					{
						current_state.Remove(talisman);
						Destroy(talisman.GetComponent<TextScript>());
						Destroy(talisman);
						enableTextScriptWithIndexForCharacter(turtle, 2);
					}
				}
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
				if (gameObject == frog_2)
				{
					if(current_state[turtle]<2){
						enableTextScriptWithIndexForCharacter(frog_2,2);
					}
					else
					{
						enableTextScriptWithIndexForCharacter(frog_2,3);
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
				if (gameObject == frog_2)
				{
					GameObject Frog_2TeleportTarget1 = GameObject.Find("Frog_2TeleportTarget1");
					if (Frog_2TeleportTarget1)
					{
						player.transform.position = Frog_2TeleportTarget1.transform.position;
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
				if(gameObject == turtle)
				{
					if(current_state[frog_2]==2)
					{
						enableTextScriptWithIndexForCharacter(frog_2, 3);
					}
				}
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
				if(gameObject == frog_2){
					if (current_state[pig]>=1)
					{
						enableTextScriptWithIndexForCharacter(frog_2,5);
					}
					else
					{
						enableTextScriptWithIndexForCharacter(frog_2,4);
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
				if (gameObject == frog_2)
				{
					GameObject Frog_2TeleportTarget2 = GameObject.Find("Frog_2TeleportTarget2");
					if (Frog_2TeleportTarget2)
					{
						player.transform.position = Frog_2TeleportTarget2.transform.position;
					}
					else
					{
						Debug.Log("teleport target not found");
					}
				}
			}
		}
		if (reaction_index == 5)
		{
			if (reacting_to == "correct")
			{
				if(gameObject == frog_2){
					BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
					boxCollider.enabled = false;
				}
			}
			else if (reacting_to == "wrong")
			{
				if (gameObject == frog_2)
				{
					GameObject Frog_2TeleportTarget3 = GameObject.Find("Frog_2TeleportTarget3");
					if (Frog_2TeleportTarget3)
					{
						player.transform.position = Frog_2TeleportTarget3.transform.position;
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
			Debug.Log(item.Key.name + " state: " + item.Value);
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
