using UnityEngine;
using System.Collections;

public class dialogueBubbleIdle : MonoBehaviour {
	public float deltaY;
	float min =0;
	float max =1;
	bool triggerIdle = false;
	public dialoguePopUp _popupControl;
	int counter = 40;

	void Start () {

	}
	

	void FixedUpdate () {
		if( _popupControl.interp == 1 && counter > -1){
			counter--;
		}
		if(triggerIdle)
		transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 0.2f,max-min)+min, transform.position.z);

		if(!triggerIdle && counter < 0){
			min=transform.position.y-deltaY;
			max=transform.position.y;
			triggerIdle = true;
		}
		
	}
}
