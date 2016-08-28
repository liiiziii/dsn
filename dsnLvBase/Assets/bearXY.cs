using UnityEngine;
using System.Collections;

public class bearXY : MonoBehaviour {
	[SerializeField]
	int thisStatusIndex = 0;
	[SerializeField]
	Transform _anXY;
	[SerializeField]
	SPCharCtr2d _thisChar2D;
	[SerializeField]
	SPCharInputCtr otherCtr;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update()
	{
		//if (PlayerInputController.playerStatus == thisStatusIndex) {
			otherCtr.directions[thisStatusIndex] = 0;
			moveThisChar (_thisChar2D.closestObjL, 1);
			moveThisChar (_thisChar2D.closestObjR, -1);
		//}
	}

	void moveThisChar(RaycastHit2D hit, int dir){
		if (hit && hit.transform == _anXY) {
			if (Vector3.Distance (_anXY.transform.position, _thisChar2D.transform.position) > 2.2f) {
				otherCtr.directions[thisStatusIndex] = dir;
				//_thisChar2D.transform.position = Vector3.MoveTowards (_thisChar2D.transform.position, new Vector3 (_anXY.transform.position.x, _anXY.transform.position.y, _thisChar2D.transform.position.z), Time.deltaTime * speed);
			}
		}
			
	}
}
