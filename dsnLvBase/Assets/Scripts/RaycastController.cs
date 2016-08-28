using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class RaycastController : MonoBehaviour {

	public LayerMask collisionMask;
	
	public const float skinWidth = .015f;
	public int runningRayCount = 3;
	public int groundRayCount = 3;

	[HideInInspector]
	public float runningRaySpacing;
	[HideInInspector]
	public float groundRaySpacing;

	[HideInInspector]
	public BoxCollider collider;
	public RaycastOrigins raycastOrigins;

	public virtual void Start() {
		collider = GetComponent<BoxCollider> ();
		CalculateRaySpacing ();//only need to find out spacing at the beginning
	}

	public void UpdateRaycastOrigins() {//find out 4 original points
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		
		raycastOrigins.bottomLeft = new Vector3 (bounds.min.x, bounds.min.y,bounds.min.z);
		raycastOrigins.bottomRight = new Vector3 (bounds.min.x, bounds.min.y,bounds.max.z);
		raycastOrigins.topLeft = new Vector3 (bounds.max.x, bounds.min.y,bounds.min.z);
		raycastOrigins.topRight = new Vector3 (bounds.max.x, bounds.min.y,bounds.max.z);
	}
	
	public void CalculateRaySpacing() {//find out the space betwwen ray lines  
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		
		runningRayCount = Mathf.Clamp (runningRayCount, 2, int.MaxValue);
		groundRayCount = Mathf.Clamp (groundRayCount, 2, int.MaxValue);
		
		runningRaySpacing = bounds.size.y / (runningRayCount - 1);
		groundRaySpacing = bounds.size.x / (groundRayCount - 1);
	}

	public struct RaycastOrigins { 
		public Vector3 topLeft, topRight;
		public Vector3 bottomLeft, bottomRight;
	}
}
