using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fruitController : MonoBehaviour {
	public List<Transform> fruits = new List<Transform> ();
	[SerializeField]
	Character3D an3D;

	void Update () {
		foreach (Transform trans in fruits)
			if (an3D.touchedObjects.Contains(trans))
				trans.gameObject.SetActive (false);
	}
}
