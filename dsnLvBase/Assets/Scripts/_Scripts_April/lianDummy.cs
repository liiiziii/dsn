using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lianDummy : MonoBehaviour {
	public List<GameObject> myMesh= new List<GameObject>();
//	List<Color> meshColor= new List<Color>();
	public Character3D lianSelf;
	public int thisIndex;
	public int doesExist = 2;//0: start combining, +-1:moving. 2:after combing
	Vector3 splitedPos;
	Vector3 targetPos;


	private bool existedLastFrame;


	float animDuration = .5f;
	float animRamp = 2.5f;
	float timer = 0;

	Vector3 startPos;
	Vector3 endPos;
	float startColor;
	float endColor;


	void Start () {
		for(int i=0; i < myMesh.Count; i++){
		}
	
	}

//	void FixedUpdate () {
//
//		if(thisIndex==0){
//			splitedPos=new Vector3(lianSelf._targetPos.x,lianSelf._targetPos.y,-7);
//		}else if(thisIndex==1){
//			splitedPos=new Vector3(-7,lianSelf._targetPos.y,lianSelf._targetPos.z);
//		}
//
//		if(lianSelf.DoesExist){
//			if (!existedLastFrame)
//			{
//				timer = 0;
//				doesExist = 1;
//
//				startColor = 0;
//				endColor = 1;
//
//				foreach (GameObject go in myMesh)
//					go.GetComponent<Renderer>().enabled = true;
//
//			}
//		}else{
//
//			if (existedLastFrame)
//			{
//				timer = 0;
//				doesExist = -1;
//
//				startColor = 1;
//				endColor = 0;
//
//				foreach (GameObject go in myMesh)
//					go.GetComponent<Renderer>().enabled = true;
//			}
//		}
//
//		if (doesExist !=1 && doesExist !=-1 )
//			return;
//
//		float interp = Mathf.Clamp01(timer / animDuration);
//
//		// "Rest at peaks"
//		if (interp < .5f)
//		{
//			interp *= 2;
//			interp = Mathf.Pow(interp, animRamp);
//			interp *= .5f;
//		}
//		else
//		{
//			interp -= .5f;
//			interp *= 2;
//			interp = 1- interp;
//
//			interp = Mathf.Pow(interp, animRamp);
//
//			interp = 1 - interp;
//			interp *= .5f;
//			interp += .5f;
//		}
//
//
//		/*
//		// rest at start
//		interp = Mathf.Pow(interp, animRamp);
//		*/
//
//		/*
//		//rest at end
//
//		ramp: min = 1, max = whatever
//		
//		interp = 1 - interp;
//		interp = Mathf.Pow(interp, animRamp);
//		interp = 1 - interp;
//		*/
//
//		if (lianSelf.DoesExist)
//		{
//			startPos = splitedPos;
//			endPos = lianSelf._targetPos;
//		}
//		else
//		{
//			startPos = lianSelf._targetPos;
//			endPos = splitedPos;
//		}
//
//		transform.position = Vector3.Lerp(startPos, endPos, interp);
//		foreach (GameObject themesh in myMesh){
//			themesh.GetComponent<Renderer>().material.color = new Color(0,0,0, Mathf.Lerp(startColor, endColor, Mathf.Clamp01(timer /animDuration)));
//		}
//
//		existedLastFrame = lianSelf.DoesExist;
//
//		if (timer >= animDuration)
//		{
//			if(startPos == splitedPos)
//				doesExist = 0;
//			else if(startPos == lianSelf._targetPos)
//				doesExist = 2;
//			foreach (GameObject go in myMesh)
//				go.GetComponent<Renderer>().enabled = false;
//		}
//
//		timer += Time.deltaTime;
//		
//	}
	
}
