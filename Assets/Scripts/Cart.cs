using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Cart : MonoBehaviour
{
	public Vector3 ResetPosition { get; set; }
	public Quaternion ResetRotation { get; set; }
	private Vector2 _direction;
	[FormerlySerializedAs("_speed")] [SerializeField]
	private float speed = 80.0f;

	[SerializeField] private int energySupply = 2000;
	[SerializeField] private int energyRechargeDelay = 100;
	[FormerlySerializedAs("energyRecharge")] [SerializeField] private int energyRechargeRate = 100;
	[SerializeField] private int boostEnergyConsumption = 1;
	private int _currentEnergy;


	[SerializeField] private float boostForce = 10;
	private bool _isBoosting = false;
	private bool _isRecharging = true;

	private float BackwardSpeed => speed * 0.7f;

	[FormerlySerializedAs("MAX_STEERING_ANGLE")] [SerializeField]
	private float maxSteeringAngle = 45;

	[SerializeField] private List<AxleInfo> axles;

	private Rigidbody _rigidbody;
	
	private void Start()
	{
		ResetPosition = transform.position;
		ResetRotation = transform.rotation;
		_rigidbody = GetComponent<Rigidbody>();
		_currentEnergy = energySupply;
	}

	public void OnMove(InputValue inputValue)
	{
		_direction = inputValue.Get<Vector2>();
	}
	
	public void OnReset()
	{
		transform.position = ResetPosition;
		transform.rotation = ResetRotation;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
	}
	
	private void FixedUpdate()
	{
		//Debug.Log(_currentEnergy);
		var acceleration = _direction.y;
		if (acceleration > 0) {
			acceleration = speed;
		} else if (acceleration < 0) {
			acceleration = -BackwardSpeed;
		}

		if (_isBoosting)
		{
				_currentEnergy -= boostEnergyConsumption;
				_rigidbody.AddForce(transform.TransformDirection(Vector3.forward * boostForce));
			if (_currentEnergy <= 0) {
				_isBoosting = false;
				_currentEnergy = 0;
				StartRecharge();
			}
		}

		if (_isRecharging && _currentEnergy < energySupply) 
		{
			_currentEnergy += energyRechargeRate;
		}

		var steering = _direction.x * maxSteeringAngle;
		foreach (var axle in axles)
		{
			if (axle.IsMotor)
			{
				axle.LeftWheel.motorTorque = acceleration;
				axle.RightWheel.motorTorque = acceleration;
			}

			if (axle.IsSteering)
			{
				axle.LeftWheel.steerAngle = steering;
				axle.RightWheel.steerAngle = steering;
			}

			if (axle.IsReverseSteering)
			{
				axle.LeftWheel.steerAngle = -steering;
				axle.RightWheel.steerAngle = -steering;
			}
		}
	}

	public void OnBoost(InputValue value)
	{
		_isBoosting = value.isPressed;
		if (_isBoosting)
		{
			_isRecharging = false;
		}
		else
		{
			StartRecharge();
		}
	}

	private void StartRecharge()
	{
		//Debug.Log("start recharge delay");
		Invoke(nameof(Recharge), energyRechargeDelay);
	}
	
	private void Recharge()
	{
		Debug.Log("recharging");
		_isRecharging = true;
	}
}

[Serializable]
public class AxleInfo
{
	[SerializeField] private WheelCollider leftWheel;
	[SerializeField] private WheelCollider rightWheel;
	[SerializeField] private bool isMotor;
	[SerializeField] private bool isSteering;
	[SerializeField] private bool isReverseSteering;

	public WheelCollider LeftWheel => leftWheel;

	public WheelCollider RightWheel => rightWheel;

	public bool IsMotor => isMotor;

	public bool IsSteering => isSteering;

	public bool IsReverseSteering => isReverseSteering;
}