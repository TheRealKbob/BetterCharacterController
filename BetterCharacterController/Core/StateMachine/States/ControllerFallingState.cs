using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{

	public class ControllerFallingState : ControllerState
	{
	
		public ControllerFallingState( ControllerStateMachine stateMachine, BetterCharacterController controller ) : base( stateMachine, controller )
		{

		}
		
		public override void EnterState()
		{
			Debug.Log("Enter Falling");
			controller.EnableGroundClamping = false;
		}
		
		public override void OnUpdate()
		{
			if( controller.Locomotion.AcquiringGround() )
			{
				stateMachine.CurrentState = ControllerStateType.IDLE;
				return;
			}
			controller.Locomotion.ApplyForce( new Vector3( 0, -1, 0 ) );
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Falling");
		}
		
	}

}