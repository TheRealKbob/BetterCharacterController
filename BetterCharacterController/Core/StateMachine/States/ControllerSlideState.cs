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
			//controller.EnableGroundClamping = false;
			controller.Locomotion.ClampingEnabled = false;
			controller.Locomotion.AddVerticalForce( 0 );
		}
		
		public override void OnUpdate()
		{
			
		}
		
		public override void ExitState()
		{
			Debug.Log("Exit Slide State");
		}
		
	}
	
}