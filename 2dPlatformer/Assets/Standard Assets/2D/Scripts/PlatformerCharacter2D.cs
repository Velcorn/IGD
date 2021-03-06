using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable 649
namespace UnityStandardAssets._2D
{
	public class PlatformerCharacter2D : MonoBehaviour
	{
		[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
		[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
		[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
		[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
		[SerializeField] private string m_PlatformsLayer;  
		[SerializeField] private string m_CharactersLayer;  

		private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
		const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		private bool m_Grounded;            // Whether or not the player is grounded.
		private Transform m_CeilingCheck;   // A position marking where to check for ceilings
		const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
		private Animator m_Anim;            // Reference to the player's animator component.
		private Rigidbody2D m_Rigidbody2D;
		private bool m_FacingRight = true;  // For determining which way the player is currently facing.
		private bool overlapPlatform = false;
		private GameObject overlapCharacter = null;
		private BoxCollider2D boxCollider;
		private string movement = "enableMovement";

		private void Awake()
		{
			// Setting up references.
			m_GroundCheck = transform.Find("GroundCheck");
			m_CeilingCheck = transform.Find("CeilingCheck");
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			boxCollider = GetComponent<BoxCollider2D>();

		}

		private void OnTriggerEnter2D(Collider2D collider){
			if(collider.gameObject.layer==LayerMask.NameToLayer(m_PlatformsLayer)){
				overlapPlatform = true;
			}
			if(collider.gameObject.layer==LayerMask.NameToLayer(m_CharactersLayer)){
				overlapCharacter = collider.gameObject;
			}
		}

		private void OnTriggerExit2D(Collider2D collider){
			if(collider.gameObject.layer==LayerMask.NameToLayer(m_PlatformsLayer)){
				overlapPlatform = false;
			}
			if(collider.gameObject.layer==LayerMask.NameToLayer(m_CharactersLayer)){
				TextScript ts = overlapCharacter.GetComponent<TextScript>();
				ts.resetAndHideAll();
				overlapCharacter = null;
			}
		}


		private void FixedUpdate()
		{

			m_Grounded = false;
			transform.parent = null;

			//disable player collider if moving up and colliding with a platform
			if(m_Rigidbody2D.velocity.y>0 && overlapPlatform){
				boxCollider.enabled = false;
			}

			else if(!overlapPlatform){
				boxCollider.enabled = true;
			}

			// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			// This can be done using layers instead but Sample Assets will not overwrite your project settings.
			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					m_Grounded = true;
					//move player with platform by setting the platform as parent
					if (colliders[i].gameObject.layer==LayerMask.NameToLayer(m_PlatformsLayer))
					{
						transform.parent = colliders[i].gameObject.transform;
					}
					else{
						transform.parent = null;
					}
				}
			}
			m_Anim.SetBool("Ground", m_Grounded);

			// Set the vertical animation
			m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

		}


		public void Move(float move, bool crouch, bool jump)
		{
			
			if (movement!="disableMovement") {
				// If crouching, check to see if the character can stand up
				if (!crouch && m_Anim.GetBool("Crouch"))
				{
					// If the character has a ceiling preventing them from standing up, keep them crouching
					if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
					{
						Collider2D obj = Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround);
						if(obj.gameObject.name!=transform.name){
							crouch = true;
						}
						
					}
				}

				// Set whether or not the character is crouching in the animator
				m_Anim.SetBool("Crouch", crouch);

				//reduce collider height if crouching
				if(crouch){
					boxCollider.offset = new Vector2(0.0f, -0.37f);
					boxCollider.size = new Vector2(0.25f, 0.5f);
				}
				else{
					boxCollider.offset = new Vector2(0.0f, -0.08f);
					boxCollider.size = new Vector2(0.25f, 1.06f);
				}


				//only control the player if grounded or airControl is turned on
				if (m_Grounded || m_AirControl)
				{
					// Reduce the speed if crouching by the crouchSpeed multiplier
					move = (crouch ? move*m_CrouchSpeed : move);

					// The Speed animator parameter is set to the absolute value of the horizontal input.
					m_Anim.SetFloat("Speed", Mathf.Abs(move));

					// Move the character
					m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

					// If the input is moving the player right and the player is facing left...
					if (move > 0 && !m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
						// Otherwise if the input is moving the player left and the player is facing right...
					else if (move < 0 && m_FacingRight)
					{
						// ... flip the player.
						Flip();
					}
				}
				// If the player should jump...
				if (m_Grounded && jump && m_Anim.GetBool("Ground"))
				{
					// Add a vertical force to the player.
					m_Grounded = false;
					m_Anim.SetBool("Ground", false);
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				}
			}
			else
			{
				m_Anim.SetFloat("Speed", 0);
			}

		}


		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;
			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		public void Interact()
		{
			if (overlapCharacter!=null)
			{
				Component[] text_scripts;
				text_scripts = overlapCharacter.GetComponents<TextScript>();
				TextScript ts = null;
				foreach (TextScript script in text_scripts){
					if(script.enabled==true){
						ts = script;
					}
				}
				if(ts!=null)
				{
					movement = ts.interaction(overlapCharacter);
				}
			}
		}


	}
}
