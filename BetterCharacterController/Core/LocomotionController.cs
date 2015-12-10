using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class LocomotionController
	{

		private CharacterMotor motor;
		private CharacterController controller;
		
		private Transform transform;
		
		private Vector3 lastGroundPosition = Vector3.zero;
		
		private Vector3 moveVector = Vector3.zero;
		public Vector3 MoveForce{ get{ return moveVector * Time.deltaTime; } }

		private ControllerColliderHit groundHit;
		public ControllerColliderHit GroundHit
		{
			set
			{
				if( groundHit == value ) return;
				groundHit = value;
				clampedTo = value.gameObject.transform;
			}
		}

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
		public float GroundAngle;
		private bool sliding = false;
		public bool Sliding{ get{ return sliding; } }
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
			transform = motor.gameObject.transform;
			intializeController();
		}

		public void UpdatePhase()
		{
			updateGroundClampPosition();
			if( sliding ) addSlideForce();
			addGravity();
			moveVector = transform.TransformDirection( moveVector );
			grounded = ( controller.Move( MoveForce ) & CollisionFlags.Below ) != 0;
			if( IsClamping )
				lastGroundPosition = clampedTo.position;
			else
				lastGroundPosition = Vector3.zero;
		}
		
		public void OnColliderHit(ControllerColliderHit hit) 
		{			
			RaycastHit rHit;
			if( Physics.Raycast(hit.point + Vector3.up, -Vector3.up, out rHit) )
			{
				float gAngle = Vector3.Angle(hit.normal, Vector3.up);
				GroundAngle = gAngle;
				//if( Mathf.Abs( gAngle ) <= controller.slopeLimit )
				GroundHit = hit;
				checkForSliding();
			}
			else
			{
				sliding = false;
			}
			
		}
		
		#region Movement Functions

		private void addGravity ()
		{
			antiBumpFactor = ( IsClamping ) ? 20f : 1f;
			moveVector.y -= ( motor.Gravity * antiBumpFactor ) * Time.deltaTime;
		}
		
		private void addSlideForce()
		{
			Vector3 hitNormal = groundHit.normal;
			moveVector = new Vector3( hitNormal.x, -hitNormal.y, hitNormal.z );
			Vector3.OrthoNormalize( ref hitNormal, ref moveVector );
			moveVector *= motor.Speed;
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

		private void checkForSliding ()
		{
			bool slidingWas = sliding;
			
			sliding = false;			
			if(groundHit == null) return;
			if( IsClamping && GroundAngle > controller.slopeLimit )
				sliding = true;
				
			if( slidingWas != sliding ) Debug.Log( "slidechange" );
		}
	}

}