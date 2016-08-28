using UnityEngine;
using System.Collections;

public class backToCheckPoint : MonoBehaviour {
	bool goBack = false;
	[SerializeField]
	GameObject char2D;
	[HideInInspector]
	public Vector3 checkPoint;
	
	void Update () {
		if(goBack){
			Application.LoadLevel(Application.loadedLevel);
			//char2D.transform.position = checkPoint;
			goBack = false;
			
		}
	
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.name=="triggerXY"){// && this.gameObject.layer == 14
			goBack=true;
		}
		if(other.name=="triggerZY"){// && this.gameObject.layer == 15
			goBack=true;
		}
	}
}
