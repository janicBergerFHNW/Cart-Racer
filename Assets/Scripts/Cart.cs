using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Cart : MonoBehaviour
{
	public class EngineStateChangeEventArgs
	{
		public EngineState EngineState;
	}

	public Vector3 ResetPosition { get; set; }
	public Quaternion ResetRotation { get; set; }
	private Vector2 _direction;
	[Header("Cart")]
	[FormerlySerializedAs("_speed")] [SerializeField]
	private float speed = 80.0f;
	[FormerlySerializedAs("MAX_STEERING_ANGLE")] [SerializeField]
	private float maxSteeringAngle = 45;
	[SerializeField] private List<AxleInfo> axles;
	[SerializeField] private float aerialRotationForce = 1000;
	
	[Space]
	[Header("Boost")]
	[SerializeField] private int energySupply = 2000;
	[SerializeField] private int energyRechargeDelay = 100;
	[FormerlySerializedAs("energyRecharge")] [SerializeField] private int energyRechargeRate = 100;
	[SerializeField] private Vector3 initialBoostVector = new Vector3(0, 1, 10000);
	[SerializeField] private int initialBoostEnergyConsumption = 20;
	[SerializeField] private float continuousBoostForce = 10;
	[SerializeField] private int continuousBoostEnergyConsumption = 1;
	[SerializeField] private float directionalBoostForce = 1000;
	private int _currentEnergy;
	private int _rechargeDelayTimeout;
	private EngineState _engineState = EngineState.Full;

	public EngineState EngineState
	{
		get
		{
			return _engineState;
		}
		set
		{
			_engineState = value;
			var ea = new EngineStateChangeEventArgs();
			ea.EngineState = _engineState;
			EngineStateChangeEvent?.Invoke(this, ea);
		}
	}
	public event EventHandler<EngineStateChangeEventArgs> EngineStateChangeEvent;
	public event EventHandler InitialBoostEvent;
	
	private float BackwardSpeed => speed * 0.7f;

	

	private Rigidbody _rigidbody;
	
	private void Start()
	{
		ResetPosition = transform.position;
		ResetRotation = transform.rotation;
		_rigidbody = GetComponent<Rigidbody>();
		_currentEnergy = energySupply;
		_rechargeDelayTimeout = energyRechargeDelay;
	}

	public void OnMove(InputAction.CallbackContext ctx)
	{
		_direction = ctx.ReadValue<Vector2>();
	}
	
	public void OnReset(InputAction.CallbackContext ctx)
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

		switch (_engineState)
		{
			case EngineState.Recharging:
				_currentEnergy += energyRechargeRate;
				if (_currentEnergy >= energySupply)
				{
					_currentEnergy = energySupply;
					_engineState = EngineState.Full;
				}
				break;
			case EngineState.RechargeDelay:
				if (--_rechargeDelayTimeout == 0)
					_engineState = EngineState.Recharging;
				break;
			case EngineState.Boost:
				if (_currentEnergy > 0)
				{
					_currentEnergy -= continuousBoostEnergyConsumption;
					_rigidbody.AddForce(transform.TransformDirection(Vector3.forward * continuousBoostForce));
				}
				if (_currentEnergy <= 0)
				{
					_currentEnergy = 0;
					_engineState = EngineState.RechargeDelay;
				}
				break;
		}

		_rigidbody.AddRelativeTorque(Vector3.right * (_direction.y * aerialRotationForce));
		if (!IsGrounded())
		{
			_rigidbody.AddRelativeTorque(Vector3.up * (_direction.x * aerialRotationForce));
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

	public bool IsGrounded()
	{
		foreach (var axle in axles)
		{
			if (axle.LeftWheel.isGrounded || axle.RightWheel.isGrounded)
				return true;
		}
		return false;
	}
	
	public void OnBoost(InputAction.CallbackContext ctx)
	{
		switch (ctx.phase)
		{
			case InputActionPhase.Canceled:
				if (_engineState == EngineState.Boost)
					_engineState = EngineState.RechargeDelay;
				break;
			case InputActionPhase.Started:
				if (_currentEnergy > 0)
				{
					InitialBoostEvent?.Invoke(this, EventArgs.Empty);
					_rigidbody.AddForce(
						transform.TransformDirection(
							initialBoostVector + Vector3.right * _direction.x * directionalBoostForce
							));
					_currentEnergy = Math.Max(_currentEnergy - initialBoostEnergyConsumption, 0);
					_rechargeDelayTimeout = energyRechargeDelay;
					_engineState = _currentEnergy > 0 ? 
						EngineState.Boost : EngineState.RechargeDelay;
				}
				break;
		}
	}

	public float Speed => _rigidbody.velocity.magnitude;

	public float CurrentEnergyRatio() => (float) _currentEnergy / energySupply;
	
	public float GetBackWheelSlip()
	{
		WheelCollider BL = axles[1].LeftWheel;
		WheelCollider BR = axles[1].RightWheel;

		if (!BL.isGrounded && !BR.isGrounded) return 0;

		if (BL.GetGroundHit(out WheelHit hitLeft))
		{
			float leftSlip = Math.Abs(hitLeft.sidewaysSlip);
			if (leftSlip > Math.Abs(BL.sidewaysFriction.extremumSlip))
			{
				return leftSlip;
			}
		}
		if (BL.GetGroundHit(out WheelHit hitRight))
		{
			float slip = Math.Abs(hitRight.sidewaysSlip);
			if (slip > Math.Abs(BR.sidewaysFriction.extremumSlip))
			{
				return slip;
			}
		}

		return 0;
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

public enum EngineState
{
	Boost,
	RechargeDelay,
	Recharging,
	Full
}