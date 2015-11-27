using UnityEngine;
using System.Collections;

namespace BetterCharacterController
{

	public class ControllerFallingState : ControllerState
	{
	
		private ControllerStateMachine stateMachine;
		private BetterCharacterController controller;
	
		public ControllerFallingState( ControllerStateMachine stateMachine, BetterCharacterController controller )
		{
			this.stateMachine = stateMachine;
			this.controller = controller;
		}
		
		public override void EnterState()
		{
			Debug.Log("Enter Falling");
		}
		
		public override void OnUpdate()
		{
			if( !controller.AcquiringGround() )
			{
				controller.AddGravity();
			}
			else
			{
				stateMachine.CurrentState = ControllerStateType.IDLE;
			}
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Falling");
		}
		
	}

}