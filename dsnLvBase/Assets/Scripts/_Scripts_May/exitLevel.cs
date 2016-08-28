using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class exitLevel : MonoBehaviour
{
		[SerializeField]
		List<Light> lights = new List<Light> ();
		public GameObject exit3d;
		public List<trigger2D> triggers = new List<trigger2D> ();
		public bool nextLevel;
		[SerializeField]
		float min = .5f;
		[SerializeField]
		float max = 1;
	Component halo;
	 
	void Start(){
		if (exit3d!=null) 
		halo = exit3d.GetComponent ("Halo"); 
	}

		void FixedUpdate ()
		{
			for (int i = 0; i < triggers.Count; i++) {
						if (!triggers [i].triggered) {
								lights [i].range = Mathf.PingPong (Time.time * 0.5f, max - min) + min;
								lights [i].intensity = Mathf.Lerp (0, 8, Time.time);
						} else {
								lights [i].range = Mathf.Lerp (min, 10, Time.time);
								lights [i].intensity = Mathf.Lerp (8, 0, Time.time);
						}
				}
				if (triggers.Count == 2) 
				if (triggers [0].triggered && triggers [1].triggered) {
						nextLevel = true;
			if (exit3d!=null) 
						halo.GetType ().GetProperty ("enabled").SetValue (halo, false, null);
				} else {
						nextLevel = false;
			if (exit3d!=null) 
						halo.GetType ().GetProperty ("enabled").SetValue (halo, true, null);
				}
				if (triggers.Count == 1)
				if (triggers [0].triggered)
						nextLevel = true;
				else
						nextLevel = false;
				
		}
}
