using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerIdleState : ControllerState
	{
		
		private ControllerStateMachine stateMachine;
		private BetterCharacterController controller;
		
		public ControllerIdleState( ControllerStateMachine stateMachine, BetterCharacterController controller )
		{
			this.stateMachine = stateMachine;
			this.controller = controller;
		}
		
		public override void EnterState()
		{
			Debug.Log("Enter Idle State at " + controller.Velocity.y + " velocity");
		}
		
		public override void OnUpdate()
		{
			if( !controller.MaintainingGround() )
			{
				stateMachine.CurrentState = ControllerStateType.FALLING;
				return;
			}			
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Idle");
		}
		
	}

}