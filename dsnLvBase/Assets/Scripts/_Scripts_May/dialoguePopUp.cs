using UnityEngine;
using System.Collections;

public class dialoguePopUp : MonoBehaviour {
	public float animDuration = .9f;
	float animRamp = 2.5f;
	float timer = 0;
//	int counter = 50;
	public bool enableDialogue = false;
	bool appearing =false;
	public float interp;
	Vector3 scale1;
	Vector3 scale2;


	void FixedUpdate () {

		if(enableDialogue){
			if(!appearing){
				timer =0;
			scale1 = new Vector3(0,0,0);
			scale2 = new Vector3(1,1,1);
				appearing = true;
			}
		}else
			if(appearing){
			timer = 0;
				scale1 = new Vector3(1,1,1);
				scale2 = new Vector3(0,0,0);
			appearing =false;
			}
		if(transform.localScale!=scale2)
		scaleAnimation(scale1,scale2);
	
	}

	void scaleAnimation(Vector3 s1, Vector3 s2){
		interp = Mathf.Clamp01(timer / animDuration);
		
		if (interp < .5f)
		{
			interp *= 2;
			interp = Mathf.Pow(interp, animRamp);
			interp *= .5f;
		}
		else
		{
			interp -= .5f;
			interp *= 2;
			interp = 1- interp;

			interp = Mathf.Pow(interp, animRamp);
			
			interp = 1 - interp;
			interp *= .5f;
			interp += .5f;
		}
		this.transform.localScale = Vector3.Lerp(s1,s2,interp);
		timer += Time.deltaTime;
	}
}
