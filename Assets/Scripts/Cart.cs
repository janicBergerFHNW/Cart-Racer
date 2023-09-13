using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cart : MonoBehaviour
{
	public Vector3 ResetPosition { get; set; }
	public Quaternion ResetRotation { get; set; }
	private Vector2 _direction;
	[SerializeField]
	private float _speed = 80.0f;

	[SerializeField]
	private float MAX_STEERING_ANGLE = 45;

	[SerializeField] private List<AxleInfo> axles;

	private void Start()
	{
		ResetPosition = transform.position;
		ResetRotation = transform.rotation;
	}

	public void OnMove(InputValue inputValue)
	{
		_direction = inputValue.Get<Vector2>();
	}

	void OnPlayerJoined()
	{
		Debug.Log("Joined");
	}

	public void OnReset()
	{
		transform.position = ResetPosition;
		transform.rotation = ResetRotation;
		var rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}
	
	private void FixedUpdate()
	{
		//transform.position += Time.fixedDeltaTime * new Vector3(_direction.x, 0, _direction.y) * speed;
		var acceleration = _direction.y * _speed;
		var steering = _direction.x * MAX_STEERING_ANGLE;
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