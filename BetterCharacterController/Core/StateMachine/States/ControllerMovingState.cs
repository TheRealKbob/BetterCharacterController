using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerMovingState : ControllerState
	{
	
		public ControllerMovingState( ControllerStateMachine stateMachine, CharacterMotor controller, LocomotionController locomotion  ) : base( stateMachine, controller, locomotion ){}

		public override void EnterState()
		{
			Debug.Log("Enter Moving State");
			controller.PlayerControl = true;
			locomotion.ClampingEnabled = true;
		}
		
		public override void OnUpdate()
		{
			if( !locomotion.IsGrounded )
			{
				stateMachine.CurrentState = ControllerStateType.FALLING;
				return;
			}
			
			if( locomotion.Sliding )
			{
				stateMachine.CurrentState = ControllerStateType.SLIDING;
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
			
			locomotion.AddForce( new Vector3( ih.x, 0, ih.y ) );
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Moving");
		}

	}

}