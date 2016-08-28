using UnityEngine;
using System.Collections;

public class trigger2D : MonoBehaviour {
	[HideInInspector]
	public bool triggered=false;

	void OnTriggerStay2D(Collider2D other) {
		if(other.name=="triggerXY" && this.gameObject.layer == 14){
			triggered=true;
		}
		if(other.name=="triggerZY" && this.gameObject.layer == 15){
			triggered=true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.name=="triggerXY" && this.gameObject.layer == 14)
			triggered=false;
		if(other.name=="triggerZY" && this.gameObject.layer == 15)
			triggered=false;
	}

}
