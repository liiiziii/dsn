using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character3D : RaycastController
{
	[SerializeField]
	PlayerInputController _inputController;
	[SerializeField]
	float _raycastEpsilon = 0.1f;
	[SerializeField]
	float _pivotGridPrecision = 0f;

	private float _rotationDuration = 0.65f;
	private Rotatable _activeRotator = null;
	private static bool _isRotating = false;
	// XXX
	public static bool IsRotating {
		get { return _isRotating; }
	}

	Quaternion _nextRot;
	[SerializeField]
	GameObject _playerxy;
	[SerializeField]
	GameObject _playerzy;
	Transform _playerxyTransform;
	Transform _playerzyTransform;
	BoxCollider2D _playerxyCollider;
	BoxCollider2D _playerzyCollider;
	Transform _transform;

	GameObject groundedObject;
	public Vector3 groundedRayDir;
	List<Vector2> groundedRayIndex = new List<Vector2> ();
	//[HideInInspector]
	public List<int> disabledDir = new List<int> ();

	public Vector3 runningRayDir;
	public GameObject runningToObject;
	float runningDir = 1;

	RaycastHit forHitObj;
	float hitDetectDist = 1f;
	public List<Transform> touchedObjects = new List<Transform> ();

	public override void Start ()
	{
		base.Start ();
		disabledDir.Clear ();
		disabledDir.Add (0);
		disabledDir.Add (0);
		_playerxyTransform = _playerxy.transform;
		_playerzyTransform = _playerzy.transform;
		_playerxyCollider = _playerxy.GetComponent<BoxCollider2D> ();
		_playerzyCollider = _playerzy.GetComponent<BoxCollider2D> ();
		_transform = transform;
	}

	void Update ()
	{
		if (_activeRotator == null || !_activeRotator.IsRotating)
			_isRotating = false;
		if (!_isRotating) {
			_transform.position = new Vector3 (_playerxyTransform.position.x, _transform.position.y, -_playerzyTransform.position.x);
		} else {
			_playerxyTransform.position = new Vector3 (_transform.position.x, _transform.position.y, _playerxyTransform.position.z);
			_playerzyTransform.position = new Vector3 (-_transform.position.z, _transform.position.y, _playerzyTransform.position.z);
			
		}
		UpdateRaycastOrigins ();
		disabledDir [0] = 0;
		disabledDir [1] = 0;
		calculateGroundRays (groundedRayDir);
		if (_inputController.enableRunInput) {
			if (Input.GetKey (_inputController.leftBtn))
				runningDir = 1;
			if (Input.GetKey (_inputController.rightBtn))
				runningDir = -1;
			calculateHorizontalRay (runningRayDir * runningDir, PlayerInputController.playerStatus);
		}
	}

	//	private float YPosition {
	//		get { return (_playerxyTransform.position.y + _playerzyTransform.position.y)/2f; }
	//	}

	public void Rotate (float direction)
	{
		if (_isRotating) {
			return;
		}
		RaycastHit[] hitInfo = new RaycastHit[4];
		bool[] didHit = new bool[4];
		float maxDistance = Mathf.Infinity;
		Collider hitCollider = null;
		Vector3 pivot = SnappingMath.SnapToRoundedOffset (_transform.position, _pivotGridPrecision);
		didHit [0] = Physics.Raycast (pivot + Vector3.up * _raycastEpsilon + Vector3.right * _playerxyCollider.size.x / 2f, -Vector3.up, out hitInfo [0]);
		didHit [1] = Physics.Raycast (pivot + Vector3.up * _raycastEpsilon + Vector3.left * _playerxyCollider.size.x / 2f, -Vector3.up, out hitInfo [1]);
		didHit [2] = Physics.Raycast (pivot + Vector3.up * _raycastEpsilon + Vector3.forward * _playerzyCollider.size.x / 2f, -Vector3.up, out hitInfo [2]);
		didHit [3] = Physics.Raycast (pivot + Vector3.up * _raycastEpsilon + Vector3.back * _playerzyCollider.size.x / 2f, -Vector3.up, out hitInfo [3]);
		for (int i = 0; i < 4; i++) {
			float currDistance = hitInfo [i].distance;
			if (didHit [i] && currDistance < maxDistance) {
				maxDistance = currDistance;
				hitCollider = hitInfo [i].collider;
			}
		}
		if (hitCollider == null) {
			Debug.LogError ("HIT NOTHING!");
		} else {
			BlockInformation blockInfo = hitCollider.GetComponent<BlockInformation> ();
			if (hitCollider.transform.parent.name != "Planes" && blockInfo != null && blockInfo.CanRotate) {
				_activeRotator = blockInfo.MyRotatable;
				_activeRotator.CharacterDriveRotation (direction, _activeRotator.transform.position, _rotationDuration);
				_isRotating = true;
			}
		}
	}

	void calculateGroundRays (Vector3 rayDir)
	{
		//float directionX = Mathf.Sign (rayDir.x);
		float directionY = Mathf.Sign (rayDir.y);
		float rayLength = Mathf.Abs (rayDir.y) + skinWidth;
		RaycastHit hit;
		groundedRayIndex.Clear ();
		if (groundedObject)
			groundedObject.GetComponent<BlockInformation> ().beTouched = 2;
		for (int j = 0; j < groundRayCount; j++)
			for (int i = 0; i < groundRayCount; i++) {
				Vector3 rayOrigin = raycastOrigins.bottomLeft + Vector3.right * (groundRaySpacing * i) + Vector3.forward * (groundRaySpacing * j);
				if (Physics.Raycast (rayOrigin, -Vector3.up * directionY, rayLength)) {
					groundedRayIndex.Add (new Vector2 (j, i));
				}
				//find out the touched object
				if (i == (groundRayCount - groundRayCount % 2) / 2 && j == (groundRayCount - groundRayCount % 2) / 2) {
					if (Physics.Raycast (rayOrigin, -Vector3.up * directionY, out hit)) {
						if (hit.collider != null && hit.collider.gameObject.GetComponent<BlockInformation> ()) {
							groundedObject = hit.collider.gameObject;
							groundedObject.GetComponent<BlockInformation> ().beTouched = 10;
						}
					}
				}
				Debug.DrawRay (rayOrigin, -Vector3.up * directionY * rayLength, Color.red);
			}
		//detect if the character touches the edge
		if (!groundedRayIndex.Contains (new Vector2 (1, 0))) {
			disabledDir [0] = 1;	//0 right
		}
		if (!groundedRayIndex.Contains (new Vector2 (1, 2))) {
			disabledDir [0] = -1;//0 left	
		}
		if (!groundedRayIndex.Contains (new Vector2 (2, 1))) {
			disabledDir [1] = 1;//1 right
		}
		if (!groundedRayIndex.Contains (new Vector2 (0, 1))) {
			disabledDir [1] = -1;//1 left	
		}
	}

	void calculateHorizontalRay (Vector3 rayDir, int controlDir)
	{//do the same thing as vertical
		float directionX = Mathf.Sign (rayDir.x);
		float rayLength = Mathf.Abs (rayDir.x) + skinWidth;
		RaycastHit hit;
		//	runningToObject = null;
		for (int j = 0; j < groundRayCount; j++)
			for (int i = 1; i < runningRayCount; i++) {
				//find out what running ray we should use
				Vector3 rayOrigin = Vector3.zero;
				Vector3 lineDir = Vector3.zero;
				Vector3 plusDir = Vector3.zero;
				if (controlDir == 0 && directionX == 1) {
					rayOrigin = raycastOrigins.topLeft;
					lineDir = Vector3.right;
					plusDir = Vector3.forward;
				}
				if (controlDir == 0 && directionX == -1) {
					rayOrigin = raycastOrigins.bottomLeft;
					lineDir = Vector3.right;
					plusDir = Vector3.forward;
				}
				if (controlDir == 1 && directionX == 1) {
					rayOrigin = raycastOrigins.bottomLeft;
					lineDir = -Vector3.forward;
					plusDir = Vector3.right;
				}
				if (controlDir == 1 && directionX == -1) {
					rayOrigin = raycastOrigins.bottomRight;
					lineDir = -Vector3.forward;
					plusDir = Vector3.right;
				}
			
				rayOrigin += Vector3.up * (runningRaySpacing * i) + plusDir * (groundRaySpacing * j);
				if (Physics.Raycast (rayOrigin, lineDir * directionX, rayLength))
				if (Physics.Raycast (rayOrigin, lineDir * directionX, out hit))
				if (hit.collider != null) {
					runningToObject = hit.collider.gameObject;
					disabledDir [controlDir] = -(int)directionX;
				}
				Debug.DrawRay (rayOrigin, lineDir * directionX * rayLength, Color.magenta);
				/////this is for finding the objects that An has touched
				if (Physics.Raycast (rayOrigin, lineDir * directionX, out forHitObj, hitDetectDist)) {
					if (!touchedObjects.Contains (forHitObj.transform)) {
						touchedObjects.Add (forHitObj.transform);
					}
					Debug.DrawRay (rayOrigin, lineDir * directionX * forHitObj.distance, Color.yellow);
				}
			}
	}


}
