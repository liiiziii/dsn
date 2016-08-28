using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class btnForMovingController : MonoBehaviour {
	public List<BlockInformation> buttonsXY = new List<BlockInformation> ();
	public List<Movable> movablesXY = new List<Movable> ();
//	public List<BlockInformation> buttonsZY = new List<BlockInformation> ();
//	public List<Movable> movablesZY = new List<Movable> ();
//	public List<BlockInformation> buttons3D = new List<BlockInformation> ();
//	public List<Movable> movables3D = new List<Movable> ();

	void Start () {
	
	}

	void Update () {
		//type 1
		    int type1 = 0;
		if (buttonsXY [type1].SPCharBeTouched [0].x == 1) {
			if (movablesXY [type1].lastDir != 1 && movablesXY [type1].dir == 0) {
				movablesXY [type1].dir = 1;
				}
			}
		movablesXY [type1].chooseMoving ();
		//type2
		int type2 = 1;
		if (buttonsXY [type2].beTouched == 10) {
			if (movablesXY [type2].lastDir != 1 && movablesXY [type2].dir == 0) {
				movablesXY [type2].dir = 1;
			}
		}
		if (buttonsXY [type2].beTouched  != 10) {
			if (movablesXY [type2].lastDir != -1 && movablesXY [type2].dir == 0) {
				movablesXY [type2].dir = -1;
			}
		}
		movablesXY [type2].chooseMoving ();
	
	}
}
