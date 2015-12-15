using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerIdleState : ControllerState
	{
		
		public ControllerIdleState( ControllerStateMachine stateMachine, CharacterMotor controller, LocomotionController locomotion  ) : base( stateMachine, controller, locomotion ){}
		
		public override void EnterState()
		{
			Debug.Log("Enter Idle State");
			controller.PlayerControl = true;
			locomotion.ClampingEnabled = true;
			locomotion.AddHorizontalForce( new Vector2( 0, 0 ) );
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
		
			if( controller.HorizontalInput != Vector2.zero )
				stateMachine.CurrentState = ControllerStateType.MOVING;
			else if( controller.VerticalInput > 0 )
				stateMachine.CurrentState = ControllerStateType.JUMPING;
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Idle");
		}
		
	}

}