using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionScript : MonoBehaviour
{

	public void react()
	{
		Debug.Log("reacting");
		Debug.Log(gameObject.name);
		if(gameObject.name=="MaskGuy"){
			BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.enabled=false;
		}
		if(gameObject.name=="NinjaFrog"){
			BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.enabled=false;
		}
	}

}
