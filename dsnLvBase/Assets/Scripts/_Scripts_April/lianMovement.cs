using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lianMovement : MonoBehaviour
{
		public PlayerInputController _inputController;
		public List<Animator> anim = new List<Animator> ();
		public float speedDampTime = 0.1f;  // The damping for the speed parameter
	[SerializeField]
	Renderer[] lianRender;


		void Update ()
		{
			
				float h = Input.GetAxis ("Horizontal");
				MovementManagement (h);

		}
		
		void MovementManagement (float horizontal)
		{
		if (_inputController.enableRunInput) {
						for (int i=0; i<anim.Count; i++) {
				anim [i].SetFloat ("Speed", 0);
								if (horizontal != 0f || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow))
										anim [i].SetFloat ("Speed", 5.5f, speedDampTime, Time.deltaTime);
						}
						foreach (Renderer r in lianRender) {
								r.material.color = Color.black;
						}
				} else {
			foreach (Renderer r in lianRender) {
				r.material.color = Color.grey * 0.8f;
			}
				}
		}

}