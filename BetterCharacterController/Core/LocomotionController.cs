using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class LocomotionController
	{

		private CharacterMotor motor;
		private CharacterController controller;
		//private GroundController groundController;
		
		private Transform transform;
		
		private Vector3 moveVector = Vector3.zero;
		public Vector3 MoveForce{ get{ return moveVector * Time.deltaTime; } }

		#region Clamping Properties
		
		#endregion

		#region Physics Simulation
		public bool IsGrounded{ get{ return controller.isGrounded; } }
		public Vector3 Velocity{ get{ return controller.velocity; } }
		#endregion

		#region Pointers
		
		#endregion
		
		public LocomotionController( CharacterMotor motor )
		{
			this.motor = motor;
			transform = motor.gameObject.transform;
			intializeController();
		}

		public void UpdatePhase()
		{
			addGravity();
			moveVector = transform.TransformDirection( moveVector );
			controller.Move( MoveForce );
		}

		#region Movement Functions

		private void addGravity ()
		{
			moveVector.y -= 9.8f * Time.deltaTime;
		}
		
		public void AddForce ( Vector3 force )
		{
			AddHorizontalForce( new Vector2( force.x, force.z ) );
			AddVerticalForce( force.y );
		}
		
		public void AddHorizontalForce ( Vector2 force )
		{
			if( force.x != 0 && force.y != 0 )
				force *= 0.7f;
				
			moveVector.x = ( force.x != 0 ) ? (force.x * motor.Speed) : 0;
			moveVector.z = ( force.y != 0 ) ? (force.y * motor.Speed) : 0;
		}
		public void AddVerticalForce ( float force )
		{
			moveVector.y = force * motor.JumpHeight;
		}
		
		#endregion
		
		private void intializeController()
		{
			controller = motor.gameObject.GetComponent<CharacterController>();
			if( controller == null )
				controller = motor.gameObject.AddComponent<CharacterController>();
		}
		
	}

}