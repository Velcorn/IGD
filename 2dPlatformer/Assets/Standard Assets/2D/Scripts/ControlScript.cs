using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{

	public class ControlScript : MonoBehaviour
	{
		private CharacterScript m_Character;
		private bool m_Jump;
		private bool m_Interaction;


		private void Awake()
		{
			m_Character = GetComponent<CharacterScript>();
		}


		private void Update()
		{
			if (!m_Jump)
			{
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
			if (!m_Interaction)
			{
				m_Interaction = Input.GetKeyDown(KeyCode.E);
			}

		}


		private void FixedUpdate()
		{
			// Read the inputs.
			bool crouch = Input.GetKey(KeyCode.LeftShift);
			float h = CrossPlatformInputManager.GetAxis("Horizontal");
			// Pass all parameters to the character control script.
			m_Character.Move(h, crouch, m_Jump);
			m_Jump = false;
			if (m_Interaction){
				m_Character.Interact();
				m_Interaction = false;
			}
		}
	}
}
