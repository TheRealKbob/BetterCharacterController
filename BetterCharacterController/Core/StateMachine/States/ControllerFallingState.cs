using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerFallingState : ControllerState
	{
	
		public ControllerFallingState( ControllerStateMachine stateMachine, CharacterMotor controller, LocomotionController locomotion  ) : base( stateMachine, controller, locomotion ){}
		
		public override void EnterState()
		{
			Debug.Log("Enter Falling");
			locomotion.ClampingEnabled = false;
			locomotion.AddVerticalForce( 0 );
		}
		
		public override void OnUpdate()
		{
			Vector2 ih = controller.HorizontalInput;
			if( locomotion.IsGrounded )
			{
				if( ih == Vector2.zero )
					stateMachine.CurrentState = ControllerStateType.IDLE;
				else
					stateMachine.CurrentState = ControllerStateType.MOVING;
				return;
			}
			
			if( controller.MovementWhileAirborne )
			{
				locomotion.AddHorizontalForce( new Vector2( ih.x, ih.y ) );
			}
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Falling");
		}
		
	}

}