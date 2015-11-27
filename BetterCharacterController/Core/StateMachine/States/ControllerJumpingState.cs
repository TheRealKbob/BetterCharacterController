using UnityEngine;
using System.Collections;

namespace BetterCharacterController
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
	}

}