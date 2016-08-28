using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class platformTriggerDetect : MonoBehaviour {
	[SerializeField]
	private Rotatable _rot;

	[SerializeField]
	private BlockInformation _blockInfoMom;

	void Start(){
		GetComponent<Rigidbody> ().isKinematic = true;
		GetComponent<Rigidbody> ().useGravity = false;
		GetComponent<Collider> ().isTrigger = true;
		this.tag = "3d";
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "3d" && transform.parent != other.transform.parent && transform.parent!= other.transform && other.transform.parent != transform) {
			if(_rot && _rot.IsRotating)
						_rot.shouldTurnBack = true;
//			if(_blockInfoMom && _blockInfoMom.moving){
//			//	_blockInfoMom.hitObstacle = true;
//			}
			}
			

	}
}
