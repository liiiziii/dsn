using UnityEngine;
using System.Collections;

public class bearZY : MonoBehaviour {
	[SerializeField]
	int thisStatusIndex = 1;
	[SerializeField]
	SPCharCtr2d _thisChar2D;
	[SerializeField]
	SPCharInputCtr otherCtr;
	Vector3 hitL;
	Vector3 hitR;
	float distL = 0;
	float distR = 0;

	float deltaDist = .1f;
	int moving = 0;
	Vector3 nextPos;
	int countdown = 100;
	int countdownTimer = 0;

	void Update()
	{
		//if (PlayerInputController.playerStatus == thisStatusIndex) {
			otherCtr.directions [thisStatusIndex] = 0;
			if (moving == 0) {
				countdownTimer--;
				if(countdownTimer < 0)
				chooseMovement();
			} else {
				float dist = Mathf.Abs (_thisChar2D.transform.position.x - nextPos.x);
				if (dist < deltaDist) {
					moving = 0;
					countdownTimer = countdown;
				}
			}
			otherCtr.directions[thisStatusIndex] = moving;
		//}
	}

	void chooseMovement(){
		distL = 0;
		distR = 0;
		distToChar (_thisChar2D.closestObjL, 1);
		distToChar (_thisChar2D.closestObjR, -1);
		if (distL > deltaDist && distL > distR) { 
			nextPos = hitL + new Vector3(0.7f,0,0);
			moving = 1;
		}
		if (distR > deltaDist && distR > distL) {
			nextPos = hitR + new Vector3(-0.7f,0,0);
			moving = -1;
		}
	}

	void distToChar(RaycastHit2D hit, int side){
		if (hit && hit.transform.GetComponent<ShadowRenderer>().parentObj.tag == "bird") {
			if (side == 1) {
				distL = hit.distance;
				hitL = _thisChar2D.transform.position + new Vector3 (hit.distance, 0, 0);
			}
			if (side == -1) {
				distR = hit.distance;
				hitR = _thisChar2D.transform.position + new Vector3 (-hit.distance, 0, 0);
			}
		}
	}
}