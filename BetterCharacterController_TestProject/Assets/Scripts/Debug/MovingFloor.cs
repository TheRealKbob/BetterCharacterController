using UnityEngine;
using System;
using System.Collections;

public class MovingFloor : MonoBehaviour {

	public float MoveSpeed = 5.0f;
	
	public float frequency = 20.0f;  // Speed of sine movement
	public float magnitude = 0.5f;   // Size of sine movement
	private Vector3 axis;

	private float angle;

	private Vector3 pos;
	
	void Start () {
		pos = transform.localPosition;
		angle = 0;
	}
	
	void FixedUpdate () {
		angle += frequency;
		if( angle >= 360 ) angle = 0;
		transform.localPosition = Vector3.Lerp( transform.localPosition, transform.localPosition + new Vector3(0, 0, Mathf.Sin(angle)) * magnitude, Time.deltaTime );
	}
}
