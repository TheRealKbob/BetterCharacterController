using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerJumpingState : ControllerState
	{
	
		private ControllerStateMachine stateMachine;
		private BetterCharacterController controller;
	
		public ControllerJumpingState( ControllerStateMachine stateMachine, BetterCharacterController controller )
		{
			this.stateMachine = stateMachine;
			this.controller = controller;
		}
		
		public override void OnUpdate()
		{
			if( !controller.AcquiringGround() )
			{
				controller.AddGravity();
				if( controller.Velocity.y < 0 )
					stateMachine.CurrentState = ControllerStateType.FALLING;
			}
			else
			{
				stateMachine.CurrentState = ControllerStateType.IDLE;
			}
		}
	}

}