using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionScript : MonoBehaviour
{
	
	public GameObject player;
	public Transform FrogTeleportTarget;

	public void react(bool correct)
	{
		if(correct){
			Debug.Log("reacting to correct answer");
			Debug.Log(gameObject.name);
			if(gameObject.name=="MaskGuy"){
				BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
				boxCollider.enabled=false;
			}
			if(gameObject.name=="NinjaFrog"){
				Debug.Log("no action");
			}
		}
		else{
			Debug.Log("reacting to incorrect");
			Debug.Log(gameObject.name);
			if(gameObject.name=="MaskGuy"){
				Debug.Log("no action");
			}
			if(gameObject.name=="NinjaFrog"){
				player.transform.position = FrogTeleportTarget.transform.position;
			}
		}
	}

}
