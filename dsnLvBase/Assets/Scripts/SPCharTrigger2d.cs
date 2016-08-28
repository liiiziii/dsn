using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SPCharTrigger2d : MonoBehaviour
{
	int indexInBlockInfo;
	[SerializeField]
	int
		_myIndex;
	[SerializeField]
	SPCharInputCtr _charInputCtr;

	void Start(){
		indexInBlockInfo = _charInputCtr.indexInBlockInfo;
	}

	public void OnTriggerStay2D (Collider2D col)
	{
				
		if (col && col.GetComponent<ShadowRenderer> ()) {
			if (_myIndex == 0 && col.gameObject.layer == 11)
				addBottomObjects (0, col.GetComponent<ShadowRenderer> ().parentObj.GetComponent<BlockInformation> (), _charInputCtr.pXYhitObj, 0);
			if (_myIndex == 1 && col.gameObject.layer == 12)
				addBottomObjects (1, col.GetComponent<ShadowRenderer> ().parentObj.GetComponent<BlockInformation> (), _charInputCtr.pZYhitObj, 1);
		}
	}

	public void OnTriggerExit2D (Collider2D col)
	{
		if (col.GetComponent<ShadowRenderer> ()) {
			removeObj (0, col.GetComponent<ShadowRenderer> ().parentObj.GetComponent<BlockInformation> (), _charInputCtr.pXYhitObj);
			removeObj (1, col.GetComponent<ShadowRenderer> ().parentObj.GetComponent<BlockInformation> (), _charInputCtr.pZYhitObj);
		}
	}

	void removeObj (int i, BlockInformation go, List<BlockInformation> k)
	{
		if (i == _myIndex) {
			if (k.Contains (go)) {
				if (go) {	
					if(go.SPCharBeTouched.Count > 0){
					Vector2 touched = go.SPCharBeTouched [indexInBlockInfo];
					if (i == 0)
						go.SPCharBeTouched [indexInBlockInfo] = new Vector2 (0,touched.y);
					if (i == 1)
						go.SPCharBeTouched [indexInBlockInfo] = new Vector2 (touched.x,0);
					}
				}
				k.Remove (go);
			}
		}
	}

	void addBottomObjects (int i, BlockInformation go, List<BlockInformation> k, int n)
	{
		if (i == _myIndex) {
			if (!k.Contains (go)) {
				k.Add (go);
			}
			if (go) {
				if (go.SPCharBeTouched.Count > 0) {
					Vector2 touched = go.SPCharBeTouched [indexInBlockInfo];
					if (i == 0)
						go.SPCharBeTouched [indexInBlockInfo] = new Vector2 (1, touched.y);
					if (i == 1)
						go.SPCharBeTouched [indexInBlockInfo] = new Vector2 (touched.x, 1);
				}
			}	
		}
	}

}

