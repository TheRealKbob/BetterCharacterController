using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerMovingState : ControllerState
	{
	
		public ControllerMovingState( ControllerStateMachine stateMachine, CharacterMotor controller ) : base( stateMachine, controller ){}

		public override void EnterState()
		{
			Debug.Log("Enter Moving State");
			controller.Locomotion.ClampingEnabled = true;
		}
		
		public override void OnUpdate()
		{
			if( !controller.Locomotion.IsGrounded )
			{
				stateMachine.CurrentState = ControllerStateType.FALLING;
				return;
			}
			
			Vector2 ih = controller.HorizontalInput;
			float iv = controller.VerticalInput;
			if( ih == Vector2.zero )
			{
				stateMachine.CurrentState = ControllerStateType.IDLE;
				return;
			}
			
			if( iv > 0 )
			{
				stateMachine.CurrentState = ControllerStateType.JUMPING;
				return;
			}
			
			controller.Locomotion.AddForce( new Vector3( ih.x, 0, ih.y ) );
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Moving");
		}

	}

}