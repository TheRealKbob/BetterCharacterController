using UnityEngine;
using System.Collections;

namespace BetterCharacterControllerFramework
{
	
	public class ControllerSlideState : ControllerState
	{
		
		public ControllerSlideState( ControllerStateMachine stateMachine, CharacterMotor controller ) : base( stateMachine, controller ){}
		
		public override void EnterState()
		{
			Debug.Log("Enter Slide State");
			controller.Locomotion.AddHorizontalForce( new Vector2( 0, 0 ) );
			controller.Locomotion.AddVerticalForce( 0 );
			controller.PlayerControl = false;
		}
		
		public override void OnUpdate()
		{
			if( !controller.Locomotion.Sliding )
			{
				stateMachine.CurrentState = ControllerStateType.IDLE;
				return;
			}
			
			controller.Locomotion.AddSlideForce();
			
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Slide State");
		}
		
	}
	
}