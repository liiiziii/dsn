using UnityEngine;
using System.Collections;

public abstract class ICanBeDriven : MonoBehaviour {
	public abstract void drivenAxis(int a);
	public abstract void PerformAnimation (float direction, Vector3 origin, float duration);
}