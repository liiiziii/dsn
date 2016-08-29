using UnityEngine;
using System.Collections;

public class Movable : MonoBehaviour {
	[SerializeField]
	float speed = 0.1f;
	[SerializeField]
	Transform pointB;
	Vector3 pointAPos;
	Vector3 pointBPos;
	[HideInInspector]
	public int dir = 0;
	[HideInInspector]
	public int lastDir = 0;
	[SerializeField]
	bool autoMovement = false;
	int parkCounter = 0;
	int parkTime = 50;
	[SerializeField]
	BlockInformation thisBlock;
	[SerializeField]
	float lastMoveOffset = 0;
	//---------------------------------------------------------------------//
	void Start () {
		pointAPos = transform.position;
		pointBPos = pointB.position;
		if (autoMovement)
			lastDir = -1;
	}
	//---------------------------------------------------------------------//
	void Update () {
		autoMovementFunction (autoMovement);
	}
	//---------------------------------------------------------------------//
	void autoMovementFunction(bool a){
		if (a) {
			if (dir == 0 && lastDir == -1) {
				parkCounter--;
				if (parkCounter < 0) {
					lastDir = 0;
					dir = 1;
					parkCounter = parkTime;
				}
			}
			if (dir == 0 && lastDir == 1) {
				parkCounter--;
				if (parkCounter < 0) {
					lastDir = 0;
					dir = -1;
					parkCounter = parkTime;
				}
			}
			chooseMoving ();
		}
	}
	//---------------------------------------------------------------------//
	public void chooseMoving(){
		//after deciding direction, move the platform
		if (dir == 1) {
			performMoving (pointBPos, pointAPos, dir);
		}
		if (dir == -1) {
			performMoving (pointAPos, pointBPos, dir);
		}
	}
	//---------------------------------------------------------------------//
	void performMoving(Vector3 nextPos, Vector3 previousPos, int ld){
		Vector3 velocity = speed * (nextPos-previousPos).normalized;
		if (Vector3.Distance (transform.position, nextPos) > 0.1f) {
			transform.Translate (velocity);
			translateCharacters(velocity);
		} else {
			Vector3 deltaDist = nextPos - transform.position + new Vector3 (0, lastMoveOffset, 0);
			transform.position = nextPos;
			translateCharacters (deltaDist);
			lastDir = ld;
			dir = 0;
		}
	}
	//---------------------------------------------------------------------//
	void translateCharacters(Vector3 v){
		if (thisBlock.beTouched == 10) {
			WorldManager.g.char3D.transform.Translate (v);
		}
		for (int i = 0; i < thisBlock.SPCharBeTouched.Count; i++) {
			if (thisBlock.SPCharBeTouched [i].x == 1) {
				WorldManager.g.supportCharXY [i].Translate (v);
			}
			if (thisBlock.SPCharBeTouched [i].y == 1) {
				WorldManager.g.supportCharZY [i].Translate (v);
			}
		}
	}
	//---------------------------------------------------------------------//
}
