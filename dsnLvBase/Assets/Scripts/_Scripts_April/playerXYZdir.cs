using UnityEngine;
using System.Collections;

public class playerXYZdir : MonoBehaviour {
	public Transform sXY;
	public Transform sZY;
	float nextAngleY;

	// Use this for initialization
	void Start () {
	
	}
	
	void changeSpriteSc(Transform trans){
		trans.localScale = new Vector3( -trans.localScale.x, trans.localScale.y, trans.localScale.z );
	}

	void Update () {
		if(PlayerInputController.playerStatus==0){//left
			if(sXY.transform.localScale.x<0){
				nextAngleY=90;
			}else{
				nextAngleY=270;
			}
		}else{
			if(sZY.transform.localScale.x<0){
				nextAngleY=180;
			}else{
				nextAngleY=0;
			}
		}

	
		
		this.transform.eulerAngles=Vector3.Lerp(transform.eulerAngles,new Vector3(0,nextAngleY,0),Time.deltaTime*8);
	
	}
}
