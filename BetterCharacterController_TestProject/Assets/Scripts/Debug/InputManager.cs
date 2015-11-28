using UnityEngine;
using System.Collections;
using BetterCharacterControllerFramework;

public class InputManager : MonoBehaviour {

	private BetterCharacterController controller;



	// Use this for initialization
	void Start () {
		controller = GetComponent<BetterCharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		if( x != 0 && z != 0 )
		{
			controller.Move( x, z );
		}

		if( Input.GetKeyUp( KeyCode.Space ) )
			controller.AddJump();

	}
}
