using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class CharacterMotor : MonoBehaviour
	{	
		private ControllerStateMachine stateMachine;
		private LocomotionController locomotion;
		public LocomotionController Locomotion{ get{ return locomotion; } }
		
		#region Editor Properties
		public LayerMask EnvironmentLayer;
		public float Gravity = 20f;
		public bool PlayerInputEnabled = true;
		public float Speed = 4;
		public float Acceleration = 10;
		public float JumpHeight = 8;
		public bool MovementWhileAirborne = true;
		public bool SlideDownSlopes = true;
		#endregion
		
		private Vector3 inputVector = Vector3.zero;
		public Vector2 HorizontalInput{ get{ return new Vector2( inputVector.x, inputVector.z ); } }
		public float VerticalInput{ get{ return inputVector.y; } }
		
		void Start()
		{
			locomotion = new LocomotionController( this );
			stateMachine = new ControllerStateMachine( this );
		}

		void FixedUpdate()
		{
			locomotion.UpdatePhase( 0 );
			if( PlayerInputEnabled ) stateMachine.DoUpdate();
			locomotion.UpdatePhase( 1 );
			inputVector.y = 0;
		}
		
		public void Move( float x, float z ){ Move( new Vector2(x, z) ); }
		public void Move( Vector2 force )
		{
			inputVector.x = force.x;
			inputVector.z = force.y;
		}

		public void Jump()
		{
			inputVector.y = 1;
		}
		
		void OnControllerColliderHit (ControllerColliderHit hit) 
		{
			locomotion.ClampedTo = hit.collider.gameObject.transform;
			
			RaycastHit rHit;
			if( Physics.Raycast(hit.point + Vector3.up, -Vector3.up, out rHit) )
			{
				locomotion.GroundAngle = Vector3.Angle(hit.normal, Vector3.up);
			}
		}	
	}

}