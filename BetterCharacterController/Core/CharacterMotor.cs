using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class CharacterMotor : MonoBehaviour
	{	
	
		#region controllers
		private ControllerStateMachine stateMachine;
		private LocomotionController locomotion;
		#endregion
		
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
		
		#region Getters/Setters
		
		public Vector3 Velocity{ get{ return locomotion.Velocity; } }
		
		private bool playerControl = true;
		public bool PlayerControl
		{ 
			set
			{ 
				if( value == true )
					playerControl = PlayerInputEnabled;
				else
				{
					playerControl = value;
					inputVector = Vector3.zero;
				}
			} 
			get{ return playerControl; }
		}
		#endregion
		
		#region Input
		private Vector3 inputVector = Vector3.zero;
		public Vector2 HorizontalInput{ get{ return new Vector2( inputVector.x, inputVector.z ); } }
		public float VerticalInput{ get{ return inputVector.y; } }
		#endregion
		
		void Start()
		{
			locomotion = new LocomotionController( this );
			stateMachine = new ControllerStateMachine( this, locomotion );
		}

		void FixedUpdate()
		{
			if( PlayerInputEnabled ) stateMachine.DoUpdate();
			locomotion.UpdatePhase();
			inputVector.y = 0;
		}
		
		public void Move( float x, float z ){ Move( new Vector2(x, z) ); }
		public void Move( Vector2 force )
		{
			if( !PlayerControl || !PlayerInputEnabled ) return;
			inputVector.x = force.x;
			inputVector.z = force.y;
		}

		public void Jump()
		{
			if( !PlayerControl || !PlayerInputEnabled ) return;
			inputVector.y = 1;
		}
		
		void OnControllerColliderHit (ControllerColliderHit hit) 
		{			
			locomotion.OnColliderHit( hit );
		}	
	}

}