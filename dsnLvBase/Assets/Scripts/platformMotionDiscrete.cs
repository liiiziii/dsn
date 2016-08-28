using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class platformMotionDiscrete : MonoBehaviour
{
	[SerializeField]
	Transform _anotherPoint;
	[SerializeField]
	float speed = 0.1f;
	Vector3 velocity;
	Vector3 desiredVel;
	float lastSqrMag = 0;
	Vector3 nextPos;
	Vector3 previousPos;
	Vector3 originalPos;
	public int dir = -1;
	int lastDir = 1;
	public bool shouldMove = false;
	BlockInformation _blockInfo;
		[SerializeField]
		public GameObject[] players;
		[HideInInspector]
		public BlockInformation controllerBlockInfo;
		int _stoppedDir = 0;
		LineRenderer path;



		void Start ()
		{
				
		_blockInfo = GetComponent<BlockInformation> ();
		originalPos = transform.position;
		nextPos = transform.position;
		previousPos = transform.position;
		velocity = (nextPos - previousPos).normalized * speed;
		lastSqrMag = Mathf.Infinity;	
		path = GetComponent<LineRenderer> ();
		path.SetPosition (0, originalPos);
				path.SetPosition (1, _anotherPoint.position);
				path.SetWidth (0.15F, 0.15F);
		path.enabled = true;
		}
	
	void FixedUpdate ()
	{
		//line color
		if (dir == 1) 
						path.material.color = Color.white;
				else
						path.material.color = Color.black / 2;
		//For Direction
		if (dir != lastDir) {
			shouldMove = true;
			if (dir > 0) {
				nextPos = _anotherPoint.position;
				previousPos = originalPos;
			} else {
				nextPos = originalPos;
				previousPos = _anotherPoint.position;
			}
			velocity = (nextPos - previousPos).normalized * speed;
			lastSqrMag = Mathf.Infinity;
			lastDir = dir;
		}
		//For velocity
		float sqrMag = (nextPos - transform.position).sqrMagnitude;
		if (sqrMag > lastSqrMag) {
			Vector3 endVel = nextPos - transform.position;
			movePlayers(endVel);
			transform.position = nextPos;
			velocity = Vector3.zero; 
			shouldMove = false;
		}
		lastSqrMag = sqrMag;
		//detect obstacle
		Vector3 obstacleRay = Vector3.Scale (velocity.normalized, transform.lossyScale);
		if (velocity != Vector3.zero && Physics.Raycast (transform.position, velocity.normalized, obstacleRay.magnitude/2+0.2F))
						return;
		//move the character with 2D collider
		if (!shouldMove)
						return;
		movePlayers (velocity);
		transform.Translate (velocity);
	}

	void movePlayers(Vector3 vel){
		if (_blockInfo.beTouched == 0) {
			players [0].transform.Translate (vel.x, vel.y, 0);
		} else if (_blockInfo.beTouched == 1) {
			players [1].transform.Translate (vel.z, vel.y, 0);
		} else if (_blockInfo.beTouched == 10) {
			players [0].transform.Translate (vel.x, vel.y, 0);
			players [1].transform.Translate (vel.z, vel.y, 0);
		}
	}

}
