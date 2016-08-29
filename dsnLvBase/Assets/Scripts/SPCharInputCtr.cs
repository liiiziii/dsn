using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SPCharInputCtr : MonoBehaviour
{
	public int indexInBlockInfo = 0;
	[HideInInspector]
	public int thisCharStatus;
	bool disableControl = false;
	public List<BlockInformation> pXYhitObj = new List<BlockInformation> ();
	public List<BlockInformation> pZYhitObj = new List<BlockInformation> ();
	// movement config
	public float gravity = -25f;
	public float runSpeed = 3.8f;
	public float groundDamping = 20f;
	// how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float normalizedHorizontalSpeed = 0;
	[SerializeField]
	float
		_char3dVisibilityThreshold = 1f;

	[System.Serializable]
	public struct PlayerInfo2D
	{
		public Transform spriteTransform;
		public Transform spriteModel;
		//				public Animator animator;
		public SPCharCtr2d controller;
		[HideInInspector]
		public Vector3 velocity;
	}


	[SerializeField]
	PlayerInfo2D
		_charXY;
	[SerializeField]
	PlayerInfo2D
		_charZY;
	[SerializeField]
	Transform _char3D;
	[HideInInspector]
	public PlayerInfo2D[] all2DPlayers;
	public int characterIndexUnderControl;

	BlockInformation touch3dObj;
	bool iHitObject = false;
	[HideInInspector]
	public int[] directions;

	void Awake ()
	{
		all2DPlayers = new PlayerInfo2D[2];
		all2DPlayers [0] = _charXY;
		all2DPlayers [1] = _charZY;
		directions = new int[2];
		directions [0] = 0;
		directions [1] = 0;
	}

	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void touchOfObjects ()
	{
		touch3dObj = null;
		iHitObject = false;
		Vector2 touchBoth = new Vector2 (1, 1);
		foreach (BlockInformation go in pXYhitObj) {
			if(go != null && go.SPCharBeTouched.Count > 0)
			if (go.SPCharBeTouched [indexInBlockInfo] == touchBoth) {
				touch3dObj = go;
				iHitObject = true;
			}
		}
		foreach (BlockInformation go in pZYhitObj) {
			if(go != null)
			if (go.SPCharBeTouched.Count > 0 && go.SPCharBeTouched [indexInBlockInfo] == touchBoth) {
				touch3dObj = go;
				iHitObject = true;
			}
		}
	}

	void Update ()
	{
		touchOfObjects ();
		//charControllInfo ();
		for (int i = 0; i < all2DPlayers.Length; i++) {
//			bool isInputAllowed = (i == characterIndexUnderControl);
//			if (isInputAllowed)
//				thisCharStatus = i;
			SimulatePlayer (i, all2DPlayers [i], true,directions[i]);
		}
		//updateChar3D ();
	}

//	void charControllInfo ()
//	{
//		if (Input.GetKeyDown (KeyCode.Alpha2)) {
//			disableControl = false;
//		}
//		if (Input.GetKeyDown (KeyCode.Alpha1)) {
//			disableControl = true;
//		}
////		if (!disableControl) {
////			//change status
////			if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) { 
////				setSpriteDirection (all2DPlayers [characterIndexUnderControl], 0);
////				characterIndexUnderControl = (characterIndexUnderControl + 1) % (all2DPlayers.Length);
////				setSpriteDirection (all2DPlayers [characterIndexUnderControl], 1);
////			}
////		}
//	}

	void updateChar3D ()
	{
		if (Char3dDoesExist) {
			_char3D.gameObject.SetActive (true);
			_char3D.position = new Vector3 (_charXY.spriteTransform.position.x, (_charXY.spriteTransform.position.y + _charZY.spriteTransform.position.y) / 2f, -_charZY.spriteTransform.position.x);
			float rangle = all2DPlayers [characterIndexUnderControl].spriteModel.eulerAngles.y + 90 * characterIndexUnderControl;
			_char3D.rotation = Quaternion.Euler (0, rangle, 0);
		} else {
			_char3D.gameObject.SetActive (false);
		}
	}

				
	void SimulatePlayer (int index, PlayerInfo2D player, bool isInputAllowed, int dir)
	{
		SPCharCtr2d _controller = player.controller;

		// grab our current _velocity to use as a base for all calculations
		player.velocity = _controller.velocity;

		if (_controller.isGrounded)
			player.velocity.y = 0;

		if (isInputAllowed && dir == 1) {
			if (!disableControl) {
				normalizedHorizontalSpeed = 1;
				setSpriteDirection (player, -1);
//								if (_controller.isGrounded) {
//										player.animator.Play (Animator.StringToHash ("Run"));
//								}
			}
		} else if (isInputAllowed && dir == -1) {
			if (!disableControl) {
				normalizedHorizontalSpeed = -1;
				setSpriteDirection (player, 1);
//								if (_controller.isGrounded) {
//										player.animator.Play (Animator.StringToHash ("Run"));
//								}
			}

		} else {
			normalizedHorizontalSpeed = 0;
//						if (_controller.isGrounded) {
//								if (characterIndexUnderControl == index) {
//												player.animator.Play (Animator.StringToHash ("Idle_Active"));
//								} else {
//												player.animator.Play (Animator.StringToHash ("Idle"));
//								}
//
//						}
		}
		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		player.velocity.x = Mathf.Lerp (player.velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
		// apply gravity before moving
		player.velocity.y += gravity * Time.deltaTime;
		_controller.move (player.velocity * Time.deltaTime);
	}

	public void setSpriteDirection (PlayerInfo2D player, int direction)
	{
		player.spriteModel.rotation = Quaternion.Euler (0, 90 + 90 * direction, 0);
	}


	public bool Char3dDoesExist {
		get { return (Mathf.Abs (_charXY.spriteTransform.position.y - _charZY.spriteTransform.position.y) < _char3dVisibilityThreshold && iHitObject); }
	}
}

