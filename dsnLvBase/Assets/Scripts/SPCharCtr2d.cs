#define DEBUG_CC2D_RAYS
using UnityEngine;
using System;
using System.Collections.Generic;


[RequireComponent (typeof(BoxCollider2D), typeof(Rigidbody2D))]

public class SPCharCtr2d : MonoBehaviour
{
	#region internal types

	private struct CharacterRaycastOrigins
	{
		public Vector3 topLeft;
		public Vector3 bottomRight;
		public Vector3 bottomLeft;
	}

	public class CharacterCollisionState2D
	{
		public bool right;
		public bool left;
		public bool above;
		public bool below;
		public bool becameGroundedThisFrame;
		public bool wasGroundedLastFrame;


		public bool hasCollision ()
		{
			return below || right || left || above;
		}


		public void reset ()
		{
			right = left = above = below = becameGroundedThisFrame = false;
		}


		public override string ToString ()
		{
			return string.Format ("[CharacterCollisionState2D] r: {0}, l: {1}, a: {2}, b: {3}, wasGroundedLastFrame: {6}, becameGroundedThisFrame: {7}",
				right, left, above, below, wasGroundedLastFrame, becameGroundedThisFrame);
		}
	}

	#endregion


	#region events, properties and fields

	public event Action<RaycastHit2D> onControllerCollidedEvent;

	/// <summary>
	/// toggles if the RigidBody2D methods should be used for movement or if Transform.Translate will be used. All the usual Unity rules for physics based movement apply when true
	/// such as getting your input in Update and only calling move in FixedUpdate amonst others.
	/// </summary>
	public bool usePhysicsForMovement = false;
	Collider2D hitCollider = null;

	[SerializeField]
	[Range (0.001f, 0.3f)]
	private float _skinWidth = 0.02f;

	/// <summary>
	/// defines how far in from the edges of the collider rays are cast from. If cast with a 0 extent it will often result in ray hits that are
	/// not desired (for example a foot collider casting horizontally from directly on the surface can result in a hit)
	/// </summary>
	public float skinWidth {
		get { return _skinWidth; }
		set {
			_skinWidth = value;
			recalculateDistanceBetweenRays ();
		}
	}


	/// <summary>
	/// mask with all layers that the player should interact with
	/// </summary>
	public LayerMask platformMask = 0;

	/// <summary>
	/// mask with all layers that trigger events should fire when intersected
	/// </summary>
	public LayerMask triggerMask = 0;

	/// <summary>
	/// mask with all layers that should act as one-way platforms. Note that one-way platforms should always be EdgeCollider2Ds. This is private because it does not support being
	/// updated anytime outside of the inspector for now.
	/// </summary>
	[SerializeField]
	private LayerMask closestObjectMask = 0;


	/// <summary>
	/// the threshold in the change in vertical movement between frames that constitutes jumping
	/// </summary>
	/// <value>The jumping threshold.</value>
	public float jumpingThreshold = 0.07f;


	[Range (2, 20)]
	public int totalHorizontalRays = 8;
	[Range (2, 20)]
	int totalVerticalRays = 4;

	[Range (0.8f, 0.999f)]
	public float triggerHelperBoxColliderScale = 0.95f;


	[HideInInspector][NonSerialized]
	public new Transform transform;
	[HideInInspector][NonSerialized]
	public BoxCollider2D boxCollider;
	[HideInInspector][NonSerialized]
	public Rigidbody2D rigidBody2D;

	[HideInInspector][NonSerialized]
	public CharacterCollisionState2D collisionState = new CharacterCollisionState2D ();
	[HideInInspector][NonSerialized]
	public Vector3 velocity;

	public bool isGrounded { get { return collisionState.below; } }

	private const float kSkinWidthFloatFudgeFactor = 0.001f;

	#endregion


	/// <summary>
	/// holder for our raycast origin corners (TR, TL, BR, BL)
	/// </summary>
	private CharacterRaycastOrigins _raycastOrigins;

	/// <summary>
	/// stores our raycast hit during movement
	/// </summary>
	private RaycastHit2D _raycastHit;


	/// <summary>
	/// stores any raycast hits that occur this frame. we have to store them in case we get a hit moving
	/// horizontally and vertically so that we can send the events after all collision state is set
	/// </summary>
	private List<RaycastHit2D> _raycastHitsThisFrame = new List<RaycastHit2D> (2);

	// horizontal/vertical movement data
	private float _verticalDistanceBetweenRays;
	private float _horizontalDistanceBetweenRays;


