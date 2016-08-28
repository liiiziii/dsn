using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {
	public Vector3 Origin {
		get { return transform.position; }
	}
	public Vector3 Up {
		get { return -transform.forward; }
	}
	public Vector3 Right {
		get { return -transform.right; }
	}
	public Vector3 Normal {
		get { return transform.up; }
	}
	public int Layer {
		get { return gameObject.layer; }
	}

	public int myIndex;
	Color nextColor;
	public Color activeColor;
	public Color oColor;
	public GameObject[] otherChildren;
	[SerializeField]
	Light currentLight;
	[HideInInspector]
	public bool isMe = false;

	void Update(){
		//if(this.transform.FindChild("quad"))
		//this.transform.FindChild("quad").GetComponent<Renderer>().material.color=Color.Lerp(this.transform.FindChild("quad").GetComponent<Renderer>().material.color,nextColor,Time.deltaTime*6);
		if(otherChildren.Length>0)
		foreach(GameObject k in otherChildren)
		k.GetComponent<Renderer>().material.color=Color.Lerp(k.GetComponent<Renderer>().material.color,nextColor,Time.deltaTime*6);

		if(PlayerInputController.playerStatus==myIndex){
			isMe=true;
			nextColor=activeColor;
			currentLight.intensity = Mathf.Lerp(currentLight.intensity,1.3f, Time.deltaTime*6);
		}else{
			isMe=false;
			nextColor=oColor;
			currentLight.intensity = Mathf.Lerp(currentLight.intensity,0, Time.deltaTime*6);
		}
	}
}
