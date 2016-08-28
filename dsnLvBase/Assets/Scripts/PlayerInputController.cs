using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

public class PlayerInputController : MonoBehaviour {
	public static int playerStatus;
	public static bool restart;
	[SerializeField] 
	SPCharInputCtr SupportCharScript;
	// movement config
	public float gravity = -25f;
	public float runSpeed = 3.8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 1.65f;
	public float normalizedHorizontalSpeed = 0;

	[System.Serializable]
	struct PlayerInfo2D {
		public Transform spriteTransform;
		public Animator animator;
		public CharacterController2D controller;
		// [HideInInspector]
		// public RaycastHit2D lastControllerColliderHit;
		[HideInInspector]
		public Vector3 velocity;
	}
	
	[SerializeField]
	PlayerInfo2D _playerXY;
	[SerializeField]
	PlayerInfo2D _playerZY;
	[SerializeField]
	private Character3D _playerXYZ;
	private PlayerInfo2D[] _all2DPlayers;
	[SerializeField]
	int _characterIndexUnderControl;

	public KeyCode upBtn;
	public KeyCode downBtn;
	public KeyCode leftBtn;
	public KeyCode rightBtn;
	public KeyCode enterBtn;
	[HideInInspector]
	public bool enableRunInput = true;

	void Awake()
	{
		restart=false;
//		platformVelXY=new Vector3(0,0,0);
//		platformVelZY=new Vector3(0,0,0);

		_all2DPlayers = new PlayerInfo2D[2];
		_all2DPlayers[0] = _playerXY;
		_all2DPlayers[1] = _playerZY;


		// listen to some events for illustration purposes
		//_controller.onControllerCollidedEvent += onControllerCollider;
		//_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		//_controller.onTriggerExitEvent += onTriggerExitEvent;
	}

// TODO(Julian): Re-enable events and add a ref to the related controller
	// #region Event Listeners

	// void onControllerCollider( RaycastHit2D hit )
	// {
	// 	// bail out on plain old ground hits cause they arent very interesting
	// 	if( hit.normal.y == 1f )
	// 		return;

	// 	// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
	// 	//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	// }


	// void onTriggerEnterEvent( Collider2D col )
	// {
	// 	Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	// }


	// void onTriggerExitEvent( Collider2D col )
	// {
	// 	Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	// }

	// #endregion
	
	void Update () {
		if(restart)
			Application.LoadLevel(Application.loadedLevel);

		if(InputManager.ActiveDevice.Action3.WasPressed || Input.GetKeyDown(KeyCode.R)){
			restart=true;
		}


	

	
			if (InputManager.ActiveDevice.LeftBumper.WasPressed || InputManager.ActiveDevice.LeftTrigger.WasPressed || Input.GetKey(KeyCode.Z)) {
				_playerXYZ.Rotate(-1);
			}
			if (InputManager.ActiveDevice.RightBumper.WasPressed || InputManager.ActiveDevice.RightTrigger.WasPressed || Input.GetKey(KeyCode.X)) {
				_playerXYZ.Rotate(1);
			}

		if (enableRunInput) {
			if (InputManager.ActiveDevice.Action2.WasPressed  || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
				SupportCharScript.setSpriteDirection (SupportCharScript.all2DPlayers [SupportCharScript.characterIndexUnderControl], 0);
				_characterIndexUnderControl = (_characterIndexUnderControl + 1) % (_all2DPlayers.Length);
				SupportCharScript.characterIndexUnderControl = _characterIndexUnderControl;
				SupportCharScript.setSpriteDirection (SupportCharScript.all2DPlayers [SupportCharScript.characterIndexUnderControl], 1);
			}
						for (int i = 0; i < _all2DPlayers.Length; i++) {
								bool isInputAllowed = (i == _characterIndexUnderControl);
								if (isInputAllowed)
										playerStatus = i;
								if (Character3D.IsRotating) {
										isInputAllowed = false;
								}
								SimulatePlayer (i, _all2DPlayers [i], isInputAllowed);
						}
				}
		
	}



	void SimulatePlayer(int index, PlayerInfo2D player, bool isInputAllowed)
	{
		CharacterController2D _controller = player.controller;
		Transform spriteTransform = player.spriteTransform;

		// grab our current _velocity to use as a base for all calculations
		player.velocity = _controller.velocity;

		if( _controller.isGrounded )
			player.velocity.y = 0;
		if( isInputAllowed && (InputManager.ActiveDevice.LeftStickX.Value < 0 || Input.GetKey(KeyCode.LeftArrow)))
		{
			if(_playerXYZ.disabledDir[index] != -1){
				normalizedHorizontalSpeed = 1;
				if( spriteTransform.localScale.x < 0f )
					spriteTransform.localScale = new Vector3( -spriteTransform.localScale.x, spriteTransform.localScale.y, spriteTransform.localScale.z );

				if( _controller.isGrounded ){
					player.animator.Play( Animator.StringToHash( "Run" ) );
				}
			}
		}
		else if( isInputAllowed && (InputManager.ActiveDevice.LeftStickX.Value > 0 || Input.GetKey(KeyCode.RightArrow)))
		{
			if(_playerXYZ.disabledDir[index] != 1){
			normalizedHorizontalSpeed = -1;
			if( spriteTransform.localScale.x > 0f )
				spriteTransform.localScale = new Vector3( -spriteTransform.localScale.x, spriteTransform.localScale.y, spriteTransform.localScale.z );

			if( _controller.isGrounded ){
				player.animator.Play( Animator.StringToHash( "Run" ) );
			}
			}
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded ){
				if (_characterIndexUnderControl == index) {
					if(Character3D.IsRotating)
						player.animator.Play( Animator.StringToHash( "rotating_active" ) );
					else
					player.animator.Play( Animator.StringToHash( "Idle_Active" ) );
				} else {
					if(Character3D.IsRotating)
						player.animator.Play( Animator.StringToHash( "rotating" ) );
					else
					player.animator.Play( Animator.StringToHash( "Idle" ) );
				}

			}
		}


		// we can only jump whilst grounded
//		if( isInputAllowed && _controller.isGrounded && (InputManager.ActiveDevice.Action1.WasPressed || Input.GetKeyDown(KeyCode.UpArrow)) )
//		{
//			player.velocity.x=0;
//			player.velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
//			player.animator.Play( Animator.StringToHash( "Jump" ) );
//		}


		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		player.velocity.x = Mathf.Lerp( player.velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		player.velocity.y += gravity * Time.deltaTime;
			_controller.move( player.velocity * Time.deltaTime );
	}
}
