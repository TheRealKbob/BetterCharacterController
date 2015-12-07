using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class LocomotionController
	{

		private CharacterMotor motor;
		private CharacterController controller;
		private GroundController groundController;
		
		private Transform transform;
		
		private Vector3 lastGroundPosition = Vector3.zero;
		
		private Vector3 moveVector = Vector3.zero;
		public Vector3 MoveForce{ get{ return moveVector * Time.deltaTime; } }

		#region Clamping Properties
		public bool ClampingEnabled = false;
		private Transform clampedTo = null;
		public Transform ClampedTo
		{
			set
			{
				if( clampedTo != value )
					clampedTo = value;
			}
			get
			{
				return clampedTo;
			}
		}
		private float antiBumpFactor = 1f;
		public bool IsClamping{ get{ return ( grounded && ClampingEnabled ); } }
		#endregion

		#region Physics Simulation
		private bool grounded = false;
		public bool IsGrounded
		{ 
			get
			{ 
				return grounded;
			} 
		}
		public Vector3 Velocity{ get{ return controller.velocity; } }
		#endregion
		
		#region Pointers
		public Vector3 Position{ get{ return motor.transform.position; } }
		public float Radius{ get{ return controller.radius; } }
		public Vector3 Up{ get{ return transform.up; } }
		#endregion
		
		public LocomotionController( CharacterMotor motor )
		{
			this.motor = motor;
			groundController = new GroundController( this, motor.EnvironmentLayer );
			transform = motor.gameObject.transform;
			intializeController();
		}

		public void UpdatePhase()
		{
			updateGroundClampPosition();
			groundController.Probe();
			addGravity();
			moveVector = transform.TransformDirection( moveVector );
			grounded = ( controller.Move( MoveForce ) & CollisionFlags.Below ) != 0;
			if( IsClamping )
				lastGroundPosition = clampedTo.position;
			else
				lastGroundPosition = Vector3.zero;
		}

		#region Movement Functions

		private void addGravity ()
		{
			antiBumpFactor = ( IsClamping ) ? 20f : 1f;
			moveVector.y -= ( motor.Gravity * antiBumpFactor ) * Time.deltaTime;
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

		private void updateGroundClampPosition ()
		{
			if( !IsClamping || lastGroundPosition == Vector3.zero ) return;
			
			transform.position += ( clampedTo.position - lastGroundPosition );
			
		}
	}

}