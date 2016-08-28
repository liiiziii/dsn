using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockInformation : RaycastController{
	int SPCharIndexCount = 1;
	public List<Vector2> SPCharBeTouched = new List<Vector2>();//x: touch XY; y: touch ZY; 1 touch, 0 no touch
	[SerializeField]
	bool isCharacter = false;
	public GameObject landOnGO;
	[SerializeField]
	private Rotatable _rotatable;
	public Rotatable MyRotatable {
		get {return _rotatable;}
	}
	
    public int beTouched = 2;//2: touches nothing
    public bool CanRotate
    {
        get { return _rotatable != null; }
    }
    [HideInInspector]
    public float rotOnce = 0;
	[HideInInspector]
	public bool enteredControl = false;
	[SerializeField]
	Renderer thisMesh;

	public override void Start () {
		//for Support charater
		for (int i = 0; i < SPCharIndexCount; i++) {
			SPCharBeTouched.Add (new Vector2(0,0));
		}
		base.Start ();
    }

	void Update(){
		//the block changes its color when shadow characters standing there
//		thisMesh.material.color = Color.white * 0.8f;
//		foreach (Vector2 i in SPCharBeTouched) {
//			if (i.x == 1 || i.y == 1) {
//				thisMesh.material.color = Color.gray * 1.2f;
//			}
//		}

		if (!isCharacter)
						return;
		UpdateRaycastOrigins ();
		calculateGroundRays (new Vector3 (0, 1, 0));



	}

	void calculateGroundRays(Vector3 rayDir){
		//float directionX = Mathf.Sign (rayDir.x);
		float directionY = Mathf.Sign (rayDir.y);
		float rayLength = Mathf.Abs (rayDir.y) + skinWidth;
		RaycastHit hit;

			Vector3 rayOrigin = raycastOrigins.bottomLeft + Vector3.right * (groundRaySpacing * 1) +Vector3.forward * (groundRaySpacing * 1);
			if( Physics.Raycast(rayOrigin, -Vector3.up * directionY, out hit)){
				if( hit.collider!= null && hit.collider.gameObject.GetComponent<BlockInformation>()){
					if(landOnGO && landOnGO != hit.collider.gameObject)
						landOnGO.GetComponent<BoxCollider>().enabled = true;
					landOnGO = hit.collider.gameObject;
					landOnGO.GetComponent<BoxCollider>().enabled = false;
				}


			Debug.DrawRay(rayOrigin, -Vector3.up * directionY * rayLength,Color.red);
		}

	}

}
