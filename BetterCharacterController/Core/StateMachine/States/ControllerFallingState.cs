using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerFallingState : ControllerState
	{
	
		public ControllerFallingState( ControllerStateMachine stateMachine, CharacterMotor controller ) : base( stateMachine, controller ){}
		
		public override void EnterState()
		{
			Debug.Log("Enter Falling");
			//controller.EnableGroundClamping = false;
			controller.Locomotion.ClampingEnabled = false;
			controller.Locomotion.AddVerticalForce( 0 );
		}
		
		public override void OnUpdate()
		{
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
			Debug.Log("Exit Falling");
		}
		
	}

}