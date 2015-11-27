using UnityEngine;
using System.Collections;

namespace BetterCharacterController
{

	public class ControllerMovingState : ControllerState
	{
	
		private ControllerStateMachine stateMachine;
		private BetterCharacterController controller;
	
		public ControllerMovingState( ControllerStateMachine stateMachine, BetterCharacterController controller )
		{
			this.stateMachine = stateMachine;
			this.controller = controller;
		}
	}

}