	private RaycastHit2D[] _raycastHitAll;
	public SPCharInputCtr _CharInputCtr;
	[SerializeField]
	Transform _Char3D;


	#region Monobehaviour

	void Awake ()
	{

		// add our one-way platforms to our normal platform mask so that we can land on them from above
		//platformMask |= oneWayPlatformMask;

		// cache some components
		transform = GetComponent<Transform> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		rigidBody2D = GetComponent<Rigidbody2D> ();

		// here, we trigger our properties that have setters with bodies
		skinWidth = _skinWidth;

		// we want to set our CC2D to ignore all collision layers except what is in our triggerMask
		for (var i = 0; i < 32; i++) {
			// see if our triggerMask contains this layer and if not ignore it
			if ((triggerMask.value & 1 << i) == 0)
				Physics2D.IgnoreLayerCollision (gameObject.layer, i);
		}
	}

	#endregion

	[System.Diagnostics.Conditional ("DEBUG_CC2D_RAYS")]
	private void DrawRay (Vector3 start, Vector3 dir, Color color)
	{
		Debug.DrawRay (start, dir, color);
	}


	#region Public

	public RaycastHit2D closestObjR {
		get { return Physics2D.Raycast (boxCollider.bounds.center - new Vector3 (boxCollider.bounds.size.x / 2 + skinWidth, 0, 0), Vector3.left, Mathf.Infinity, closestObjectMask); }
	}

	public RaycastHit2D closestObjL {
		get { return Physics2D.Raycast (boxCollider.bounds.center + new Vector3 (boxCollider.bounds.size.x / 2 + skinWidth, 0, 0), -Vector3.left, Mathf.Infinity, closestObjectMask); }
	}

	/// <summary>
	/// attempts to move the character to position + deltaMovement. Any colliders in the way will cause the movement to
	/// stop when run into.
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	public void move (Vector3 deltaMovement)
	{
		// save off our current grounded state which we will use for wasGroundedLastFrame and becameGroundedThisFrame
		collisionState.wasGroundedLastFrame = collisionState.below;

		// clear our state
		collisionState.reset ();
		_raycastHitsThisFrame.Clear ();

		var desiredPosition = transform.position + deltaMovement;
		primeRaycastOrigins (desiredPosition, deltaMovement);


		// now we check movement in the horizontal dir
//		if (deltaMovement.x != 0)
//			moveHorizontally (ref deltaMovement);

		// next, check movement in the vertical dir
		if (deltaMovement.y != 0)
			moveVertically (ref deltaMovement);


		// move then update our state
		if (usePhysicsForMovement) {
			GetComponent<Rigidbody2D> ().MovePosition (transform.position + deltaMovement);
			velocity = GetComponent<Rigidbody2D> ().velocity;
		} else {
			transform.Translate (deltaMovement, Space.World);

			// only calculate velocity if we have a non-zero deltaTime
			if (Time.deltaTime > 0)
				velocity = deltaMovement / Time.deltaTime;
		}

		// set our becameGrounded state based on the previous and current collision state
		if (!collisionState.wasGroundedLastFrame && collisionState.below)
			collisionState.becameGroundedThisFrame = true;

		// send off the collision events if we have a listener
		if (onControllerCollidedEvent != null) {
			for (var i = 0; i < _raycastHitsThisFrame.Count; i++) {
				onControllerCollidedEvent (_raycastHitsThisFrame [i]);

			}
		}
	}


	/// <summary>
	/// moves directly down until grounded
	/// </summary>
	public void warpToGrounded ()
	{
		do {
			move (new Vector3 (0, -1f, 0));
		} while(!isGrounded);
	}


	/// <summary>
	/// this should be called anytime you have to modify the BoxCollider2D at runtime. It will recalculate the distance between the rays used for collision detection.
	/// It is also used in the skinWidth setter in case it is changed at runtime.
	/// </summary>
	public void recalculateDistanceBetweenRays ()
	{
		// figure out the distance between our rays in both directions
		// horizontal
		var colliderUseableHeight = boxCollider.size.y * Mathf.Abs (transform.localScale.y) - (2f * _skinWidth);
		_verticalDistanceBetweenRays = colliderUseableHeight / (totalHorizontalRays - 1);

		// vertical
		var colliderUseableWidth = boxCollider.size.x * Mathf.Abs (transform.localScale.x) - (2f * _skinWidth);
		_horizontalDistanceBetweenRays = colliderUseableWidth / (totalVerticalRays - 1);
	}

	#endregion


	#region Private Movement Methods

