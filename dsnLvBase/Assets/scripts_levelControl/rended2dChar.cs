using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rended2dChar : MonoBehaviour {
		public List<Animator> anim= new List<Animator>();
		public float speedDampTime = 0.1f;  // The damping for the speed parameter


		
		void Update ()
		{
		for(int i=0; i<anim.Count;i++){
			float h = Input.GetAxis("Horizontal");
			bool jump = false;
			MovementManagement(h, jump);
		if(PlayerInputController.playerStatus != i){
				anim[i].SetFloat("Speed", 0);
				anim[i].SetBool("idleFat",false);
				anim[i].SetBool("rotating",false);
				anim[i].SetBool("Jump", false);
		}
		}
		}
		
		
		void MovementManagement (float horizontal,  bool jumping)
		{
//			for(int i=0; i<anim.Count;i++){
//				anim[i].SetBool("Jump", jumping);
//				if(horizontal != 0f || Input.GetKey(KeyCode.LeftArrow) ||  Input.GetKey(KeyCode.RightArrow) ||  Input.GetKey(KeyCode.DownArrow))
//				{
//					anim[i].SetFloat("Speed", 5.5f, speedDampTime, Time.deltaTime);
//				}
//				else{
//					anim[i].SetFloat("Speed", 0);
//				}
//				if(Character3D._touch3dObj && Character3D._touch3dObj.GetComponent<BlockInformation>() && Character3D._touch3dObj.GetComponent<BlockInformation>().CanRotate  
//				   && Character3D._touch3dObj.parent.GetComponent<Rotatable>() && Character3D._touch3dObj.parent.GetComponent<Rotatable>()._canBeDrivenByCharacter){
//					if(Character3D.IsRotating){
//						anim[i].SetBool("rotating",true);
//					}else{
//						anim[i].SetBool("rotating",false);
//					}
//					anim[i].SetBool("idleFat",true);
//					
//					
//				}else{
//					anim[i].SetBool("idleFat",false);
//					
//				}
//			}
		}

	}