using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public static WorldManager g;
	[SerializeField]
	Plane[] _planes;
	public Transform startPlatform;
	public Transform char3D;
	public Transform charXY;
	public Transform charZY;
	public Transform[] supportCharXY;
	public Transform[] supportCharZY;


	public Plane[] Planes {
		get { return _planes; }
	}

	void Awake () {
		if (g == null) {
			g = this;
		} else {
			Destroy(this);
		}
		Vector3 pos = startPlatform.position;
		pos.y = startPlatform.position.y + startPlatform.lossyScale.y / 2 + char3D.lossyScale.y / 2 + 0.4f;
		char3D.transform.position = pos;
		charXY.position = new Vector3 (pos.x, pos.y, charXY.position.z);
		charZY.position = new Vector3 (- pos.z, pos.y, charZY.position.z);
	}

}
