using UnityEngine;
using System.Collections;
using BetterCharacterControllerFramework;

public class InputManager : MonoBehaviour {

	private BetterCharacterController controller;

	private Vector2 previousInput = Vector2.zero;

	// Use this for initialization
	void Start () {
		controller = GetComponent<BetterCharacterController>();
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

		if( Input.GetKeyUp( KeyCode.Space ) )
			controller.Jump();

	}
}
