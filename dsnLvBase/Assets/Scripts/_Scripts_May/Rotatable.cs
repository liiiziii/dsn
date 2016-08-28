using UnityEngine;
using System.Collections;

public class Rotatable : ICanBeDriven {
	[HideInInspector]
	public enum Axis {
		X = 0, Y = 1, Z = 2
	}

	private Transform _rotator;
	private Transform _oldParent;
	private float _fractionRotated = 0f;
	private bool _isRotating = false;
	public bool IsRotating {
		get { return _isRotating; }
	}
	private float _nextAngle=0;
	private Quaternion _nextRot;
	private Quaternion _previousRot;
	private float _rotationDuration = 1.2f;
//	[SerializeField]
//	Axis _rotationAxis = Axis.Y;
	[HideInInspector]
	public int _currentRotationAxis = 1;
	[SerializeField]
	public bool _canBeDrivenByCharacter = true;
	[SerializeField]
	ICanBeDriven[] _drives;
	public int[] _drivesRotationAxis;
	public bool rotOnce = false;
	[SerializeField]
	platformMotionDiscrete[] _controlledPF;
	[HideInInspector]
	public bool shouldTurnBack = false;
	[SerializeField]
	bool detectTurnBack = true;
	
	void Awake () {
		_rotator = new GameObject(gameObject.name + "_Rotator").transform;
		_rotator.parent = transform.parent;
		_rotator.position = transform.position;
	}

	void StartRotation () {
		_oldParent = transform.parent;
		transform.SetParent(_rotator);
	}

	void FinishRotation () {
		transform.SetParent(_oldParent);
	}

	void Update () {
		if (_isRotating) {
			_fractionRotated += Time.deltaTime/_rotationDuration;
			if (_fractionRotated >= 1f) {
				_isRotating = false;
				_fractionRotated = 1f;
				rotOnce = true;
				shouldTurnBack =  false;
				FinishRotation();
			}
			if(detectTurnBack){
				if(!shouldTurnBack)
					_rotator.rotation = Quaternion.Lerp(_rotator.rotation, _nextRot, _fractionRotated);
				else
					_rotator.rotation = Quaternion.Lerp(_rotator.rotation, _previousRot, _fractionRotated);
			}else{
				_rotator.rotation = Quaternion.Lerp(_rotator.rotation, _nextRot, _fractionRotated);
			}
		}else
			_rotator.rotation = Quaternion.Euler(0,0,0);

	}

	public void CharacterDriveRotation (float direction, Vector3 origin, float duration) {
		if (!_canBeDrivenByCharacter) {
			return;
		} else {
			_currentRotationAxis = 1;
			PerformAnimation (direction, origin, duration);
		}
	}

	public override void drivenAxis(int a){
		_currentRotationAxis = a;
	}

	public override void PerformAnimation (float direction, Vector3 origin, float duration) {
		if (_isRotating) {
						return;
				} else {
						_previousRot = _rotator.rotation;
						_rotationDuration = duration;
						_rotator.position = origin;
						_nextAngle = direction * 90;
						Vector3 rot = _rotator.eulerAngles;
						rot [_currentRotationAxis] = _nextAngle;
						foreach(platformMotionDiscrete i in _controlledPF){
							i.dir = -i.dir;
						}
						_nextRot = Quaternion.Euler (rot);
						_fractionRotated = 0f;
						_isRotating = true;
						StartRotation ();
						for (int i = 0; i < _drives.Length; i++) {
								_drives [i].drivenAxis (_drivesRotationAxis [i]);
								_drives [i].PerformAnimation (direction, _drives [i].transform.position, duration);
						}
				}
	}
}