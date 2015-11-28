using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerIdleState : ControllerState
	{
		
		public ControllerIdleState( ControllerStateMachine stateMachine, BetterCharacterController controller ) : base( stateMachine, controller )
		{
			this.stateMachine = stateMachine;
			this.controller = controller;
		}
		
		public override void EnterState()
		{
			Debug.Log("Enter Idle State");
			controller.EnableGroundClamping = true;
		}
		
		public override void OnUpdate()
		{
			if( controller.HorizontalInput != Vector2.zero )
				stateMachine.CurrentState = ControllerStateType.MOVING;
			else if( controller.VerticalInput > 0 )
				stateMachine.CurrentState = ControllerStateType.JUMPING;
			else if( !controller.Locomotion.MaintainingGround() )
				stateMachine.CurrentState = ControllerStateType.FALLING;
			else
				controller.Locomotion.ApplyFriction();
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Idle");
		}
		
	}

}