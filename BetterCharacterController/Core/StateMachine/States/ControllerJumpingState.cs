using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerJumpingState : ControllerState
	{
	
		public ControllerJumpingState( ControllerStateMachine stateMachine, CharacterMotor controller, LocomotionController locomotion  ) : base( stateMachine, controller, locomotion ){}
		
		public override void EnterState()
		{
			Debug.Log("Enter Jumping State");
			locomotion.ClampingEnabled = false;
			locomotion.AddVerticalForce( 1 );
		}
		
		public override void OnUpdate()
		{
			if( locomotion.Velocity.y <= 0 )
			{
				stateMachine.CurrentState = ControllerStateType.FALLING;
				return;
			}
					
			if( controller.MovementWhileAirborne )
			{
				Vector2 ih = controller.HorizontalInput;
				locomotion.AddHorizontalForce( new Vector2( ih.x, ih.y ) );
			}
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Jumping");
		}
	}

}