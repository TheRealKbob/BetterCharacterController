using UnityEngine;
using System.Collections;
using BetterCharacterControllerFramework;

public class InputManager : MonoBehaviour {

	private CharacterMotor controller;

	private Vector2 previousInput = Vector2.zero;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		if( x != 0 || z != 0 )
		{
			controller.Move( x, z );
			previousInput = new Vector2( x, z );
		}
		else
		{
			if( previousInput != Vector2.zero ) 
				controller.Move( 0, 0 );
			previousInput = Vector2.zero;
		}

		if( Input.GetKeyDown( KeyCode.Space ) )
			controller.Jump();

		if( Input.GetKeyDown( KeyCode.LeftShift ) )
			controller.Speed = 15;
		else if( Input.GetKeyUp( KeyCode.LeftShift ) )
			controller.Speed = 8;

	}
}
