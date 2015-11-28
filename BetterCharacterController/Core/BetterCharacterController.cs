using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class BetterCharacterController : MonoBehaviour
	{

		#region Editor Properties
		public Vector3 DebugMove = Vector3.zero;

		public float Radius = 0.5f;
		public LayerMask EnvironmentLayer = 0;

		//Gravity
		public float GravityScale = 1f;
		private const float gravitationalAcceleration = 9.81f;
		public float Gravity{  get { return GravityScale * gravitationalAcceleration; } }

		//Player Control
		public float Speed = 4f;
		public float Acceleration = 30f;
		public float Decceleration = 100f;
		public float Friction{ get{ return ( 1 - (Decceleration / 100) ); } }
		public float JumpHeight = 3;
		#endregion

		private ControllerStateMachine stateMachine;
		private LocomotionController locomotion;
		public LocomotionController Locomotion{ get{ return locomotion; } }
	
		private Vector3 inputVector = Vector3.zero;
		public Vector3 InputVector{ get{ return inputVector; } }
		public Vector2 HorizontalInput{ get{ return new Vector2(inputVector.x, inputVector.z); } }
		public float VerticalInput{ get{ return inputVector.y; } }

		public bool EnableGroundClamping = false;

		void Start()
		{
			stateMachine = new ControllerStateMachine( this );
			locomotion = new LocomotionController( this );
		}

		void FixedUpdate()
		{
			locomotion.UpdatePhase( 1 );
			stateMachine.DoUpdate();
			inputVector = Vector3.zero;
			locomotion.UpdatePhase( 2 );
		}
		
		public void Move( float x, float z ){ Move( new Vector2(x, z) ); }
		public void Move( Vector2 force )
		{
			Vector3 f = new Vector3( force.x, inputVector.y, force.y );
			inputVector = f;
		}

		public void Jump()
		{
			inputVector = new Vector3( inputVector.x, 1, inputVector.z );
		}
	}

}