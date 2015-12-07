using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerIdleState : ControllerState
	{
		
		public ControllerIdleState( ControllerStateMachine stateMachine, CharacterMotor controller ) : base( stateMachine, controller ){}
		
		public override void EnterState()
		{
			Debug.Log("Enter Idle State");
			controller.Locomotion.ClampingEnabled = true;
			controller.Locomotion.AddHorizontalForce( new Vector2( 0, 0 ) );
		}
		
		public override void OnUpdate()
		{
		
			if( !controller.Locomotion.IsGrounded )
			{
				stateMachine.CurrentState = ControllerStateType.FALLING;
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