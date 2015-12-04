using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerJumpingState : ControllerState
	{
	
		public ControllerJumpingState( ControllerStateMachine stateMachine, CharacterMotor controller ) : base( stateMachine, controller )
		{

		}
		
		public override void EnterState()
		{
			Debug.Log("Enter Jumping State");
//			controller.EnableGroundClamping = false;
			controller.Locomotion.AddVerticalForce( 1 );
		}
		
		public override void OnUpdate()
		{
		
			if( controller.Locomotion.Velocity.y <= 0 )
			{
				stateMachine.CurrentState = ControllerStateType.FALLING;
				return;
			}
		
			if( controller.Locomotion.IsGrounded )
			{
				stateMachine.CurrentState = ControllerStateType.IDLE;
				return;
			}
			
			if( controller.MovementWhileAirborne )
			{
				Vector2 ih = controller.HorizontalInput;
				controller.Locomotion.AddHorizontalForce( new Vector2( ih.x, ih.y ) );
			}
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Jumping");
		}
	}

}