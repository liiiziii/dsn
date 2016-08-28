using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class trigger3D : MonoBehaviour {
	[SerializeField]
	TextMesh textHint;
	[SerializeField]
	Renderer bg;
	public List<GameObject> triggeredObj =  new List<GameObject>();
	[SerializeField]
	int playerIndex;
	[SerializeField]
	Transform followObject;
	int activeControl = -1;
	[SerializeField]
	PlayerInputController _inputController;

	void Start(){
		bg.material.color = Color.white;
	}

	void Update(){
		textHint.text = " ";
		thisPosition ();
		if (PlayerInputController.playerStatus == playerIndex) {
			if (triggeredObj.Count < 1) {
				_inputController.enableRunInput = true;
				bg.material.color = Color.white;
				activeControl = -1;
				return;
			}
			if (Input.GetKeyDown (_inputController.enterBtn)) {
				_inputController.enableRunInput = true;
				activeControl = -activeControl;
			}
			//enter 2d controlled objects
			if(activeControl == 1){
				textHint.text = "{"+_inputController.rightBtn.ToString()+"/"+_inputController.leftBtn.ToString()+"/"+_inputController.enterBtn.ToString()+"}";
				_inputController.enableRunInput = false;
				bg.material.color = Color.grey;
				triggeredObjAvailability(true);
			}else{
				textHint.text = "{"+_inputController.enterBtn.ToString()+"}";
				//back to 3d control
				_inputController.enableRunInput = true;
				bg.material.color = Color.white;
				triggeredObjAvailability(false);
			}
		}
	}

	void triggeredObjAvailability(bool b){
		foreach (GameObject g in triggeredObj) {
			g.GetComponent<BlockInformation>().enteredControl = b;
				}
	}

	void thisPosition (){
		//get my posistion
		if (followObject) 
			if (playerIndex == 1)
				transform.position = new Vector3 (transform.position.x, followObject.position.y, followObject.position.z);
		else if (playerIndex == 0)
			transform.position = new Vector3 (followObject.position.x, followObject.position.y, transform.position.z);
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<BlockInformation>()) {
			other.GetComponent<BlockInformation>().beTouched = playerIndex;
			triggeredObj.Add(other.gameObject);
				}
	}

	void OnTriggerExit(Collider other) {
		if (other.GetComponent<BlockInformation>()) {
			triggeredObj.Remove(other.gameObject);
			other.GetComponent<BlockInformation>().enteredControl = false;
			other.GetComponent<BlockInformation>().beTouched = 2;
		}
	}
}
