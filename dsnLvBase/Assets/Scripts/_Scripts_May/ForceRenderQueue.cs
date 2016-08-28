using UnityEngine;
using System.Collections;

public class ForceRenderQueue : MonoBehaviour {
	Material _material;
	[SerializeField]
	int _renderOrder = 0;
	void Awake () {
		_material = GetComponent<MeshRenderer>().material;
		_material.renderQueue = _renderOrder;
	}
	
	void OnValidate () {
		if(_material != null) {
			_material.renderQueue = _renderOrder;
		}
	}
}
