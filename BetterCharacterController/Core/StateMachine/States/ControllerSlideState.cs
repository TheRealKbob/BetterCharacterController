using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{
	
	public class ControllerSlideState : ControllerState
	{
		
		public ControllerSlideState( ControllerStateMachine stateMachine, CharacterMotor controller, LocomotionController locomotion  ) : base( stateMachine, controller, locomotion ){}
		
		public override void EnterState()
		{
			locomotion.AddHorizontalForce( new Vector2( 0, 0 ) );
			locomotion.AddVerticalForce( 0 );
			controller.PlayerControl = false;
		}
		
		public override void OnUpdate()
		{
			if( !locomotion.Sliding )
			{
				stateMachine.CurrentState = ControllerStateType.IDLE;
				return;
			}
			
			locomotion.AddSlideForce();
			
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Slide State");
		}
		
	}
	
}