	/// <summary>
	/// resets the raycastOrigins to the current extents of the box collider inset by the skinWidth. It is inset
	/// to avoid casting a ray from a position directly touching another collider which results in wonky normal data.
	/// </summary>
	/// <param name="futurePosition">Future position.</param>
	/// <param name="deltaMovement">Delta movement.</param>
	private void primeRaycastOrigins (Vector3 futurePosition, Vector3 deltaMovement)
	{
		// our raycasts need to be fired from the bounds inset by the skinWidth
		var modifiedBounds = boxCollider.bounds;
		modifiedBounds.Expand (-2f * _skinWidth);

		_raycastOrigins.topLeft = new Vector2 (modifiedBounds.min.x, modifiedBounds.max.y);
		_raycastOrigins.bottomRight = new Vector2 (modifiedBounds.max.x, modifiedBounds.min.y);
		_raycastOrigins.bottomLeft = modifiedBounds.min;
	}


	/// <summary>
	/// we have to use a bit of trickery in this one. The rays must be cast from a small distance inside of our
	/// collider (skinWidth) to avoid zero distance rays which will get the wrong normal. Because of this small offset
	/// we have to increase the ray distance skinWidth then remember to remove skinWidth from deltaMovement before
	/// actually moving the player
	/// </summary>
	private void moveHorizontally (ref Vector3 deltaMovement)
	{
		var isGoingRight = deltaMovement.x > 0;
		var rayDistance = Mathf.Abs (deltaMovement.x) + _skinWidth;
		var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
		var initialRayOrigin = isGoingRight ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;

		for (var i = 0; i < totalHorizontalRays; i++) {
			var ray = new Vector2 (initialRayOrigin.x, initialRayOrigin.y + i * _verticalDistanceBetweenRays);

			DrawRay (ray, rayDirection * rayDistance, Color.red);

			// if we are grounded we will include oneWayPlatforms only on the first ray (the bottom one). this will allow us to
			// walk up sloped oneWayPlatforms
			_raycastHit = Physics2D.Raycast (ray, rayDirection, rayDistance, platformMask);
			if (_raycastHit) {	
				// set our new deltaMovement and recalculate the rayDistance taking it into account
				deltaMovement.x = _raycastHit.point.x - ray.x;
				rayDistance = Mathf.Abs (deltaMovement.x);

				// remember to remove the skinWidth from our deltaMovement
				if (isGoingRight) {
					deltaMovement.x -= _skinWidth;
					collisionState.right = true;
				} else {
					deltaMovement.x += _skinWidth;
					collisionState.left = true;
				}

				_raycastHitsThisFrame.Add (_raycastHit);

				// we add a small fudge factor for the float operations here. if our rayDistance is smaller
				// than the width + fudge bail out because we have a direct impact
				if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor)
					break;
			}
		}
	}


	private void moveVertically (ref Vector3 deltaMovement)
	{
		var isGoingUp = deltaMovement.y > 0;
		var rayDistance = Mathf.Abs (deltaMovement.y) + _skinWidth;
		var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
		var initialRayOrigin = isGoingUp ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;

		// apply our horizontal deltaMovement here so that we do our raycast from the actual position we would be in if we had moved
		initialRayOrigin.x += deltaMovement.x;

		// if we are moving up, we should ignore the layers in oneWayPlatformMask
		var mask = platformMask;

		for (var i = 0; i < totalVerticalRays; i++) {
			var ray = new Vector2 (initialRayOrigin.x + i * _horizontalDistanceBetweenRays, initialRayOrigin.y);

			DrawRay (ray, rayDirection * rayDistance, Color.red);
			hitCollider = null;
			_raycastHit = Physics2D.Raycast (ray, rayDirection, rayDistance, mask);
			if (_raycastHit) {
				// set our new deltaMovement and recalculate the rayDistance taking it into account
				deltaMovement.y = _raycastHit.point.y - ray.y;
				rayDistance = Mathf.Abs (deltaMovement.y);
				//hit collider
				hitCollider = _raycastHit.collider;
				// remember to remove the skinWidth from our deltaMovement
				if (isGoingUp) {
					deltaMovement.y -= _skinWidth;
					collisionState.above = true;
				} else {
					deltaMovement.y += _skinWidth;
					collisionState.below = true;
				}

				_raycastHitsThisFrame.Add (_raycastHit);

				// we add a small fudge factor for the float operations here. if our rayDistance is smaller
				// than the width + fudge bail out because we have a direct impact
				if (rayDistance < _skinWidth + kSkinWidthFloatFudgeFactor)
					return;
			}
		}
	}


	#endregion


}